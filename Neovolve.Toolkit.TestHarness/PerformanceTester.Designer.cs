namespace Neovolve.Toolkit.TestHarness
{
    partial class PerformanceTester
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Start = new System.Windows.Forms.Button();
            this.Actions = new System.Windows.Forms.ListBox();
            this.Iterations = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.RecursionDepth = new System.Windows.Forms.NumericUpDown();
            this.CollectGC = new System.Windows.Forms.Button();
            this.Progress = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.Iterations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RecursionDepth)).BeginInit();
            this.SuspendLayout();
            // 
            // Start
            // 
            this.Start.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Start.Location = new System.Drawing.Point(524, 362);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(75, 23);
            this.Start.TabIndex = 0;
            this.Start.Text = "Start";
            this.Start.UseVisualStyleBackColor = true;
            this.Start.Click += new System.EventHandler(this.Start_Click);
            // 
            // Actions
            // 
            this.Actions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Actions.FormattingEnabled = true;
            this.Actions.IntegralHeight = false;
            this.Actions.Location = new System.Drawing.Point(12, 12);
            this.Actions.Name = "Actions";
            this.Actions.Size = new System.Drawing.Size(302, 373);
            this.Actions.TabIndex = 1;
            this.Actions.SelectedIndexChanged += new System.EventHandler(this.Actions_SelectedIndexChanged);
            // 
            // Iterations
            // 
            this.Iterations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Iterations.Location = new System.Drawing.Point(479, 10);
            this.Iterations.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.Iterations.Name = "Iterations";
            this.Iterations.Size = new System.Drawing.Size(120, 20);
            this.Iterations.TabIndex = 2;
            this.Iterations.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(320, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Iterations:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(320, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Recursion Depth:";
            // 
            // RecursionDepth
            // 
            this.RecursionDepth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RecursionDepth.Location = new System.Drawing.Point(479, 48);
            this.RecursionDepth.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.RecursionDepth.Name = "RecursionDepth";
            this.RecursionDepth.Size = new System.Drawing.Size(120, 20);
            this.RecursionDepth.TabIndex = 5;
            this.RecursionDepth.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // CollectGC
            // 
            this.CollectGC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CollectGC.Location = new System.Drawing.Point(443, 362);
            this.CollectGC.Name = "CollectGC";
            this.CollectGC.Size = new System.Drawing.Size(75, 23);
            this.CollectGC.TabIndex = 6;
            this.CollectGC.Text = "Collect GC";
            this.CollectGC.UseVisualStyleBackColor = true;
            this.CollectGC.Click += new System.EventHandler(CollectGC_Click);
            // 
            // Progress
            // 
            this.Progress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Progress.Location = new System.Drawing.Point(323, 333);
            this.Progress.Name = "Progress";
            this.Progress.Size = new System.Drawing.Size(276, 23);
            this.Progress.TabIndex = 7;
            // 
            // PerformanceTester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(611, 397);
            this.Controls.Add(this.Progress);
            this.Controls.Add(this.CollectGC);
            this.Controls.Add(this.RecursionDepth);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Iterations);
            this.Controls.Add(this.Actions);
            this.Controls.Add(this.Start);
            this.Name = "PerformanceTester";
            this.Text = "Performance Tester";
            this.Load += new System.EventHandler(this.PerformanceTester_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Iterations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RecursionDepth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Start;
        private System.Windows.Forms.ListBox Actions;
        private System.Windows.Forms.NumericUpDown Iterations;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown RecursionDepth;
        private System.Windows.Forms.Button CollectGC;
        private System.Windows.Forms.ProgressBar Progress;
    }
}

