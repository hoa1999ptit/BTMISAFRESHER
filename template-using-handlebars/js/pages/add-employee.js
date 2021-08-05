$(".button.btn-save").on("click", function() {
    saveEmployee();
    $('.popup.add-employee').attr('formmode', 0);
})
$(".avatar-image").on("click", function() {
    $("#avatar-file").trigger("click")
})
$(function() {
    $("#avatar-file").change(function(event) {
        var filepath = URL.createObjectURL(event.target.files[0])
        $(".avatar-image").css('background-image', `url(${filepath})`)
    })
});

moneyInput($(".field-input.money"));
/**
 * Kiểm tra ô nhập tiền tệ
 */
function moneyInput(thismoney) {
    thismoney.on("input", function() {
        let myinput = thismoney.val();
        // $(this).css('caret-color', 'black');
        // if (!isNaN(parseFloat(myinput))) {
        //     $(this).css('caret-color', 'red');
        // }
        res = CommonFn.formatMoney(myinput);
        thismoney.val(res);
    });
}


//Validate dữ liệu tại các ô bắt buộc nhập

$('input[required]').blur(function() {
    let ip = $(this).val();
    if (ip == '') {
        //Đổi trạng thái lỗi input
        // $(this).css('border', '1px solid red');
        $(this).addClass('missing')
    } else {
        $(this).removeClass('missing');
    }
    console.log(ip)
})

function validateInputFormat() {
    let employeeCode = $('#EmployeeCode').val(),
        fullName = $('#FullName').val(),
        identityNumber = $('#IdentityNumber').val(),
        phoneNumber = $('#PhoneNumber').val(),
        email = $('#Email').val();
    let mess = '';
    if ((employeeCode.length * fullName.length * identityNumber.length * phoneNumber.length * email.length) == "") {
        mess += 'Vui lòng nhập các dữ liệu cần thiết \n';
    }
    if (CommonFn.validName(fullName) != 1) {
        mess += 'Tên nhân viên không hợp lệ \n';
    }
    // if (CommonFn.validIdentity(identityNumber) != 1) {
    //     mess += 'CMND/ CCCD không hợp lệ \n';
    // }
    // if (CommonFn.validPhoneNumber(phoneNumber) != 1) {
    //     mess += 'SDT không hợp lệ \n';
    // }
    // if (CommonFn.validEmail(email) != 1) {
    //     mess += 'Email không hợp lệ \n';
    // }
    if (mess != '') alert(mess);
}

/**
 * Lấy thông tin nhân viên từ các ô nhập liệu
 * Hiển thị ra table ( tạm thời)
 * Gửi dữ liệu đến API
 */

function saveEmployee() {
    myurl = '';
    mymethod = '';
    var formmode = $('.popup.add-employee').attr('formmode');
    if (formmode == 0) {
        myurl = "http://cukcuk.manhnv.net/v1/Employees/";
        mymethod = "POST";
        mess = "Đã thêm mới nhân viên ";
    } else {
        console.log("Formmode:", $('.popup.add-employee').attr("formmode"))
        employeeId = $('.popup.add-employee').attr('empid');
        myurl = "http://cukcuk.manhnv.net/v1/Employees/" + employeeId;
        mymethod = "PUT";
        mess = "Đã cập nhật thông tin nhân viên ";
    }
    let newEmployee = {
        // "createdDate": "2021-07-20T09:14:19.271Z",
        // "createdBy": ":<",
        // "modifiedDate": "2021-07-20T09:14:19.271Z",
        // "modifiedBy": ":<",
        // "employeeId": "aab58d84-e93f-11eb-94eb-42010a8c0002",
        // "employeeCode": "NV956",
        // "firstName": ":<",
        // "lastName": ":<",
        // "fullName": "NGuyen Van A",
        // "gender": 1,
        // "dateOfBirth": "2021-07-20T09:14:19.271Z",
        // "phoneNumber": "091345484",
        // "email": "dungsuatentoooi@gmail.com",
        // "address": "HD-VN",
        // "identityNumber": "030099900102",
        // "identityDate": "2021-07-20T09:14:19.271Z",
        // "identityPlace": "HN-VN",
        // "joinDate": "2021-07-20T09:14:19.271Z",
        // "martialStatus": 0,
        // "educationalBackground": 0,
        // "qualificationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        // "departmentId": "17120d02-6ab5-3e43-18cb-66948daf6128",
        // "positionId": "548dce5f-5f29-4617-725d-e2ec561b0f41",
        // "workStatus": 1,
        // "personalTaxCode": "000111222",
        // "salary": 1800000,
        // "positionCode": "P94",
        // "positionName": "Fresher",
        // "departmentCode": "string",
        // "departmentName": "Phòng Đào tạo",
        // "qualificationName": null
    };
    newEmployee.employeeCode = $('#EmployeeCode').val();
    newEmployee.fullName = $('#FullName').val();
    newEmployee.dateOfBirth = $('#DateOfBirth').val();
    newEmployee.gender = $('#GenderName').attr('value');
    newEmployee.identityNumber = $('#IdentityNumber').val();
    newEmployee.identityPlace = $('#IdentityPlace').val();
    newEmployee.identityDate = $('#IdentityDate').val();
    newEmployee.phoneNumber = $('#PhoneNumber').val();
    newEmployee.email = $('#Email').val();
    newEmployee.departmentId = $('#DepartmentName').attr('value');
    newEmployee.positionId = $('#PositionName').attr('value');
    newEmployee.salary = CommonFn.formatNumber($('#Salary').val());
    // newEmployee.workStatus = $('#WorkStatus').text();
    console.log(myurl, mymethod, $('#Salary').val(), newEmployee)
    validateInputFormat();
    // $.ajax({
    //     headers: {
    //         'Accept': 'application/json',
    //         'Content-Type': 'application/json'
    //     },
    //     url: myurl,
    //     method: mymethod,
    //     data: JSON.stringify(newEmployee),
    //     dataType: 'json',
    //     'contentType': 'application/json',
    //     success: function() {
    //         alert(mess)
    //         location.reload();
    //     },
    //     error: function(e) {
    //         console.log("error", e);
    //     }
    // })
}