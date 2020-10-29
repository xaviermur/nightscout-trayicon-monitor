using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NightscoutTrayIconMonitor
{

    public class MonitorData
    {
        public int SGV { get; set; }
        public string Direction { get; set; }
        public DateTime ServerDateTime { get; set; }
        public DateTime LastReadDateTime { get; set; }
        public string Response { get; set; }
        public MonitorData(int sgv, string direction, DateTime serverDateTime, DateTime lastReadDateTime, string response)
        {
            SGV = sgv;
            Direction = direction;
            ServerDateTime = serverDateTime;
            LastReadDateTime = lastReadDateTime;
            Response = response;
        }
    }

    class MyApplicationContext : ApplicationContext
    {
        //Component declarations
        private NotifyIcon TrayIcon;
        private ContextMenuStrip TrayIconContextMenu;
        private ToolStripMenuItem CloseMenuItem;
        private int LastReadSGV;
        private DateTime LastReadDateTime;
        private enum TypeLog { DEBUG, INFO, ERROR };
        public MyApplicationContext()
        {
            Application.ApplicationExit += new EventHandler(this.OnApplicationExit);

            InitializeComponent();

            TrayIcon.Visible = true;
        }

        private void InitializeComponent()
        {
            TrayIcon = new NotifyIcon();

            TrayIcon.BalloonTipIcon = ToolTipIcon.Info;
            //TrayIcon.BalloonTipText = "I noticed that you double-clicked me! What can I do for you?";
            //TrayIcon.BalloonTipTitle = "You called Master?";
            //TrayIcon.Text = "My fabulous tray icon demo application";

            //The icon is added to the project resources. Here I assume that the name of the file is 'TrayIcon.ico'
            TrayIcon.Icon = Properties.Resources.BloodIcon;

            //Optional - handle doubleclicks on the icon:
            TrayIcon.Click += TrayIcon_Click;

            //Optional - Add a context menu to the TrayIcon:
            TrayIconContextMenu = new ContextMenuStrip();
            CloseMenuItem = new ToolStripMenuItem();
            TrayIconContextMenu.SuspendLayout();

            // 
            // TrayIconContextMenu
            // 
            this.TrayIconContextMenu.Items.AddRange(new ToolStripItem[] {
            this.CloseMenuItem});
            this.TrayIconContextMenu.Name = "TrayIconContextMenu";
            this.TrayIconContextMenu.Size = new Size(250, 70);
            // 
            // CloseMenuItem
            // 
            this.CloseMenuItem.Name = "CloseMenuItem";
            this.CloseMenuItem.Size = new Size(152, 22);
            this.CloseMenuItem.Text = "Close nighscout remote monitor tray icon program";
            this.CloseMenuItem.Click += new EventHandler(this.CloseMenuItem_Click);

            TrayIconContextMenu.ResumeLayout(false);

            TrayIcon.ContextMenuStrip = TrayIconContextMenu;

            RefreshState();
            Timer timer = new Timer();
            if (int.TryParse(System.Configuration.ConfigurationManager.AppSettings["Interval"], out int interval)) {
                timer.Interval = (interval * 1000); // 10 secs
                timer.Tick += new EventHandler(RefreshState_Tick);
                timer.Start();
            }
            else
            {
                MessageBox.Show("Please, update app.config interval value and restart again.", "Wrong interval value", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            //Cleanup so that the icon will be removed when the application is closed
            TrayIcon.Visible = false;
        }

        private void TrayIcon_Click(object sender, EventArgs e)
        {
            try
            {
                MonitorData md = GetMonitorData();
                if (!LastReadDateTime.Equals(md.LastReadDateTime))
                {
                    LastReadSGV = md.SGV;
                    LastReadDateTime = md.LastReadDateTime.AddHours(-1);
                }
                string info = String.Join(
                    Environment.NewLine,
                    md.SGV.ToString(),
                    md.Direction,
                    (md.SGV - LastReadSGV).ToString(),
                    ((md.ServerDateTime).Subtract(LastReadDateTime)).Minutes + "m.",
                    md.Response
                );
                TrayIcon.BalloonTipText = info;
                TrayIcon.BalloonTipTitle = "Server response";
                TrayIcon.ShowBalloonTip(10000);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can't update info from server.", "Server error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                WriteLog("Can't refresh state.", TypeLog.ERROR, ex);
            }
        }

        private void CloseMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to close me?",
                                "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                                MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void RefreshState_Tick(object sender, EventArgs e)
        {
            RefreshState();
        }

        private void RefreshState()
        {
            try
            {
                MonitorData md = GetMonitorData();
                if (!LastReadDateTime.Equals(md.LastReadDateTime))
                {
                    LastReadSGV = md.SGV;
                    LastReadDateTime = md.LastReadDateTime.AddHours(-1);
                }
                TrayIcon.Icon = GetIcon(md.SGV);
                TrayIcon.Text = String.Join("  ",
                    md.SGV.ToString(),
                    md.Direction,
                    (md.SGV - LastReadSGV).ToString(),
                    ((md.ServerDateTime).Subtract(LastReadDateTime)).Minutes + "m."
                );

            }
            catch (Exception ex)
            {
                WriteLog("Can't refresh state.", TypeLog.ERROR, ex);
            }
        }

        private MonitorData GetMonitorData()
        {
            string[] response = Regex.Split(GetServerResponse(), @"\s+");
            if (response != null && response.Length >= 5)
            {
                if (int.TryParse(response[2], out int rgv))
                {
                    string direction = response[3].Replace("\"", "");
                    DateTime dtServer = GetServerTime();
                    DateTime dtNow = DateTime.Parse(response[0].Replace("\"", ""));
                    return new MonitorData(rgv, direction, dtServer, dtNow, string.Join(" - ", response));
                }
                else
                {
                    throw new Exception("Invalid response. Unexpected RGV response: " + response[2]);
                }
            }
            else
            {
                throw new Exception("Invalid response. Invalid number of parameters recovered: " + string.Join(" ", response));
            }
        }

        private void WriteLog(string message,TypeLog typeLog, Exception e = null)
        {
            Logger log = LogManager.GetLogger(typeLog == TypeLog.DEBUG ? "debugLogger" : "appLogger");
            switch(typeLog)
            {
                case TypeLog.DEBUG:
                    {
                        log.Debug(message);
                        break;
                    }
                case TypeLog.INFO:
                    {
                        log.Info(message);
                        break;
                    }
                case TypeLog.ERROR:
                    {
                        log.Error(e,message);
                        break;
                    }
                default: return;
            }
        }

        private string GetServerResponse()
        {
            WebClient wc = new WebClient();
            string result = wc.DownloadString(System.Configuration.ConfigurationManager.AppSettings["Server"] + "entries")
                .Split(Environment.NewLine.ToCharArray()).FirstOrDefault();
            return result;
        }

        private DateTime GetServerTime()
        {
            WebClient wc = new WebClient();
            string result = wc.DownloadString(System.Configuration.ConfigurationManager.AppSettings["Server"] + "status.json");
            //dynamic objResult = JsonConvert.DeserializeObject(result);
            JObject objResult = JObject.Parse(result);
            if (DateTime.TryParse(objResult["serverTime"].ToString(), out DateTime dtResult))
            {
                return dtResult;
            }
            else
            {
                throw new Exception("Can't get server time. Server data: " + result);
            }
        }

        private Icon GetIcon(int value)
        {
            String str = value.ToString();

            //white pen to draw some lines in the icon
            Pen pen = new Pen(Color.White);

            //white brush for the text
            SolidBrush brush = new SolidBrush(Color.White);

            //create a small font to write the values in the bitmap
            Font font = new Font("Tahoma", 8.5F);

            //origin for the string
            PointF origin = new PointF(-3.2F, 2F);

            //create a new bitmap with 16x16 pixel
            Bitmap bitmap = new Bitmap(16, 16);

            //use the bitmap to draw
            Graphics graph = Graphics.FromImage(bitmap);

            //draw two horizontale lines
            //graph.DrawLine(pen, 0, 15, 15, 15);
            //graph.DrawLine(pen, 0, 0, 15, 0);

            //draw the string including the value
            graph.DrawString(str, font, brush, origin);

            //get an Hicon from the bitmap
            IntPtr Hicon = bitmap.GetHicon();

            //clean up
            pen.Dispose();
            brush.Dispose();
            font.Dispose();
            bitmap.Dispose();
            graph.Dispose();

            //create a new icon from the handle
            return Icon.FromHandle(Hicon);
        }

    }
}
