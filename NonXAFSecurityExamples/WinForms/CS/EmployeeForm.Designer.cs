namespace NonXAFSecurityWindowsFormsApp {
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
			((System.ComponentModel.ISupportInitialize)(this.employeeGrid)).BeginInit();
			this.SuspendLayout();
			// 
			// employeeGrid
			// 
			this.employeeGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.employeeGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Employee,
            this.Department});
			this.employeeGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.employeeGrid.Location = new System.Drawing.Point(0, 0);
			this.employeeGrid.Name = "employeeGrid";
			this.employeeGrid.RowHeadersVisible = false;
			this.employeeGrid.Size = new System.Drawing.Size(598, 417);
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
			// EmployeeForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(598, 417);
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
	}
}