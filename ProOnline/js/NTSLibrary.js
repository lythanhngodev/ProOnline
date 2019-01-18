//function chuyenBuoc(step) {
 
//    var dem = $('.step-content').find('.active').attr('data-step');
//    console.log('chọn ' + step + '; ' + 'hiện tại' + dem);
//    var wizard = $('#fuelux-wizard-container').data('fu.wizard');
//    if (parseInt(dem) == parseInt(step) - 1) {
//        wizard.currentStep = step;
//        wizard.setState();
//        $("#btnTiep").trigger("click"); 
//    }
//    else if (parseInt(step) <= parseInt(dem)) {
//        wizard.currentStep = step;
//        wizard.setState();
//    }
//    else {
//        alert("Bạn phải nhập hoàn thành các thông tin ở bước " + (parseInt(dem) + 1));
//        return false;
//    }
//}
//jQuery(document).ready(function () {
//    $('ul.steps li').click(function (event) {
//        chuyenBuoc($(this).attr('data-step'));
//    });
//})
String.prototype.replaceAll = function (strTarget, strSubString) {
    var strText = this;
    if (strText.length > 0) {
        var intIndexOfMatch = strText.indexOf(strTarget);
        while (intIndexOfMatch != -1) {
            strText = strText.replace(strTarget, strSubString)
            intIndexOfMatch = strText.indexOf(strTarget);
        }
        return (strText);
    }
    else {
        return "";
    }
}
function HienThiControl(item, visibe) { 
    var container = document.getElementById(item);
    if (typeof (container) != 'undefined') {
        if (visibe) {
            container.style.visibility = "visible";
            container.style.display = "Block";
        }
        else {
            container.style.visibility = "hidden";
            container.style.display = "none";
        }
    }
}
function checkDate(value,flag)
{
    var re = /^\d{1,2}\/\d{1,2}\/\d{4}$/;
    //Cho phép rỗng
    if (flag == true)
    {
        if (!value.match(re)) {
            return false;
        }
        else
            return true;
    }
    else if (value != '' && !value.match(re)) { 
        return false;
    }
}
//Kiểm tra value null hoặc bằng 0
function isEmtyValue(value) {
    if (value == "0" || value == "")
        return true;
    return false;
}
function bindData(comboName, dataSource) { 
    comboName.options.clear();
    if (dataSource != null) {
        $.each(JSON.parse(dataSource.toJSON()).Rows, function (index, item) {
            comboName.options.add(item[1] + "", item[0] + "", comboName.options.length);
        }) 
    }
}
function checkEmtyValue(value)
{
    if (value == "0" || value == "")
        return true;
    return false;
}
//Kiểm tra value null
function isEmty(value) {
    if (value == "")
        return true;
    return false;
}
//Định dạng window
function dinhDangWindow(winDowName) {
    var screenWidth = screen.width;
    var screenHeight = screen.height;
    var WindowSize = winDowName.getSize();
    winDowName.setPosition(parseFloat((parseFloat(screenWidth) - parseFloat(WindowSize.width)) / 2), window.scrollY + 100);
    winDowName.Open();
}
//Định dạng số
function dinhDangSo(sender, value) {
    sender.value(formatNumber(value));
}
//Định dạng tiền tệ
function formatNumber(str) { 
    var soAm = (str + "").substring(0, 1);
    if (soAm == "-")
        str = str.replace("-", "");
        if (jQuery.type(str) == "undefined" || str == null)
            return '0';
        str = str.toString();
        var m = str.lastIndexOf(",");
        var phanNguyen = "";
        var phanTapPhan = "";
        if (m == -1)
            phanNguyen = str;
        else {
            phanNguyen = str.substring(0, str.lastIndexOf(","));
            phanTapPhan = str.substring(str.lastIndexOf(","), str.length);
        }
        var kq = "";
        var dem = 0;
        phanNguyen = parseFloat(phanNguyen.split(".").join('').split(",").join('')).toString();
        for (var i = phanNguyen.length; i > 0; i--) {

            if (!isNaN(phanNguyen.substring(i, i - 1))) {
                kq = phanNguyen.substring(i, i - 1).toString() + kq;
                if (dem == 2 && i != 1) {
                    kq = "." + kq;
                    dem = 0;
                } else {
                    dem = dem + 1;
                }
            }
        }
        if (phanTapPhan != '' && phanTapPhan != ',' && parseFloat(phanTapPhan.split(",").join('').split(".").join('')).toString() != "0") {
            phanTapPhan = parseFloat(phanTapPhan.split(",").join('').split(".").join('')).toString();
            phanTapPhan = ',' + phanTapPhan;
        }
        else {
            phanTapPhan = '';
        }
        if (kq + phanTapPhan == '')
            return 0;

        kq = kq + phanTapPhan;
        if (soAm == "-")
            kq = "-" + kq + "";
        return kq;
    
}
 
