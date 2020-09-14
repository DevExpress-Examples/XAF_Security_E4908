$(function () {
	$("#grid").dxDataGrid({
		height: 900,
		remoteOperations: { paging: true, filtering: true, sorting: true },
		dataSource: new DevExpress.data.DataSource({
			store: new DevExpress.data.ODataStore({
				url: "Employees",
				version: 4,
				key: "Oid",
				keyType: "Guid",
				onLoaded: onLoaded
			}),
			expand: ["Department"]
		}),
		columnAutoWidth: true,
		cacheEnabled: false,
		filterRow: { visible: true },
		groupPanel: { visible: true },
		grouping: { autoExpandAll: false },
		onInitialized: onInitialized,
		onCellPrepared: onCellPrepared,
		onEditorPreparing: onEditorPreparing,
		pager: {
			showInfo: true
		},
		editing: {
			mode: "form",
			allowUpdating: true,
			allowAdding: true,
			allowDeleting: true
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
						keyType: "Guid"
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

	$("#Logout").dxButton({
		text: "Log Out",
		type: "normal",
		onClick: function () {
			$.ajax({
				method: 'GET',
				url: 'Logout',
				complete: function () {
					window.location = "Authentication.html";
				}
			});
		}
	});

	function onInitialized(e) {
		$.ajax({
			method: 'GET',
			url: 'GetTypePermissions?typeName=Employee',
			async: false,
			complete: function (data) {
				typePermissions = data.responseJSON;
			}
		});
		var grid = e.component;
		grid.option("editing.allowAdding", typePermissions.Create);
	}

	function onCellPrepared(e) {
		if (e.rowType === "data") {
			var key = e.key._value;
			var objectPermission = getPermission(key);
			if (!e.column.command) {
				var dataField = e.column.dataField.split('.')[0];
				if (!objectPermission[dataField].Read) {
					e.cellElement.text("*******");
				}
			}
			else if (e.column.command == 'edit') {
				if (!objectPermission.Delete) {
					e.cellElement.find(".dx-link-delete").remove();
				}
				if (!objectPermission.Write) {
					e.cellElement.find(".dx-link-edit").remove();
				}
			}
		}
	}

	function onEditorPreparing(e) {
		if (e.parentType === "dataRow") {
			var dataField = e.dataField.split('.')[0];
			var key = e.row.key._value;
			if (key != undefined) {
				var objectPermission = getPermission(key);
				if (!objectPermission[dataField].Read) {
					e.editorOptions.disabled = true;
                    e.editorOptions.value = "*******";
				}
				if (!objectPermission[dataField].Write) {
					e.editorOptions.disabled = true;
				}
			}
			else {
				if (!typePermissions[dataField]) {
					e.editorOptions.disabled = true;
				}
			}
		}
	}

	function onLoaded(data) {
        var oids = $.map(data, function (val) {
			return val.Oid._value;
		});
		var parameters = {
			keys: oids,
			typeName: 'Employee'
		};
		var options = {
			dataType: "json",
			contentType: "application/json",
			type: "POST",
			async: false,
            data: JSON.stringify(parameters)
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