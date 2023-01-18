function onInitialized(e) {
	typePermissions = e;
}

function onCellPrepared(e) {
	if (e.rowType === "data") {
		var key = e.key;
		var objectPermission = getPermission(key);
		if (!e.column.command && e.column.dataField != undefined) {
			var dataField = e.column.dataField.split('.')[0];
			if (!objectPermission.Data[dataField].Read) {
                e.cellElement.text("*******");
			}
		}
	}
}

function allowUpdating(e) {
	if (e.row.rowType === "data") {
		var objectPermission = getPermission(e.row.key);
		if (objectPermission.Write) {
			return true;
		}
	}
	return false;
}

function allowDeleting(e) {
	if (e.row.rowType === "data") {
		var objectPermission = getPermission(e.row.key);
		if (objectPermission.Delete) {
			return true;
		}
	}
	return false;
}

function onEditorPreparing(e) {
	if (e.parentType === "dataRow") {
		var dataField = e.dataField.split('.')[0];
		var key = e.row.key;
		if (e.row.isNewRow) {
			if (!typePermissions[dataField]) {
				e.editorOptions.disabled = true;
			}
		}
		else {
			var objectPermission = getPermission(key);
			if (!objectPermission.Data[dataField].Read) {
				e.editorOptions.disabled = true;
                e.editorOptions.value = "*******";
			}
			if (!objectPermission.Data[dataField].Write) {
				e.editorOptions.disabled = true;
			}
		}
	}
}

function onLoaded(data) {
	var ids = $.map(data, function (val) {
		return val.ID;
	});
	var parameters = {
		keys: ids,
		typeName: 'Employee'
	};
	var options = {
		dataType: "json",
		contentType: "application/x-www-form-urlencoded; charset=UTF-8",
		type: "POST",
		async: false,
		data: parameters
	};
	$.ajax("api/Actions/GetPermissions", options)
		.done(function (e) {
			permissions = e;
		});
}

function getPermission(key) {
	var permission = permissions.filter(function (entry) {
		return entry.Key == key;
	});
	return permission[0];
}

function onClick() {
	$.ajax({
		method: 'GET',
		url: 'Logout',
		complete: function () {
			window.location = "Authentication";
		}
	});
}