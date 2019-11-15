using System;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Principal;
using System.Diagnostics;
using System.IO.Compression;

using OverloadClientTool;

namespace OverloadClientApplication
{
    public partial class OlmodUpdateForm : Form
    {
        public OlmodUpdateForm(string currentVersion, string newVersion)
        {
            InitializeComponent();

            OlmodCurrentVersion.Text = "Your installed version is " + currentVersion;
            OlmodNewVersion.Text     = "Current online version is " + newVersion;

            if (currentVersion == newVersion)
            {
                UpdateQuestion.Text = "No new release - install anyway?";
                SkipUpdateButton.Text = "No";
            }
            else
            {
                SkipUpdateButton.Text = "Not now";
            }
        }

        private void UpgradeButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void DeclineUpgrade_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
