#DropDB XafSolutionEasyTest

#Application XafSolution

*FillForm
 User Name = Admin
*Action Log In

*Action Navigation(Default.Role)

*ProcessRecord 
 Name = Default
*CheckFieldValues
 Permission Policy = Deny all by default
*CheckTable Users
 Columns = User Name
 Row[0] = User
*Action Navigation Permissions
*CheckTable Navigation Permissions
 Columns = Navigation Item, Navigate
 Row = Employee, Allow
 Row = Department, Allow
 
*Action Type Permissions

*ProcessRecord Type Permissions
 Target Type = Employee
*CheckFieldValues
 Read = Allow
 Write = Allow
 Create = ''
 Delete = ''
*Action Member Permissions
*CheckTable Member Permissions
 Columns = Members, Read, Write, Criteria
 Row[0] = Last Name, '', Deny, 'Not Contains([Department.Title], 'Development')'
*Action Object Permissions
*CheckTable Object Permissions
 Columns = Criteria, Read, Write, Delete
 Row[0] = 'Contains([Department.Title], 'Development')', '', '', Allow
*Action Close

*ProcessRecord Type Permissions
 Target Type = Department
*CheckFieldValues
 Read = Deny
 Write = ''
 Create = ''
 Delete = ''
*Action Object Permissions
*CheckTable Object Permissions
 Columns = Criteria, Read, Write, Delete
 Row[0] = 'Contains([Title], 'Development')', Allow, '', ''
 
*Action Close