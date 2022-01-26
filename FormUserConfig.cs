using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NightscoutTrayIconMonitor
{
    public partial class FormUserConfig : Form
    {

        public event EventHandler OnClose;
        public FormUserConfig()
        {
            InitializeComponent();
            UserConfig uc = new UserConfig();
            uc.Read();
            tbNameServer.Text = uc.Data.Server ?? System.Configuration.ConfigurationManager.AppSettings["Server"];
            tbTokenServer.Text = uc.Data.Token ?? System.Configuration.ConfigurationManager.AppSettings["Token"];
            tbTimer.Text = uc.Data.Interval > 0 ? uc.Data.Interval.ToString() : System.Configuration.ConfigurationManager.AppSettings["Interval"];
            String unitsType = uc.Data.Units ?? System.Configuration.ConfigurationManager.AppSettings["Units"];
            bool isUnitsMmol = (unitsType == "mmol") ? true : false;
            rbUnitsMmol.Checked = isUnitsMmol;
            rbUnitsMg.Checked = !isUnitsMmol;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            UserConfig uc = new UserConfig(tbNameServer.Text, tbTokenServer.Text, tbTimer.Text, rbUnitsMmol.Checked ? "mmol" : "mg");
            uc.Save();
            if (OnClose != null) OnClose(this, EventArgs.Empty);
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (OnClose != null) OnClose(this, EventArgs.Empty);
            this.Close();
        }

    }
}
