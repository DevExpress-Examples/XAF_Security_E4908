$(function () {
	$("#grid").dxDataGrid({
		height: 900,
		remoteOperations: { paging: true, filtering: true, sorting: true },
		dataSource: new DevExpress.data.DataSource({
			store: new DevExpress.data.ODataStore({
				url: "Employees",
				version: 4,
				key: "Oid",
				keyType: {
					Oid: "Guid"
				},
				onLoaded: onLoaded,
			}),
			expand: ["Department"]
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
				lookup: {
					dataSource: new DevExpress.data.ODataStore({
						url: "Departments",
						version: 4,
						key: "Oid",
						keyType: {
							Oid: "Guid"
						}
					}),
					displayExpr: "Title",
					valueExpr: "Oid"
				},
				calculateFilterExpression: function (filterValue) {
					var filterExpression = [
						[this.dataField.replace('.', '/'), "=", filterValue]
					];
					return filterExpression;
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
			var key = e.key._value;
			var objectPermission = getPermission(key);
			if (e.column.command != 'edit') {	
				var dataField = e.column.dataField.split('.')[0];
				if (!objectPermission[dataField]) {
					e.cellElement.text("Protected Content");
				}
			}
		}
	}

	function onLoaded(data) {
		var oids = $.map(data, function (val) {
			return val.Oid._value;
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
		var permission = permissions.filter(function (entry) {
			return entry.Key === key;
		});
		return permission[0];
	}
});