
$(function () {
	$("#userName").dxTextBox({
		name: "userName",
		placeholder: "User name",
		value: "User"
	}).dxValidator({
		validationRules: [
			{ type: "required" }
		]
	});

	$("#password").dxTextBox({
		name: "Password",
		placeholder: "Password",
		mode: "password"
	});

	$("#validateAndSubmit").dxButton({
		text: "Login",
		type: "success",
		useSubmitBehavior: true
	});

	$("#grid").dxDataGrid({
		height: 800,
		remoteOperations: { /*paging: true, filtering: true, sorting: true, grouping: true, summary: true, groupPaging: true*/ },
		dataSource: new DevExpress.data.ODataStore({
			url: "Employees",
			version: 4
		}),
		OnCellPrepared: cell_prepared(),
		columnAutoWidth: true,
		filterRow: { visible: true },
		groupPanel: { visible: true },
		grouping: { autoExpandAll: false },
		pager: {
			showInfo: true
		},
		columns: [
			{
				caption: "Employee",
				calculateDisplayValue: "FullName",
				dataField: "FullName"
			},
			{
				caption: "Department",
				calculateDisplayValue: "Department.Title",
				dataField: "Department",
				customizeText: function (cellInfo) {
					var result = cellInfo.value;
					if (result == null) {
						result = "Protected content"
					}
					return result;
				}
			}
		]
	});

	function cell_prepared(options) {
		//var fieldData = options.value,
		//	fieldHtml = "";
		//var x = options.value;
	}

	$("#testgrid").dxDataGrid({
		height: 800,
		remoteOperations: { /*paging: true, filtering: true, sorting: true, grouping: true, summary: true, groupPaging: true*/ },
		dataSource: new DevExpress.data.ODataStore({
			url: "Test",
			version: 4
		}),
		columnAutoWidth: true,
		filterRow: { visible: true },
		groupPanel: { visible: true },
		grouping: { autoExpandAll: false },
		pager: {
			showInfo: true
		},
		columns: [
			{
				caption: "Employee",
				calculateDisplayValue: "Employee.FullName",
				dataField: "FullName"
			},
			{
				caption: "Department",
				calculateDisplayValue: "Employee.Department.Title",
				dataField: "Employee.Department.Department",
				customizeText: function (cellInfo) {
					if (cellInfo.value == null) {
						return "Protected content"
					}
					return cellInfo.value;
				}
			}
		]
	});

});