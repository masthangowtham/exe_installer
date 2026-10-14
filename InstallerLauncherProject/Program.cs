using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace InstallerLauncher
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new LauncherForm());
        }
    }

    public sealed class LauncherForm : Form
    {
        private readonly TextBox pathBox = new();
        private readonly Label status = new();

        public LauncherForm()
        {
            Text = "Installer Launcher";
            Width = 680;
            Height = 240;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            var label = new Label
            {
                Text = "Select an installer (.exe or .msi):",
                Left = 18, Top = 20, Width = 400, Height = 24
            };
            Controls.Add(label);

            pathBox.Left = 18;
            pathBox.Top = 52;
            pathBox.Width = 500;
            pathBox.ReadOnly = true;
            Controls.Add(pathBox);

            var browse = new Button
            {
                Text = "Browse...",
                Left = 530, Top = 49, Width = 110, Height = 30
            };
            browse.Click += Browse_Click;
            Controls.Add(browse);

            var launch = new Button
            {
                Text = "Launch Installer",
                Left = 18, Top = 100, Width = 160, Height = 38
            };
            launch.Click += Launch_Click;
            Controls.Add(launch);

            status.Left = 18;
            status.Top = 155;
            status.Width = 620;
            status.Height = 35;
            status.Text = "Select an installer to begin.";
            Controls.Add(status);
        }

        private void Browse_Click(object? sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Title = "Select an installer",
                Filter = "Installers (*.exe;*.msi)|*.exe;*.msi|All files (*.*)|*.*",
                CheckFileExists = true
            };

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                string ext = Path.GetExtension(dialog.FileName).ToLowerInvariant();
                if (ext != ".exe" && ext != ".msi")
                {
                    MessageBox.Show(this, "Please select an .exe or .msi installer.",
                        "Unsupported file", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                pathBox.Text = dialog.FileName;
                status.Text = "Ready to launch. Windows/your organization may request authorization.";
            }
        }

        private void Launch_Click(object? sender, EventArgs e)
        {
            string file = pathBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(file) || !File.Exists(file))
            {
                MessageBox.Show(this, "Please select an existing .exe or .msi file.",
                    "No installer selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string ext = Path.GetExtension(file).ToLowerInvariant();
            try
            {
                ProcessStartInfo psi;
                if (ext == ".msi")
                {
                    // Launch MSI via Windows Installer. Elevation is governed by Windows and IT policy.
                    psi = new ProcessStartInfo
                    {
                        FileName = "msiexec.exe",
                        Arguments = "/i " + Quote(file),
                        UseShellExecute = true
                    };
                }
                else if (ext == ".exe")
                {
                    psi = new ProcessStartInfo
                    {
                        FileName = file,
                        WorkingDirectory = Path.GetDirectoryName(file) ?? Environment.CurrentDirectory,
                        UseShellExecute = true
                    };
                }
                else
                {
                    MessageBox.Show(this, "Only .exe and .msi files are supported.",
                        "Unsupported file", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Process.Start(psi);
                status.Text = "Installer launch requested. Follow any Windows or company prompts.";
            }
            catch (Exception ex)
            {
                status.Text = "Launch failed.";
                MessageBox.Show(this, "Could not launch the installer:\r\n" + ex.Message,
                    "Launch error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string Quote(string value) => "\"" + value.Replace("\"", "\\\"") + "\"";
    }
}
