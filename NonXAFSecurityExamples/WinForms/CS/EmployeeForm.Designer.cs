namespace WindowsFormsApplication {
	partial class EmployeeForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.employeeGrid = new System.Windows.Forms.DataGridView();
			this.Employee = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Department = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.logoff_button = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.employeeGrid)).BeginInit();
			this.SuspendLayout();
			// 
			// employeeGrid
			// 
			this.employeeGrid.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
			this.employeeGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.employeeGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.employeeGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Employee,
            this.Department});
			this.employeeGrid.Location = new System.Drawing.Point(0, 0);
			this.employeeGrid.Name = "employeeGrid";
			this.employeeGrid.RowHeadersVisible = false;
			this.employeeGrid.Size = new System.Drawing.Size(507, 476);
			this.employeeGrid.TabIndex = 0;
			// 
			// Employee
			// 
			this.Employee.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.Employee.HeaderText = "Employee";
			this.Employee.Name = "Employee";
			this.Employee.ReadOnly = true;
			// 
			// Department
			// 
			this.Department.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.Department.HeaderText = "Department";
			this.Department.Name = "Department";
			this.Department.ReadOnly = true;
			// 
			// logoff_button
			// 
			this.logoff_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.logoff_button.Location = new System.Drawing.Point(175, 493);
			this.logoff_button.Name = "logoff_button";
			this.logoff_button.Size = new System.Drawing.Size(108, 37);
			this.logoff_button.TabIndex = 1;
			this.logoff_button.Text = "Logoff";
			this.logoff_button.UseVisualStyleBackColor = true;
			this.logoff_button.Click += new System.EventHandler(this.Logoff_button_Click);
			// 
			// EmployeeForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(507, 542);
			this.Controls.Add(this.logoff_button);
			this.Controls.Add(this.employeeGrid);
			this.Name = "EmployeeForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Employees";
			((System.ComponentModel.ISupportInitialize)(this.employeeGrid)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView employeeGrid;
		private System.Windows.Forms.DataGridViewTextBoxColumn Employee;
		private System.Windows.Forms.DataGridViewTextBoxColumn Department;
		private System.Windows.Forms.Button logoff_button;
	}
}