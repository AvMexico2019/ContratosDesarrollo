
function validateRadio() {
    var flag = false;
    var list = document.getElementById("<%=RadioButtonList1.ClientID%>"); //Client ID of the radiolist
    var inputs = list.getElementsByTagName("input");
    for (var i = 0; i < inputs.length; i++) {
        if (inputs[i].checked) {
            flag = true;
            break;
        }
    }
    return flag;
}

function validateDropList() {

    if (document.getElementById("<%=DropDownList1.ClientID%>").value == "") {
        return false;
    }
    else
        return true;
}

//test de funciones para validar: radiobutton y DropDownlist
function submitForm() {
    if (!validateRadio()) {
        alert("Please do mark option.");
        return false; //do not submit form
    }
    else if (!validateDropList()) {
        alert("Please do select a country.");
        return false; //do not submit form
    }
    else
        return true;
}