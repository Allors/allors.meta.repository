// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeneratingDialog.cs" company="Allors bvba">
//   Copyright 2002-2009 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.R1.Development.Repository.WinForms.Dialogs
{
    using System;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    using Allors.R1.Development.Repository.Generation;

    public class GeneratingDialog : Form
    {
        private Button button;
        private volatile bool cancellationPending;
        private Label currentTemplateLabel;
        private volatile Label currentTemplateNameLabel;
        private volatile Template[] templates;
        private GroupBox groupBox1;
        private ProgressBar progressBar;
        private GroupBox progressGroupBox;
        private RichTextBox reportRichTextBox;
        private volatile Thread worker;

        public GeneratingDialog(Template[] templates)
        {
            this.InitializeComponent();
            this.templates = templates;
            this.cancellationPending = false;

            this.worker = new Thread(this.DoWork);
            this.worker.Start();
        }

        private delegate void TemplateStartedDelegate(Template currentTemplate);

        private delegate void PercentCompletedDelegate(int percentCompleted);

        private delegate void ReportDelegate(string reportLine);

        private delegate void WorkerFinishedDelegate();

        private void ButtonClick(object sender, EventArgs e)
        {
            if (this.worker.ThreadState.Equals(ThreadState.Running))
            {
                this.cancellationPending = true;
            }
            else
            {
                this.Close();
            }
        }

        private void DoWork()
        {
            if (this.templates.Length > 0)
            {
                var log = new Log(this);
                try
                {
                    //TODO: Windows handle not created error!
                    Thread.Sleep(1000);

                    int percentIncrement = 100 / this.templates.Length;
                    int percentComplete = 0;

                    foreach (Template template in this.templates)
                    {
                        if (this.cancellationPending)
                        {
                            return;
                        }

                        this.Invoke(new TemplateStartedDelegate(this.GenerateStarted), new object[] { template });

                        template.Generate(log);

                        percentComplete += percentIncrement;
                        this.Invoke(new PercentCompletedDelegate(this.PercentCompleted), new object[] { percentComplete });
                    }
                }
                catch (Exception e)
                {
                    log.Error(this, e.Message);
                }
                finally
                {
                    this.Invoke(new WorkerFinishedDelegate(this.WorkerFinished), null);
                }
            }
        }

        private void GenerateStarted(Template currentTemplate)
        {
            this.currentTemplateNameLabel.Text = currentTemplate.Name;
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.progressGroupBox = new System.Windows.Forms.GroupBox();
            this.currentTemplateNameLabel = new System.Windows.Forms.Label();
            this.currentTemplateLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.reportRichTextBox = new System.Windows.Forms.RichTextBox();
            this.progressGroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button
            // 
            this.button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button.Location = new System.Drawing.Point(328, 261);
            this.button.Name = "button";
            this.button.Size = new System.Drawing.Size(90, 26);
            this.button.TabIndex = 11;
            this.button.Text = "Cancel";
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(23, 32);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(370, 27);
            this.progressBar.TabIndex = 12;
            // 
            // progressGroupBox
            // 
            this.progressGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressGroupBox.Controls.Add(this.currentTemplateNameLabel);
            this.progressGroupBox.Controls.Add(this.currentTemplateLabel);
            this.progressGroupBox.Controls.Add(this.progressBar);
            this.progressGroupBox.Location = new System.Drawing.Point(25, 144);
            this.progressGroupBox.Name = "progressGroupBox";
            this.progressGroupBox.Size = new System.Drawing.Size(417, 108);
            this.progressGroupBox.TabIndex = 14;
            this.progressGroupBox.TabStop = false;
            this.progressGroupBox.Text = "Progress";
            // 
            // currentTemplateNameLabel
            // 
            this.currentTemplateNameLabel.AutoSize = true;
            this.currentTemplateNameLabel.Location = new System.Drawing.Point(139, 75);
            this.currentTemplateNameLabel.Name = "currentTemplateNameLabel";
            this.currentTemplateNameLabel.Size = new System.Drawing.Size(0, 17);
            this.currentTemplateNameLabel.TabIndex = 14;
            // 
            // currentTemplateLabel
            // 
            this.currentTemplateLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.currentTemplateLabel.AutoSize = true;
            this.currentTemplateLabel.Location = new System.Drawing.Point(19, 75);
            this.currentTemplateLabel.Name = "currentTemplateLabel";
            this.currentTemplateLabel.Size = new System.Drawing.Size(122, 17);
            this.currentTemplateLabel.TabIndex = 13;
            this.currentTemplateLabel.Text = "Current Template:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.reportRichTextBox);
            this.groupBox1.Location = new System.Drawing.Point(25, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(417, 123);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Report";
            // 
            // reportRichTextBox
            // 
            this.reportRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportRichTextBox.Location = new System.Drawing.Point(3, 18);
            this.reportRichTextBox.Name = "reportRichTextBox";
            this.reportRichTextBox.Size = new System.Drawing.Size(411, 102);
            this.reportRichTextBox.TabIndex = 0;
            this.reportRichTextBox.Text = "";
            // 
            // GeneratingDialog
            // 
            this.CancelButton = this.button;
            this.ClientSize = new System.Drawing.Size(459, 299);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.progressGroupBox);
            this.Controls.Add(this.button);
            this.Name = "GeneratingDialog";
            this.Text = "Allors - Generating";
            this.progressGroupBox.ResumeLayout(false);
            this.progressGroupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void PercentCompleted(int percentComplete)
        {
            this.progressBar.Value = percentComplete;
        }

        private void Report(string reportLine)
        {
            this.reportRichTextBox.Text = this.reportRichTextBox.Text + reportLine;
        }

        private void WorkerFinished()
        {
            if (!string.IsNullOrEmpty(this.reportRichTextBox.Text))
            {
                this.button.Text = "Close";
            }
            else
            {
                this.Close();
            }
        }

        private class Log : Allors.R1.Development.Repository.Log
        {
            private readonly GeneratingDialog generatingDialog;

            public Log(GeneratingDialog generatingDialog)
            {
                this.generatingDialog = generatingDialog;
            }

            public override void Error(object sender, string message)
            {
                this.generatingDialog.Invoke(new ReportDelegate(this.generatingDialog.Report), new object[] { message });
            }
        }
    }
}