function performSearch(grid, index, value) {
    for (var i = index; i < grid.ColumnsCollection.length - 1; i++) {
        var s = grid.ColumnsCollection[i].DataField;
        if (grid.ColumnsCollection[i].Visible == true) {
            grid.addFilterCriteria(s, OboutGridFilterCriteria.Contains, value);
        }
    }
    grid.executeFilter();
    searchTimeout = null;
    return false;
}
var searchTimeout = null;
function searchValue(grid, index, value) {
    debugger;
    if (searchTimeout != null) {
        return false;
    } 
    if (jQuery.type(value) == "undefined")
        value = '';
    for (var i = index; i < grid.ColumnsCollection.length; i++) {
        if (grid.ColumnsCollection[i].HeaderText != "") {
            var s = grid.ColumnsCollection[i].DataField;
            if (grid.ColumnsCollection[i].Visible == true && s != "") {
                grid.addFilterCriteria(s, OboutGridFilterCriteria.Contains, value);
            }
        }
    }
    //if (jQuery.type(grid.executeFilter()) == "undefined") {
    //    alert("Looix");
    //    return false;
    //}
    searchTimeout = window.setTimeout(grid.executeFilter(), 2000);
    searchTimeout = null;
    return false;
}

function bieuDoTron(result,tenBieuDo,DivID)
{  
    var dataTableGoogle = new google.visualization.DataTable(); 
    for (var i = 0; i < result.Columns.length; i++) {
        if (i == 0)
            dataTableGoogle.addColumn('string', result.Columns[i].Name);
        else
            dataTableGoogle.addColumn('number', result.Columns[i].Name);
    }
    dataTableGoogle.addRows(
                JSON.parse(result.toJSON()).Rows
            );
    // Instantiate and draw our chart, passing in some options
    var chart = new google.visualization.PieChart(document.getElementById(DivID));
    chart.draw(dataTableGoogle,
        {
            title: tenBieuDo,
            position: "top",
            fontsize: "14px",
            chartArea: { width: '100%', height: '100%' },
        });
}
function bieuDoTron3D(result, tenBieuDo, DivID) { 
    var dataTableGoogle = new google.visualization.DataTable();
    for (var i = 0; i < result.Columns.length; i++) {
        if (i == 0)
            dataTableGoogle.addColumn('string', result.Columns[i].Name);
        else
            dataTableGoogle.addColumn('number', result.Columns[i].Name);
    }
    dataTableGoogle.addRows(
                JSON.parse(result.toJSON()).Rows
            );
    // Instantiate and draw our chart, passing in some options
    //var chart = new google.visualization.PieChart(document.getElementById(DivID));
    var options = {
        title: tenBieuDo,
                position: "top",
                fontsize: "12px",
                top:'20px',
                chartArea: { width: '100%', height: '80%' },
                is3D:true
    };

    var chart = new google.visualization.PieChart(document.getElementById(DivID));

    chart.draw(dataTableGoogle, options);

    //chart.draw(dataTableGoogle,
    //    {
    //        title: tenBieuDo,
    //        position: "top",
    //        fontsize: "14px",
    //        top:'20px',
    //        chartArea: { width: '300px', height: '100%' },
    //        is3D:true
    //    });
}
function veBieuDoCot(result, tenBieuDo, DivID) {
     
    var dataTableGoogle = new google.visualization.DataTable();
    for (var i = 0; i < result.Columns.length; i++) {
        if (i == 0)
            dataTableGoogle.addColumn('string', result.Columns[i].Name);
        else
            dataTableGoogle.addColumn('number', result.Columns[i].Name);
    }
    dataTableGoogle.addRows(
                JSON.parse(result.toJSON()).Rows
            );
    // Instantiate and draw our chart, passing in some options
    var chart = new google.visualization.ColumnChart(document.getElementById(DivID));
    chart.draw(dataTableGoogle,
        {
            title: tenBieuDo,
            position: "top",
            fontsize: "14px", 
        });
}
function veBieuDoPhatTrien(result, tenBieuDo, DivID) { 
    if (result == null) {
        $('#' + DivID).html("");

    }
    else {
        var dataTableGoogle = new google.visualization.DataTable();
        for (var i = 0; i < result.Columns.length; i++) {
            if (i == 0)
                dataTableGoogle.addColumn('string', result.Columns[i].Name);
            else
                dataTableGoogle.addColumn('number', result.Columns[i].Name);
        }
        dataTableGoogle.addRows(
                    JSON.parse(result.toJSON()).Rows
                );
        var options = {
            title: tenBieuDo, 
        };
        var chart = new google.visualization.LineChart(document.getElementById(DivID));
        chart.draw(dataTableGoogle, options);
    }
    
}

