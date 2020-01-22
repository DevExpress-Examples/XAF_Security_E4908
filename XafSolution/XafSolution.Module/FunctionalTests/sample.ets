#DropDB XafSolutionEasyTest

#Application XafSolution

*FillForm
 User Name = User

*Action Log In

*Action Navigation(Default.Department)

*CheckTable
 Columns = Title
 RowCount = 2
 Row[0] = Development Department
 Row[1] = Research and Development
 
*Action Navigation(Default.Employee)

*CheckTable
 Columns = Full Name, Department
 Row = Aaron Borrmann, Protected Content
 
*ProcessRecord
 Full Name = Aaron Borrmann

!ActionAvailable Delete

!FillForm
 Last Name = Johnson
 
*Action Close

*CheckTable
 Columns = Full Name, Department
 Row = Albert Walker, Research and Development
 
*ProcessRecord
 Full Name = Albert Walker

*ActionAvailable Delete

*FillForm
 Last Name = Skywalker
 
*Action Close