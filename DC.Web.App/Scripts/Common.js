function ShowPopup() {
    bootbox.hideAll();
    bootbox.dialog({
        title: "",
        message: ""
            + "<div class='row'><div class='col-lg-12 jsLoader text-center' style='height:100px;'>"
            + "<div class='loader  '>"
            + "<div class='dot'></div>"
            + "<div class='dot'></div>"
            + "<div class='dot'></div>"
            + "<div class='dot'></div>"
            + "<div class='dot'></div>"
            + "<br />"
            + "<h6>"
            + "We are getting things ready.Please wait<br />"
            + "Thanks for your patience."
            + "</h6>"
            + "</div><div>",
        closeButton: false
    });
}
function HidePopup() {
    bootbox.hideAll();

}
function JsonDateDDMMYYY(dtm) {

    var dateString = dtm.substr(6);
    var currentTime = new Date(parseInt(dateString));
    var month = currentTime.getMonth() + 1;
    var day = currentTime.getDate();
    var year = currentTime.getFullYear();
    var date = day + "/" + month + "/" + year;
    return date;
}
function CheckBoxStyle(x, _this) {
    $(".jsError").remove();
    if (_this.checked) {
        $(x).addClass("font-weight-bold");
        $(x).addClass("text-primary");

    } else {
        $(x).removeClass("font-weight-bold");
        $(x).removeClass("text-primary");
    }
}
function AddLoader() {

    if ($("#preloader").length <= 0) {
        $("body").append('<div id="preloader" class="preloaderopacity"></div>');

    }
}
function RemoveLoader() {
    $("#preloader").remove();
}
//x is the labelid lblCalss

function CopyDropDown(src, dest, selval,deftext) {
    $('#' + dest + ' option').remove();
    var $options = $("#" + src + "").html();
    $('#' + dest + '').html('<option value="">' + deftext+'</option>'+$options);
    $('#' + dest + '').val(selval);
}
function CopyDropDown1(src, dest, selval, deftext) {
    $('#' + dest + ' option').remove();
    var $options = $("#" + src + "").html();
    $('#' + dest + '').html($options);
    $("#" + dest + " option:first").text(deftext);
    
    $('#' + dest + '').val(selval);
}
function SetImage(ctrl, src) {

    $(ctrl).attr("src", src);
}

function SetImageDefault(ctrl) {

    $(ctrl).attr("src", "/images/blank.png");
}
function SetSquareImageDefault(ctrl) {

    $(ctrl).attr("src", "/images/blansquare.jpeg");
}
 
 
function BlankUSer(ctrl) {

    $(ctrl).attr("src", "/images/blankuser.png");
}
function isNumber(evt) {

    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
     
    if (charCode == 13 || charCode == 37 || charCode == 39) {
        return true;
    }
    if (charCode == 46) {
        return false;
    }
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}
//////

//setInputFilter(document.getElementById("intTextBox"), function (value) {return /^-?\d*$/.test(value);});
//setInputFilter(document.getElementById("uintTextBox"), function (value) { return /^\d*$/.test(value);});
//setInputFilter(document.getElementById("intLimitTextBox"), function (value) {  return /^\d*$/.test(value) && (value === "" || parseInt(value) <= 500);});
//setInputFilter(document.getElementById("floatTextBox"), function (value) {return /^-?\d*[.,]?\d*$/.test(value);});
//setInputFilter(document.getElementById("currencyTextBox"), function (value) {return /^-?\d*[.,]?\d{0,2}$/.test(value);});
//setInputFilter(document.getElementById("hexTextBox"), function (value) { return /^[0-9a-f]*$/i.test(value);});
//function setInputFilter(textbox, inputFilter) {
//    ["input", "keydown", "keyup", "mousedown", "mouseup", "select", "contextmenu", "drop"].forEach(function (event) {
//        textbox.addEventListener(event, function () {
//            if (inputFilter(this.value)) {
//                this.oldValue = this.value;
//                this.oldSelectionStart = this.selectionStart;
//                this.oldSelectionEnd = this.selectionEnd;
//            } else if (this.hasOwnProperty("oldValue")) {
//                this.value = this.oldValue;
//                this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
//            }
//        });
//    });



