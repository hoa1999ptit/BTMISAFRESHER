/**
 * Biến dùng chung
 */
employeeList = [];
divisionList = [];
positionList = [];

//Init function
$(function() {
    loadAPIEmployee();
    loadAPIDivision();
    loadAPIPosition();

    // listenFilter();
})

/**
 * Gọi API lấy danh sách nhân viên
 * Gọi hàm hiển thị danh sách nhân viên
 */
function loadAPIEmployee() {
    $.ajax({
        type: "GET",
        url: "http://cukcuk.manhnv.net/v1/Employees",
        success: function(result) {
            if (result != null) {
                employeeList = result;
                loadTableEmployee(employeeList);
                console.log("Employee: ", result);
            }
        },
        error: function(e) {
            console.log("ERROR: ", e);
        }
    });
}

/**
 * Gọi API lấy dữ liệu phòng ban
 * GỌi hàm hiển thị phòng ban vào combobox
 */
function loadAPIDivision() {
    $.ajax({
        type: "GET",
        url: "http://cukcuk.manhnv.net/api/Department",
        success: function(result) {
            if (result != null) {
                divisionList = result;
                loadDropdownDepartment(divisionList);
                console.log("Department: ", divisionList);
            }
        },
        error: function(e) {
            console.log("ERROR: ", e);
        }
    });
}
/**
 * Gọi API lấy dữ liệu Chức vụ
 * Gọi hàm hiển thị chức vụ vào combobox
 */
function loadAPIPosition() {
    $.ajax({
        type: "GET",
        url: "http://cukcuk.manhnv.net/v1/Positions",
        success: function(result) {
            if (result != null) {
                positionList = result;
                loadDropdownPosition(positionList);
                console.log("Position: ", result);
            }
        },
        error: function(e) {
            console.log("ERROR: ", e);
        }
    });
}

/**
 * Hiển thị các lựa chọn của Phòng ban vào combobox
 * @param {Danh sách các phòng ban} itemList 
 */
function loadDropdownDepartment(itemList) {
    var newdivs = '';
    itemList.forEach((item, index) => {
        newdivs += `<div class="item" value=${item.DepartmentId}>${item.DepartmentName}</div>`
    });
    $(`.dropdown.division .itemwrapper`).append(newdivs);
}

/**
 * Hiển thị các lựa chọn của Chức vụ vào combobox
 * @param {Danh sách các Chức vụ} itemList 
 */
function loadDropdownPosition(itemList) {
    var newdivs = '';
    itemList.forEach((item, index) => {
        newdivs += `<div class="item" value = ${item.PositionId}>${item.PositionName}</div>`
    });
    $(`.dropdown.position .itemwrapper`).append(newdivs);
}

/**
 * Hiển thị thông tin nhân viên từ danh sách vào bảng
 * @param {Danh sách nhân viên} employeeList 
 */
function loadTableEmployee(employeeList) {
    var newrows = '';
    employeeList.forEach((item, index) => {
        if (item.DateOfBirth == null) {
            item.DateOfBirth = new Date();
        }
        switch (item.WorkStatus) {
            case 1:
                item.WorkStatus = "Đã nghỉ việc"
                break;
            case 2:
                item.WorkStatus = "Đang thử việc"
                break;
            case 3:
                item.WorkStatus = "Đã làm việc"
                break;
            default:
                item.WorkStatus = "Chưa rõ"
        }
        // debugger;
        if (item.GenderName != ("Nam" || "Nữ")) item.GenderName = "Khác";
        newrows += `<tr class empid=${item.EmployeeId}>
                <td>${index + 1}</td>
                <td value="${item.EmployeeId}">${item.EmployeeCode}</td>
                <td>${item.FullName}</td>
                <td>${item.GenderName}</td>
                <td class="date">${CommonFn.formatDateDMY(item.DateOfBirth)}</td>
                <td>${item.PhoneNumber}</td>
                <td>${item.Email}</td>
                <td>${item.PositionName}</td>
                <td>${item.DepartmentName}</td>
                <td class="money">${CommonFn.formatMoney(item.Salary)}</td>
                <td>${item.WorkStatus}</td>
                </tr>`
    });
    $(`#table-employee tbody`).append(newrows)
}

/**
 * Gọi API lấy mã nhân viên mới
 * @returns mã nhân viên mới
 */
function getNewEmployeeCode() {
    $.ajax({
        url: "http://cukcuk.manhnv.net/v1/Employees/NewEmployeeCode",
        type: "GET",
        success: function(result) {
            console.log("newcode:", result);
            $('#EmployeeCode').val('NV' + (result + '').replaceAll('NV', ''));
            return result;
        },
        error: function(e) {
            console.log("ERROR: ", e);
            alert('Không thể  lấy được mã NV mới, lỗi server');
        }
    });
}


/**
 * Tải lại trang khi bấm nút refresh
 */
$('.button.icon-refresh').click(function() {
    location.reload();
})



/**
 * Lắng nghe sự kiện click btn- Thêm nhân viên
 * Làm mờ div chính
 */
$(".button.add-employee").on("click", function() {
    getNewEmployeeCode();
    showEmployeeForm();

    console.log("clicked")
});

/***
 * Hiện form thông tin nhân viên, làm mờ BG
 */
function showEmployeeForm() {
    $(".popup.add-employee").css('display', "block");
    $(".main-section").addClass("disabledbutton");
    $(".popup.add-employee #EmployeeCode").focus();
    console.log($(".popup.add-employee #EmployeeCode").val())
};

/**
 * Đóng form thông tin nhân viên
 * Reset trạng thái form về 0 ( thêm mới)
 */
