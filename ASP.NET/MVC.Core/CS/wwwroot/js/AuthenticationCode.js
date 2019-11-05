function pressEnter(data) {
	$('#validateAndSubmit').click();
}

function getCookie(name) {
	let matches = document.cookie.match(new RegExp(
		"(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
	));
	return matches ? decodeURIComponent(matches[1]) : undefined;
}

function showError(message) {
	$(function () {
		DevExpress.ui.notify(message, "error");
	});
}