function isDecimal(evt) {
    evt = (evt) ? evt : window.event;

    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode == 13 || charCode == 37 || charCode == 39 || charCode == 46) {
        return true;
    }
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}
function UrlSafeString(data) {    //var cleanString = dirtyString.replace(/[|&;$%@"<>()+,]/g, "");

    data = data.replace(new RegExp('%', 'g'), '');
    data = data.replace(new RegExp('"', 'g'), '');
    data = data.replace(new RegExp('<', 'g'), '');
    data = data.replace(new RegExp('>', 'g'), '');
    data = data.replace(new RegExp('#', 'g'), '');
    data = data.replace(new RegExp('|', 'g'), '');
    data = data.replace(new RegExp("'", 'g'), '');

    data = data.replace(new RegExp(",", 'g'), '');

    data = data.replace(new RegExp("/", 'g'), '');
    return data;
}
Array.prototype.move = function (old_index, new_index) {
    if (new_index >= this.length) {
        var k = new_index - this.length;
        while ((k--) + 1) {
            this.push(undefined);
        }
    }
    this.splice(new_index, 0, this.splice(old_index, 1)[0]);
};
Array.prototype.remove = function (index) {
    this.splice(index, 1);
};

$.fn.inputFilter1 = function (inputFilter) {
    // return this.on("input keydown keyup mousedown mouseup select contextmenu drop", function (evt) {
    return this.on("input keydown mousedown mouseup select contextmenu drop", function (evt) {
        // evt = (evt) ? evt : window.event;
        //var charCode = (evt.which) ? evt.which : evt.keyCode;
        //var pposition = $(this).attr("data-pposition");
        //var cposition = $(this).attr("data-cposition");
        //var nposition = $(this).attr("data-nposition");
        //var dtype = $(this).attr("data-type");
        //var ctrl = "";       
        //if (charCode == 27) { 
        //    switch (parseFloat(dtype)) {
        //        case 0:
        //            ctrl = "#txt_" + pposition + "";
        //        break;
        //        case 1:
        //            ctrl = "#txt_" + cposition + "";
        //            break;
        //        case 2:
        //            ctrl = "#txtDay_" + cposition + "";
        //            break;
        //        default:
        //            ctrl = "";
        //    }
        //    if ($.trim(ctrl) != "") {
        //        $("" + ctrl + "").focus();      
        //    }                
        //    return true;
        //}
        //if (charCode == 13) {   
        //    switch (parseFloat(dtype)) {
        //        case 0:
        //            ctrl = "#txtDay_" + cposition + "";
        //            break;
        //        case 1:
        //            ctrl = "#ddnAbbrevation_" + cposition + "";
        //            break;
        //        case 2:
        //            ctrl = "#txt_" + nposition + "";
        //            break;
        //        default:
        //            ctrl = "";
        //    }

        //    if (parseInt(nposition) == -1) {
        //        $("#SData_Title").focus();
        //    }
        //    else {
        //        if ($.trim(ctrl) != "") {
        //            $("" + ctrl + "").focus();  
        //        } 
        //    }
        //    // $("#SData_Title").focus();
        //    return false;
        //}
        if (inputFilter(this.value)) {
            this.oldValue = this.value;
            this.oldSelectionStart = this.selectionStart;
            this.oldSelectionEnd = this.selectionEnd;
        } else if (this.hasOwnProperty("oldValue")) {
            this.value = this.oldValue;
            this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
        }
        $(this).removeClass("")
        //if (this.value == "" || parseInt(this.value) <= 0) {
        //    this.value = 1;
        //}
    });
};


function AddToCart(obj) {
    $("#toaster").remove();
    AddLoader();
    var bookid = $(obj).attr("data-bookid");
    var html = $(obj).html();
    $(obj).html("<span class='fa fa-spin fa-spinner'></span>");
    $.ajax({
        type: 'POST',
        url: '/MyLibrary/Dashboard/AddToCart',
        data: { BookId: bookid },
        dataType: "json",
        success: function (data) {
            if (data.StatusCode == 200) {
                //$.toaster({ priority: 'success', title: "Added to Cart", message: data.Message });

                //alert(data.Message)
                //RemoveLoader();
                sessionStorage.setItem("addedToCart", 1)
                location.reload(true);
            }
            else {
                $.toaster({ priority: 'danger', title: '', message: data.Message });
                RemoveLoader();
            }
        },
        complete: function (data) {
            RemoveLoader();
        },
        error: function (data) {
            RemoveLoader();
        }

    });
}


//Integer(both positive and negative):
//$("#intTextBox").inputFilter(function(value) {return /^-?\d*$/.test(value); });
//Integer(positive only):
//$("#uintTextBox").inputFilter(function (value) { return /^\d*$/.test(value);});
//Integer(positive and <= 500):
//$("#intLimitTextBox").inputFilter(function (value) { return /^\d*$/.test(value) && (value === "" || parseInt(value) <= 500);});
//Floating point(use.or, as decimal separator):
//$("#floatTextBox").inputFilter(function (value) { return /^-?\d*[.,]?\d*$/.test(value);});
//Currency(at most two decimal places):
//$("#currencyTextBox").inputFilter(function (value) { return /^-?\d*[.,]?\d{0,2}$/.test(value);});
//Hexadecimal:
//$("#hexTextBox").inputFilter(function (value) {   return /^[0-9a-f]*$/i.test(value);});