// control boostrap
//Ngày bỗ sung: 18/12/2018
//Người bỗ sung: Vịnh
// Nội dung: các hàm sử dụng cho theme boostrap

function Loadding(class_) {
    $('' + class_ + '').append('<div class="message-loading-overlay"><i class="fa-spin ace-icon fa fa-spinner orange2 bigger-260"></i></div>');
}
function Loadding_Finish(class_) {
    $('' + class_ + '').find('.message-loading-overlay').remove();
}

function An_HienNut(idBotton, thaoTac) {
    $('' + idBotton + '').css("visibility", '' + thaoTac + '');
    $('' + idBotton + '').css("display", '' + thaoTac + '');
}
function HienThongBao(tieudeTB, noidungTB, loaiTB) {
    switch (loaiTB) {
        case "Loi":
            //lỗi
            $.gritter.add({
                title: tieudeTB,
                text: noidungTB,
                class_name: 'gritter-error' + (!$('#gritter-light').get(0).checked ? ' gritter-light' : '')
            });
            break;
        case "ThanhCong":
            //thành công
            $.gritter.add({
                title: tieudeTB,
                text: noidungTB,
                class_name: 'gritter-success' + (!$('#gritter-light').get(0).checked ? ' gritter-light' : '')
            });
            break;
        case "CanhBao":
            //cảnh báo
            $.gritter.add({
                title: tieudeTB,
                text: noidungTB,
                class_name: 'gritter-warning' + (!$('#gritter-light').get(0).checked ? ' gritter-light' : '')
            });
            break;
        case "ChiTiet":
            // hiện chi tiêt cảnh báo
            $.gritter.add({
                title: tieudeTB,
                text: noidungTB,
                class_name: 'gritter-info gritter-center' + (!$('#gritter-light').get(0).checked ? ' gritter-light' : '')
            });
            break;

    }
}
//Loại bỏ các khoảng trắng ở đầu, cuối và dư thừa của chuỗi.
function trimSpace(str) {
    if (str == "")
        return str;
    else
        return str.replace(/(?:(?:^|\n)\s+|\s+(?:$|\n))/g, "").replace(/\s+/g, " ");
}
//kiem tra email
function KiemTraEmail(sEmail) {
    var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    if (filter.test(sEmail)) {
        return true;
    }
    else {
        return false;
    }
}
//kiem tra sdt
function KiemTraSDT(txtPhone) {
    var filter = /^[0-9]+$/;
    if (filter.test(txtPhone)) {
        return true;
    }
    else {
        return false;
    }
}
//kiem tra ngay thang năm 
function KiemTraNgay(dtValue) {
    var dtRegex = new RegExp(/\b\d{1,2}[\/-]\d{1,2}[\/-]\d{4}\b/);
    return dtRegex.test(dtValue);
}
//cho input chỉ nhập số
function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}
// load combobox
function loadComBobox(idCombo, textChon, tenBang, cotTruyVan, dieuKien) {
    //alert(cotTruyVan.split(",")[0]);
    $('#' + idCombo + '').empty().append("<option value=''>" + textChon + "</option>");
    // preloader.on();
    $.ajax({
        type: "POST",
        url: "/Services/WebServiceSystem.asmx/loadCombobox",
        data: "{\"tenBang\":\"" + tenBang + "\", \"cotTruyVan\":\"" + cotTruyVan + "\", \"dieuKien\":\"" + dieuKien + "\" }",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            var array = JSON.parse(response.d);
            //$('#' + idCombo + '').append("<option value='' >"+textChon+"</option>");
            if (array != '') {
                for (i in array) {
                    if (cotTruyVan.split(",").length > 2) {
                        $('#' + idCombo + '').append("<option value='" + array[i]["" + cotTruyVan.split(",")[0] + ""] + "' >" + array[i]["" + cotTruyVan.split(",")[1].split("=")[0] + ""] + "</option>");
                    }
                    else {
                        $('#' + idCombo + '').append("<option value='" + array[i]["" + cotTruyVan.split(",")[0] + ""] + "' >" + array[i]["" + cotTruyVan.split(",")[1] + ""] + "</option>");
                    }
                }
            }
        },
        error: function (x, e) {
        }
    });
}
// khóa phím enter
$(document).keypress(function (e) {
    if (e.which == 13) {
        return false;
    }
});
