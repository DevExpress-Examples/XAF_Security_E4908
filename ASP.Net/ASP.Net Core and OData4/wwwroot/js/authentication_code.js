$(function () {
	$("#userName").dxTextBox({
		name: "userName",
		placeholder: "User name",
        onEnterKey: pressEnter
	}).dxValidator({
		validationRules: [
			{ type: "required" }
		]
    });

	$("#password").dxTextBox({
		name: "Password",
		placeholder: "Password",
        mode: "password",
        onEnterKey: pressEnter
	});

	$("#validateAndSubmit").dxButton({
		text: "Login",
		type: "success",
		onClick: function () {
			var userName = $("#userName").dxTextBox("instance").option("value");
			var password = $("#password").dxTextBox("instance").option("value");
			var url = 'Login(userName = \'' + userName + '\', password = \'' + password + '\')'; 
			$.ajax({
				method: 'GET',
				url: url,
				complete: function (e) {
					if (e.status == 200) {
						document.location.href = "/";
					}
					if (e.status == 401) {
						alert("User name or password is incorrect");
					}
				}
			});
		}
    });

    function pressEnter(data) {
        $('#validateAndSubmit').click();
    }
});