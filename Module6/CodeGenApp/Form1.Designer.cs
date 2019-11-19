using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CodeGenApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private Button getButton;

        private TextBox resultTextbox;

        private Label resultLabel;

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
            getButton = new Button();
            resultTextbox = new TextBox();
            resultLabel = new Label();
            SuspendLayout();
            getButton.Location = new Point(188, 25);
            getButton.Name = "bt_check";
            getButton.Size = new Size(55, 20);
            getButton.TabIndex = 0;
            getButton.Text = "Get key";
            getButton.UseVisualStyleBackColor = true;
            getButton.Click += ev_a;
            resultTextbox.Location = new Point(35, 25);
            resultTextbox.Name = "tb_key";
            resultTextbox.Size = new Size(150, 20);
            resultTextbox.TabIndex = 1;
            resultLabel.AutoSize = true;
            resultLabel.Location = new Point(32, 9);
            resultLabel.Name = "label1";
            resultLabel.Size = new Size(107, 13);
            resultLabel.TabIndex = 2;
            resultLabel.Text = "Please, press button to get key";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(280, 80);
            base.Controls.Add(resultLabel);
            base.Controls.Add(resultTextbox);
            base.Controls.Add(getButton);
            base.Name = "Form1";
            Text = "Code generator";
            ResumeLayout(performLayout: false);
            PerformLayout();
            this.components = new System.ComponentModel.Container();
        }

        #endregion
    }
}