$(".popup .btn-close,.button.btn-cancel").on("click", function() {
    $(".popup.add-employee").css('display', "none");
    $(".main-section").removeClass("disabledbutton");
    $('.popup.add-employee').attr('formmode', 0);
})

dbclickonEmployeeTR();


/**
 * Lắng nghe sự kiện dbclick vào dòng thông tin nhân viên
 * Hiển thị form thông tin nhân viên
 * Cập nhật trạng thái formmode:0  (Thêm mới), 1(Cập nhật)
 */
function dbclickonEmployeeTR() {
    $('.table-employee tbody ').on('dblclick', 'tr', function() {
        // thistd = $(this).find('td:eq(1)');
        EmployeeId = $(this).attr("empid");
        getEmployeeDetail(EmployeeId);
        showEmployeeForm();
        $('.popup .add-employee').attr('formmode', 1);
        // $('.popup .add-employee').attr('empid', employeeId)  
        // Error Hiển thị nhân viên nằm ở đây, nhưng do async lỗi, chuyển vào trong getEmployeeDetail
    })
}
/**
 * Hiển thị thông tin của nhân viên lên form, load các giá trị vào các trường, combobox
 * @param {Object chứa thông tin nhân viên} fe 
 */
function displayEmployeeInfo(fe) {
    $('#EmployeeCode').val(fe.EmployeeCode);
    $('#FullName').val(fe.FullName);
    $('#DateOfBirth').val(CommonFn.formatDateYMD(fe.DateOfBirth));
    console.log(fe.DateOfBirth, CommonFn.formatDateYMD(fe.DateOfBirth))
    $('#IdentityDate').val(CommonFn.formatDateYMD(fe.IdentityDate));
    $('#IdentityPlace').val(fe.IdentityPlace);
    $('#Email').val(fe.Email);
    $('#PhoneNumber').val(fe.PhoneNumber);
    $('#Salary').val(CommonFn.formatMoney(fe.Salary));
    if (fe.Gender == 0) {
        feGender = 'Nữ';
    } else if (fe.Gender == 1) {
        feGender = 'Nam';
    } else {
        feGender = "Chưa nhập";
    }
    $('#GenderName').text(feGender);
    console.log("Position", positionList);
    positionList.forEach((position, index) => {
        console.log(fe.PositionId, position.PositionId);
        if (position.PositionId == fe.PositionId) {
            $('#PositionName').text(fe.PositionName);
            let thisdropdown = $('#PositionName').nParent(2);
            thisdropdown.find('.divtext').attr('value', fe.PositionId);
            thisdropdown.find(`.itemwrapper .item[value="${fe.PositionId}"]`).addClass('selected');

            console.log($('#PositionName').nParent(2).find('.divtext'))
        }
    })
    divisionList.forEach((deparment, index) => {
        if (deparment.DepartmentId == fe.DepartmentId) {
            $('#DepartmentName').text(fe.DepartmentName);
            let thisdropdown = $('#DepartmentName').nParent(2);
            thisdropdown.find('.divtext').attr('value', fe.DepartmentId);
            thisdropdown.find(`.itemwrapper .item[value='${fe.DepartmentId}']`).addClass('selected');

        }
    })

    $('#WorkStatus').text(fe.WorkStatus);

}

/**
 * Đặt các giá trị của form về mặc định
 */
function resetEmployeeForm() {
    let me = $('.popup.add-employee');
    me.attr('value', -1)
    me.find('input').val('');
    me.find('.dropdown').attr('value', -1);
    me.find('.item').removeClass('selected');

}

/**
 * Lấy thông tin nhân viên dựa vào ID, hiển thị thông tin lên form
 * @param {ID của nhân viên cần lấy} employeeId 
 */

function getEmployeeDetail(employeeId) {
    //Error Dùng API lấy thông tin, nhưng API bị thiếu thông tin, sử dụng thông tin từ DS NVien
    employeeList.forEach((employee, index) => {
        if (employee.EmployeeId == employeeId) {
            console.log(employee);
            fe = employee;
            resetEmployeeForm();
            displayEmployeeInfo(fe);
            $('.popup.add-employee').attr('formmode', 1);
            $('.popup.add-employee').attr('empid', employeeId);
            return;
        }

    })



    // $.ajax({
    //     type: "GET",
    //     url: "http://cukcuk.manhnv.net/v1/Employees/" + employeeId,
    //     // async: false,
    //     success: function(result) {
    //         if (result != null) {
    //             fe = result;
    //             displayEmployeeInfo(fe);
    //             $('.popup.add-employee').attr('formmode', 1);
    //             $('.popup.add-employee').attr('empid', employeeId);

    //             console.log("FOunded Emp", fe);
    //         }
    //     },
    //     error: function(e) {
    //         console.log("ERROR: ", e);
    //     }
    // });

}

$('.table-employee tbody').on('mousedown', 'tr', function(event) {
    if (event.which == 3) {
        console.log('Right Mouse button pressed.');
        if (confirm("Bạn muốn xóa nhân viên này?")) {
            EmployeeId = $(this).attr("empid");
            deleteEmployee(EmployeeId);

        }
    }
});

function deleteEmployee(EmployeeId) {
    $.ajax({
        type: "DELETE",
        url: "http://cukcuk.manhnv.net/v1/Employees/" + EmployeeId,
        // async: false,
        success: function(result) {
            if (result != null) {
                fe = result;
                console.log("FOunded Emp", fe);
                location.reload();
            }
        },
        error: function(e) {
            console.log("ERROR: ", e);
        }
    });
}