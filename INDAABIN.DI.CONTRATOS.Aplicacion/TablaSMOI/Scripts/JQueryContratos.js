

function validateRadio() {
    var flag = false;
    $('#<%=RadioButtonList1.ClientID%> input').each(function(){
        if($(this).is(":checked"))
            flag = true;
    });
    return flag;
}
 
function validateDropList() {
    if ($('#<%=DropDownList1.ClientID%>').val() == "") {
        return false;
    }
    else
        return true;
}


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