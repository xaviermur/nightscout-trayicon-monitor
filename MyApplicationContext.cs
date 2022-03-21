using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NightscoutTrayIconMonitor
{

    public class MonitorData
    {
        public decimal SGV { get; set; }
        public decimal SGVOld { get; set; }
        public string Direction { get; set; }
        public DateTime ServerDateTime { get; set; }
        public DateTime LastReadDateTime { get; set; }
        public string Response { get; set; }
        public MonitorData(decimal sgv, decimal sgvOld, string direction, DateTime serverDateTime, DateTime lastReadDateTime, string response)
        {
            SGV = sgv;
            SGVOld = sgvOld;
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
        private ToolStripMenuItem ConfigMenuItem;
        private ToolStripMenuItem CloseMenuItem;
        private DateTime LastReadDateTime;
        private Timer Timer;
        public UserConfig Config { get; set; }

        private enum TypeLog { DEBUG, INFO, ERROR };
        public MyApplicationContext()
        {
            Application.ApplicationExit += new EventHandler(this.OnApplicationExit);

            InitializeComponent();
            Config = new UserConfig();
            Config.Read();

            try
            {
                MonitorData _md = GetMonitorData();
                StartMonitor();
            }
            catch (Exception ex)
            {
                FormUserConfig _formUserConfig = new FormUserConfig();
                _formUserConfig.OnClose += _formUserConfig_OnClose;
                _formUserConfig.Show();
            }

            TrayIcon.Visible = true;
        }

        private void _formUserConfig_OnClose(object sender, EventArgs e)
        {
            Config = new UserConfig();
            Config.Read();

            try
            {
                MonitorData _md = GetMonitorData();
                StartMonitor();
            }
            catch (Exception ex)
            {
                FormUserConfig _formUserConfig = new FormUserConfig();
                _formUserConfig.OnClose += _formUserConfig_OnClose;
                _formUserConfig.Show();
            }
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
            TrayIcon.MouseClick += TrayIcon_Click;

            //Optional - Add a context menu to the TrayIcon:
            TrayIconContextMenu = new ContextMenuStrip();
            ConfigMenuItem = new ToolStripMenuItem();
            CloseMenuItem = new ToolStripMenuItem();
            TrayIconContextMenu.SuspendLayout();

            // 
            // TrayIconContextMenu
            // 
            this.TrayIconContextMenu.Items.AddRange(new ToolStripItem[] {
                this.ConfigMenuItem,this.CloseMenuItem
            });
            this.TrayIconContextMenu.Name = "TrayIconContextMenu";
            this.TrayIconContextMenu.Size = new Size(250, 70);
            // 
            // ConfigMenuItem
            // 
            this.ConfigMenuItem.Name = "ConfigMenuItem";
            this.ConfigMenuItem.Size = new Size(152, 22);
            this.ConfigMenuItem.Text = "Config";
            this.ConfigMenuItem.Click += new EventHandler(this.ConfigMenuItem_Click);
            // 
            // CloseMenuItem
            // 
            this.CloseMenuItem.Name = "CloseMenuItem";
            this.CloseMenuItem.Size = new Size(152, 22);
            this.CloseMenuItem.Text = "Exit";
            this.CloseMenuItem.Click += new EventHandler(this.CloseMenuItem_Click);

            TrayIconContextMenu.ResumeLayout(false);
            TrayIcon.ContextMenuStrip = TrayIconContextMenu;
            Timer = new Timer();
        }

        private void StartMonitor()
        {
            if (string.IsNullOrEmpty(Config.Data.Server) || string.IsNullOrEmpty(Config.Data.Token))
            {
                MessageBox.Show("Please, update app.config server and token value and restart again.", "Wrong server info",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            if (Timer.Enabled)
            {
                Timer.Stop();
            }
            RefreshState();
            if (Config.Data.Interval > 0)
            {
                Timer.Interval = (Config.Data.Interval * 1000); // 10 secs
                Timer.Tick += new EventHandler(RefreshState_Tick);
                Timer.Start();
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

        private string GetFormatSGV(decimal value)
        {
            return Config.Data.Units == "mmol" ? value.ToString("0.0") : ((int)value).ToString();
        }

        private string GetOffset(decimal current, decimal previous)
        {
            string symbol = (current >= previous) ? "+" : "";
            return symbol + GetFormatSGV(current - previous);
        }

        private void TrayIcon_Click(object sender, MouseEventArgs e)
        {
            try
            {
                if(e.Button == MouseButtons.Left)
                {
                    /*MonitorData md = GetMonitorData();
                    if (!LastReadDateTime.Equals(md.LastReadDateTime))
                    {
                        LastReadDateTime = md.LastReadDateTime.AddHours(-1);
                    }
                    string info = String.Join(
                        Environment.NewLine,
                        GetFormatSGV(md.SGV) + " (" + GetOffset(md.SGV,md.SGVOld) + ")",
                        md.Direction,
                        ((md.ServerDateTime).Subtract(LastReadDateTime)).Minutes + "m.",
                        md.Response
                    );
                    TrayIcon.BalloonTipText = info;
                    TrayIcon.BalloonTipTitle = "Server response";
                    TrayIcon.ShowBalloonTip(10000);*/
                    Uri myUri = new Uri(Config.Data.Server);
                    Process.Start(myUri.GetLeftPart(UriPartial.Authority));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can't update info from server.", "Server error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                WriteLog("Can't refresh state.", TypeLog.ERROR, ex);
            }
        }

        private void ConfigMenuItem_Click(object sender, EventArgs e)
        {
            if (Timer.Enabled) Timer.Stop();
            FormUserConfig _formUserConfig = new FormUserConfig();
            _formUserConfig.OnClose += _formUserConfig_OnClose;
            _formUserConfig.Show();
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
                    LastReadDateTime = md.LastReadDateTime.AddHours(-1);
                }
                TrayIcon.Icon = GetIcon(md.SGV);
                TrayIcon.Text = String.Join("  ",
                    GetFormatSGV(md.SGV),
                    md.Direction,
                    GetOffset(md.SGV,md.SGVOld),
                    ((md.ServerDateTime).Subtract(LastReadDateTime)).Minutes + "m."
                );

            }
            catch (Exception ex)
            {
                WriteLog("Can't refresh state.", TypeLog.ERROR, ex);
            }
        }

        private string[] GetEntriesData(string data)
        {
            string[] response = Regex.Split(data, @"\s+");
            if (response != null && response.Length >= 5)
            {
                if (int.TryParse(response[2], out int rgv))
                {
                    string direction = response[3].Replace("\"", "");
                    return new string[] { response[0], rgv.ToString(), direction };
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

        private MonitorData GetMonitorData()
        {
            string[] response = GetServerResponse();
            if (response != null && response.Length > 1)
            {
                string[] currentResponse = GetEntriesData(response[0]);
                string[] previousResponse = GetEntriesData(response[1]);
                if (!decimal.TryParse(currentResponse[1], out decimal rgv))
                {
                    throw new Exception("Invalid response. Unexpected RGV response: " + currentResponse[2]);
                }
                if (!decimal.TryParse(previousResponse[1], out decimal previousRgv))
                {
                    throw new Exception("Invalid response. Unexpected previous RGV response: " + previousResponse[2]);
                }
                decimal frgv = (Config.Data.Units == "mmol") ? rgv / 18 : rgv;
                decimal fpreviousrgv = (Config.Data.Units == "mmol") ? previousRgv / 18 : previousRgv;
                string direction = currentResponse[2].Replace("\"", "");
                DateTime dtServer = GetServerTime();
                DateTime dtNow = DateTime.Parse(currentResponse[0].Replace("\"", ""));
                return new MonitorData(frgv, fpreviousrgv, direction, dtServer, dtNow, string.Join(" - ", currentResponse));

            }
            else
            {
                throw new Exception("Invalid response. Couldn't get any entries: " + string.Join(" ", response.ToString()));
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

        private string GetApiPath(string service)
        {
            return Config.Data.Server + service + "?token=" + Config.Data.Token;
        }

        private string[] GetServerResponse()
        {
            WebClient wc = new WebClient();
            string[] result = wc.DownloadString(GetApiPath("entries"))
                .Split(Environment.NewLine.ToCharArray()).Where(x => !string.IsNullOrEmpty(x)).ToArray();
            return result;
        }

        private DateTime GetServerTime()
        {
            WebClient wc = new WebClient();
            string result = wc.DownloadString(GetApiPath("status.json"));
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

        private Icon GetIcon(decimal value)
        {
            String str = GetFormatSGV(value);

            //white pen to draw some lines in the icon
            Pen pen = new Pen(Color.White);

            //white brush for the text
            SolidBrush brush = new SolidBrush(Color.White);

            //create a small font to write the values in the bitmap
            Font font = (Config.Data.Units == "mmol") ? new Font("Tahoma", 7.5F) : new Font("Tahoma", 8.5F);

            //origin for the string
            PointF origin = (Config.Data.Units == "mmol") ? new PointF(-3.5F, 2F) : new PointF(-3.2F, 2F);

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
