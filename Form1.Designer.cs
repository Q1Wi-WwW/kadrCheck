namespace kadrCheck
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBoxFrame = new PictureBox();
            buttonOpen = new Button();
            trackBarFrames = new TrackBar();
            labelFrameNumber = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBoxFrame).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarFrames).BeginInit();
            SuspendLayout();
            // 
            // pictureBoxFrame
            // 
            pictureBoxFrame.Location = new Point(50, 41);
            pictureBoxFrame.Name = "pictureBoxFrame";
            pictureBoxFrame.Size = new Size(1277, 472);
            pictureBoxFrame.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxFrame.TabIndex = 0;
            pictureBoxFrame.TabStop = false;
            // 
            // buttonOpen
            // 
            buttonOpen.Location = new Point(12, 12);
            buttonOpen.Name = "buttonOpen";
            buttonOpen.Size = new Size(211, 23);
            buttonOpen.TabIndex = 1;
            buttonOpen.Text = "Открыть";
            buttonOpen.UseVisualStyleBackColor = true;
            buttonOpen.Click += buttonOpen_Click;
            // 
            // trackBarFrames
            // 
            trackBarFrames.Location = new Point(12, 552);
            trackBarFrames.Maximum = 0;
            trackBarFrames.Name = "trackBarFrames";
            trackBarFrames.Size = new Size(928, 45);
            trackBarFrames.TabIndex = 2;
            // 
            // labelFrameNumber
            // 
            labelFrameNumber.AutoSize = true;
            labelFrameNumber.Location = new Point(1053, 564);
            labelFrameNumber.Name = "labelFrameNumber";
            labelFrameNumber.Size = new Size(38, 15);
            labelFrameNumber.TabIndex = 3;
            labelFrameNumber.Text = "label1";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1355, 609);
            Controls.Add(labelFrameNumber);
            Controls.Add(trackBarFrames);
            Controls.Add(buttonOpen);
            Controls.Add(pictureBoxFrame);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBoxFrame).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBarFrames).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBoxFrame;
        private Button buttonOpen;
        private TrackBar trackBarFrames;
        private Label labelFrameNumber;
    }
}
