﻿namespace DataCollect
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Qua_Stop = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.QuaTime = new System.Windows.Forms.DateTimePicker();
            this.Qua_show = new System.Windows.Forms.Button();
            this.Qua_Start = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.Qua_Stop);
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Controls.Add(this.QuaTime);
            this.groupBox1.Controls.Add(this.Qua_show);
            this.groupBox1.Controls.Add(this.Qua_Start);
            this.groupBox1.Location = new System.Drawing.Point(5, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(528, 322);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "质量报告";
            // 
            // Qua_Stop
            // 
            this.Qua_Stop.Location = new System.Drawing.Point(124, 18);
            this.Qua_Stop.Name = "Qua_Stop";
            this.Qua_Stop.Size = new System.Drawing.Size(75, 23);
            this.Qua_Stop.TabIndex = 5;
            this.Qua_Stop.Text = "停止";
            this.Qua_Stop.UseVisualStyleBackColor = true;
            this.Qua_Stop.Click += new System.EventHandler(this.Qua_Stop_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(0, 49);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(528, 267);
            this.dataGridView1.TabIndex = 4;
            // 
            // QuaTime
            // 
            this.QuaTime.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.QuaTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.QuaTime.Location = new System.Drawing.Point(244, 20);
            this.QuaTime.Name = "QuaTime";
            this.QuaTime.Size = new System.Drawing.Size(160, 21);
            this.QuaTime.TabIndex = 3;
            // 
            // Qua_show
            // 
            this.Qua_show.Location = new System.Drawing.Point(447, 18);
            this.Qua_show.Name = "Qua_show";
            this.Qua_show.Size = new System.Drawing.Size(75, 23);
            this.Qua_show.TabIndex = 2;
            this.Qua_show.Text = "浏览";
            this.Qua_show.UseVisualStyleBackColor = true;
            this.Qua_show.Click += new System.EventHandler(this.Qua_show_Click);
            // 
            // Qua_Start
            // 
            this.Qua_Start.Location = new System.Drawing.Point(26, 18);
            this.Qua_Start.Name = "Qua_Start";
            this.Qua_Start.Size = new System.Drawing.Size(75, 23);
            this.Qua_Start.TabIndex = 0;
            this.Qua_Start.Text = "开始";
            this.Qua_Start.UseVisualStyleBackColor = true;
            this.Qua_Start.Click += new System.EventHandler(this.Qua_Start_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 333);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "数采服务程序";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }
     
        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DateTimePicker QuaTime;
        private System.Windows.Forms.Button Qua_show;
        private System.Windows.Forms.Button Qua_Start;
        private System.Windows.Forms.Button Qua_Stop;
  
    }
}

