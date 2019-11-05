function pressEnter(data) {
	$('#validateAndSubmit').click();
}

function getCookie(name) {
	let matches = document.cookie.match(new RegExp(
		"(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
	));
	return matches ? decodeURIComponent(matches[1]) : undefined;
}

function login(e) {
	var userName = $("#userName").dxTextBox("instance").option("value");
	var password = $("#password").dxTextBox("instance").option("value");
	$.ajax({
		method: 'POST',
		url: 'Login',
		data: {
			"userName": userName,
			"password": password
		},
		complete: function (e) {
			if (e.status === 200) {
				document.cookie = "userName=" + userName;
				document.location.href = "/";
			}
			if (e.status === 401) {
				alert("User name or password is incorrect");
			}
		}
	});

	e.preventDefault();
}
