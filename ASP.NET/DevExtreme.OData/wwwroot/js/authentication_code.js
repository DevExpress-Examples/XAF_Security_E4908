$(function () {
	$("#userName").dxTextBox({
		name: "userName",
		placeholder: "User name",
		tabIndex: 2,
		onInitialized: function (e) {
			var texBoxInstance = e.component;
			var userName = getCookie("userName");
			if (userName === undefined) {
				userName = "User";
			}
			texBoxInstance.option("value", userName);
		},
		onEnterKey: pressEnter
	}).dxValidator({
		validationRules: [{
			type: "required",
			message: "The user name must not be empty"
		}]
	});

	$("#password").dxTextBox({
		name: "Password",
		placeholder: "Password",
		mode: "password",
		tabIndex: 3,
		onEnterKey: pressEnter
	});

	$("#validateAndSubmit").dxButton({
		text: "Log In",
		tabIndex: 1,
		useSubmitBehavior: true
	});

	$("#form").on("submit", function (e) {
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
	});

	function pressEnter(data) {
		$('#validateAndSubmit').click();
	}

	function getCookie(name) {
		let matches = document.cookie.match(new RegExp(
			"(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
		));
		return matches ? decodeURIComponent(matches[1]) : undefined;
	}
});