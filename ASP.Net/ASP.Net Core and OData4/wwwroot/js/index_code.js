$(function () {
	$("#grid").dxDataGrid({
		height: 900,
		remoteOperations: { paging: true, filtering: true, sorting: true },
		dataSource: new DevExpress.data.ODataStore({
			url: "Employees?$expand=Department",
			version: 4,
			key: ["Oid"],
			keyType: {
				Oid: "Guid"
			},
			onLoaded: onLoaded
		}),
		columnAutoWidth: true,
		cacheEnabled: false,
		filterRow: { visible: true },
		groupPanel: { visible: true },
		grouping: { autoExpandAll: false },
		onCellPrepared: onCellPrepared,
		pager: {
			showInfo: true
		},
		columns: [
			{
				caption: "Employee",
				columns: [
					{
						caption: "First name",
						dataField: "FirstName",
					},
					{
						caption: "Last name",
						dataField: "LastName",
					}
				]
			},
			{
				caption: "Email",
				dataField: "Email",
			},
			{
				caption: "Department",
                dataField: "Department.Oid",
                allowFiltering: false,
				lookup: {
					dataSource: new DevExpress.data.ODataStore({
						url: "Departments",
						version: 4,
						key: ["Oid"],
						keyType: {
							Oid: "Guid"
						}
					}),
					displayExpr: "Title",
					valueExpr: "Oid"
				}
			}
		]
	});

	$("#Logoff").dxButton({
		text: "Logoff",
		type: "normal",
		onClick: function () {
			$.ajax({
				method: 'GET',
				url: 'Logoff',
				complete: function () {
					window.location = "Authentication.html";
				}
			});
		}
	});

	function onCellPrepared(e) {
		if (e.rowType === "data") {
			var key = e.key.Oid._value;
			var permission = getPermission(key);
			if (e.column.command != 'edit') {	
				var dataField = e.column.dataField.split('.')[0];
				if (!permission[dataField]) {
					e.cellElement.text("Protected Content");
				}
			}
		}
	}

	function onLoaded(data) {
		var oids = [];
		$.each(data, function (i, element) {
			oids[i] = element.Oid._value;
		});
		var data = {
			keys: oids,
			typeName: 'Employee'
		};
		var options = {
			dataType: "json",
			contentType: "application/json",
			type: "POST",
			async: false,
			data: JSON.stringify(data)
		};
		$.ajax("GetPermissions", options)
			.done(function (e) {
				permissions = e.value;
			});
	}

	function getPermission(key) {
		var permission = $.map(permissions, function (val) {
			if (val.Key === key) {
				return val;
			}
		});
		return permission[0];
	}
});