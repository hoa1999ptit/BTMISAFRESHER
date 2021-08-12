using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.CukCuk.Api.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace MISA.CukCuk.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        //Truy cập vào database
        //1. Khởi tạo thông tin kết nối database
        public static string connectionString = "Host= 47.241.69.179;" +
                "Database = MISA.CukCuk_Demo_NVMANH;" +
                "User Id = dev;" +
                "Password = 12345678;";


        /// <summary>
        /// API hiển thị tất cả các khách hàng
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCustomers()
        {
            try
            {
                //2. Khởi tạo đối tượng kết nối với database
                IDbConnection dbConnection = new MySqlConnection(connectionString);

                //3. Lấy dữ liệu
                var sqlCommand = "SELECT * FROM Customer";
                var customers = dbConnection.Query<object>(sqlCommand);

                //4. Trả về cho client
                // check lỗi
                if (customers.Count()>0)
                {
                    var response = StatusCode(200, customers);
                    return response;
                }else
                {

                    var erroObj = new
                    {
                        devMsg = "",
                        userMsg = Properties.Resources.Exception_NoData,
                        errorCode = "misa-001",
                        moreInfo = Properties.Resources.Exception_MoreInfo

                    };
                    return StatusCode(204, erroObj);
                }


            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex,
                    userMsg = Properties.Resources.Exception_ErrMsg,
                    errorCode = "misa-001",
                    moreInfo = Properties.Resources.Exception_MoreInfo
                    //traceId = "ba9587fd-1a79-4ac5-a0ca-2c9f74dfd3fb"
                };
                return StatusCode(500, errorObj);
            }
        }

        /// <summary>
        /// lấy thông tin khách hàng
        /// </summary>
        /// <param name="customerId">id khách hàng</param>
        /// <returns></returns>
        [HttpGet("{customerId}")]
        public IActionResult Get(string customerId)
        {
            try
            {
                //2. Khởi tạo đối tượng kết nối với database
                IDbConnection dbConnection = new MySqlConnection(connectionString);

                //3. Lấy dữ liệu
                var sqlCommand = $"SELECT * FROM Customer WHERE CustomerId= @CustomerIdParam";

                //De trach loi SQL Injection           
                DynamicParameters parameters = new DynamicParameters();

                parameters.Add("@CustomerIdParam", customerId);

                var customer = dbConnection.QueryFirstOrDefault<Customer>(sqlCommand, param: parameters);

                //4. Trả về cho client
                //var response = StatusCode(200, customer);
                //return response;
                if (customer != null)
                {
                    return StatusCode(200, customer);
                }
                return NoContent();


            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex,
                    userMsg = Properties.Resources.Exception_ErrMsg,
                    errorCode = "misa-001",
                    moreInfo = Properties.Resources.Exception_MoreInfo,
                    traceId = "ba9587fd-1a79-4ac5-a0ca-2c9f74dfd3fb"
                };
                return StatusCode(500, errorObj);
            }
        }

        /// <summary>
        /// API thêm mới 1 bản ghi khách hàng
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult InsertCustomer(Customer customer)
        {
            try
            {
                //kiểm tra dữ liệu hợp lệ

                //1. mã kh
                if (customer.CustomerCode == "" || customer.CustomerCode == null)
                {
                    //return BadRequest("Mã KH không được phép để trống");
                    var errorObject = new
                    {
                        userMsg = Properties.Resources.Exception_CodeInvalid,
                        errorCode = "misa-003",
                        moreInfo = Properties.Resources.Exception_MoreInfo
                    };


                    return StatusCode(400, errorObject);
                }
                //2. email đúng định dạng
                bool isEmail = System.Text.RegularExpressions.Regex.IsMatch(customer.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);


                if (isEmail == false)
                {
                    var objectError = new
                    {
                        userMsg = Properties.Resources.Exception_EmailInvalid,
                        errorCode = "misa-001",
                        moreInfo = Properties.Resources.Exception_MoreInfo

                    };
                    return StatusCode(400, objectError);
                }


                //2. Khởi tạo đối tượng kết nối với database
                IDbConnection dbConnection = new MySqlConnection(connectionString);

                //Khai báo dyanamicParam:
                var dyanamicParam = new DynamicParameters();

                //3. Thêm dữ liệu vào trong database
                var columnsName = string.Empty;

                var columnsParam = string.Empty;

                //Đọc từng property của object:
                var properties = customer.GetType().GetProperties();
                foreach (var prop in properties)
                {
                    //Lấy tên của prop:
                    var propName = prop.Name;

                    //Lấy value của prop
                    var propValue = prop.GetValue(customer);

                    //Lấy kiểu dữ liệu của prop
                    var propType = prop.PropertyType;

                    //Thêm param tương ứng với mỗi property của đối tượng
                    dyanamicParam.Add($"@{propName}", propValue);

                    columnsName += $"{propName},";

                    columnsParam += $"@{propName},";

                }

                columnsName = columnsName.Remove(columnsName.Length - 1, 1);

                columnsParam = columnsParam.Remove(columnsParam.Length - 1, 1);

                var sqlCommand = $"INSERT INTO Customer({columnsName}) VALUES({columnsParam})";

                var rowsEffects = dbConnection.Execute(sqlCommand, param: dyanamicParam);

                //4. Trả về cho client
                var response = StatusCode(200, rowsEffects);
                return response;
            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex,
                    userMsg = Properties.Resources.Exception_ErrMsg,
                    errorCode = "misa-001",
                    moreInfo = Properties.Resources.Exception_MoreInfo
                    //traceId = "ba9587fd-1a79-4ac5-a0ca-2c9f74dfd3fb"
                };
                return StatusCode(400, errorObj);
            }
             
        }

        /// <summary>
        /// Xóa khách hàng
        /// </summary>
        /// <param name="customerId"> thông tin khách hàng</param>
        /// <returns></returns>
        [HttpDelete("{customerId}")]
        public IActionResult DeleteCustomerById(string customerId)
        {
            try
            {
                //2. Khởi tạo đối tượng kết nối với database
                IDbConnection dbConnection = new MySqlConnection(connectionString);

                //3. Lấy dữ liệu
                var sqlCommand = $"DELETE FROM Customer WHERE CustomerId= @CustomerIdParam";

                //De trach loi SQL Injection           
                DynamicParameters parameters = new DynamicParameters();

                parameters.Add("@CustomerIdParam", customerId);

                var customer = dbConnection.QueryFirstOrDefault<Customer>(sqlCommand, param: parameters);

                //4. Trả về cho client
                return StatusCode(200, customer);

            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex.Message,
                    userMsg = Properties.Resources.Exception_ErrId,
                    errorCode = "misa-001",
                    moreInfo = Properties.Resources.Exception_MoreInfo
                };
                return StatusCode(400, errorObj);
            }
        }


        /// <summary>
        /// Thêm mới một khách hàng
        /// </summary>
        /// <param name="customer">Thông tin khách hàng</param>
        /// <returns>số hàng đc thêm</returns>
        [HttpPost]
        public IActionResult AddCustomer([FromBody] Customer customer)
        {
            try
            {
                // Kiểm tra EmployeeCode empty không
                if (customer.CustomerCode == null || customer.CustomerCode == "")
                {
                    var errorObject = new
                    {
                        userMsg = Properties.Resources.Exception_CodeInvalid,
                        errorCode = "misa-003",
                        moreInfo = Properties.Resources.Exception_MoreInfo
                    };


                    return StatusCode(400, errorObject);
                }

                //Check email valid
                bool isEmail = System.Text.RegularExpressions.Regex.IsMatch(customer.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                if (isEmail == false)
                {
                    var objectError = new
                    {
                        userMsg = Properties.Resources.Exception_EmailInvalid,
                        errorCode = "misa-001",
                        moreInfo = Properties.Resources.Exception_MoreInfo

                    };
                    return StatusCode(400, objectError);
                }
                // 2. Khởi tạo đối tượng kết nối với db:
                IDbConnection dbConnection = new MySqlConnection(connectionString);

                // Khai báo DynamicParam:
                var dynamicParam = new DynamicParameters();
                // 3. Thêm dữ liệu vào trong database:
                var columnsName = string.Empty;
                var columnsParam = string.Empty;

                // Đọc từng property của object: 
                var properties = customer.GetType().GetProperties();

                //Duyệt từng property:
                foreach (var prop in properties)
                {
                    // Lấy tên của prop:
                    var propName = prop.Name;

                    // Lấy value của prop:
                    var propValue = prop.GetValue(customer);

                    // Lấy kiểu dữ liệu của prop:
                    var propType = prop.PropertyType;

                    // Thêm Param tương ứng vs mỗi property:
                    dynamicParam.Add($"@{propName}", propValue);

                    columnsName += $"{propName},";
                    columnsParam += $"@{propName},";
                }
                columnsName = columnsName.Remove(columnsName.Length - 1, 1);
                columnsParam = columnsParam.Remove(columnsParam.Length - 1, 1);

                var sqlCommand = $"INSERT INTO Customer({columnsName}) VALUES({columnsParam})";

                var rowEffects = dbConnection.Execute(sqlCommand, param: dynamicParam);

                var response = StatusCode(200, rowEffects);

                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex.Message,
                    userMsg = Properties.Resources.Exception_ErrMsg,
                    errorCode = "misa-001",
                    moreInfo = Properties.Resources.Exception_MoreInfo
                };
                return StatusCode(400, errorObj);
            }
        }

        /// <summary>
        /// sửa thông tin 1 khách hàng
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPut("{customerId}")]
        public IActionResult UpdateCustomer(Guid customerId, [FromBody] Customer customer)
        {
            try
            {
                // check thông tin kh
                if (customerId != customer.CustomerId)
                {
                    var errorObject = new
                    {
                        userMsg = Properties.Resources.Exception_ErrId,
                        errorCode = "misa-003",
                        moreInfo = Properties.Resources.Exception_MoreInfo
                    };
                    return BadRequest(errorObject);
                }

                if (customer.CustomerCode == null || customer.CustomerCode == "")
                {
                    var errorObject = new
                    {
                        userMsg = Properties.Resources.Exception_CodeInvalid,
                        errorCode = "misa-003",
                        moreInfo = Properties.Resources.Exception_MoreInfo
                    };


                    return StatusCode(400, errorObject);
                }

                //check email
                bool isEmail = System.Text.RegularExpressions.Regex.IsMatch(customer.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                if (isEmail == false)
                {
                    var objectError = new
                    {
                        userMsg = Properties.Resources.Exception_EmailInvalid,
                        errorCode = "misa-001",
                        moreInfo = Properties.Resources.Exception_MoreInfo

                    };
                    return StatusCode(400, objectError);
                }



                //2. Khởi tạo đối tượng kết nối với database
                IDbConnection dbConnection = new MySqlConnection(connectionString);

                //Khai báo dyanamicParam:
                var dyanamicParam = new DynamicParameters();

                //3. Thêm dữ liệu vào trong database
                var columnsUpadateParam = string.Empty;

                //Đọc từng property của object:         
                var properties = customer.GetType().GetProperties();

                foreach (var prop in properties)
                {
                    //Lấy tên của prop:
                    var propName = prop.Name;

                    //Lấy value của prop
                    var propValue = prop.GetValue(customer);

                    //Lấy kiểu dữ liệu của prop
                    var propType = prop.PropertyType;

                    //Thêm param tương ứng với mỗi property của đối tượng
                    dyanamicParam.Add($"@{propName}", propValue);

                    columnsUpadateParam += $"{propName} = '@{ propName}' ,";

                }

                columnsUpadateParam = columnsUpadateParam.Remove(columnsUpadateParam.Length - 1, 1);

                var sqlCommand = $"UPDATE Customer SET {columnsUpadateParam} WHERE CustomerId = @customerId";
                dyanamicParam.Add("@customerId", customerId);

                var rowsEffects = dbConnection.Execute(sqlCommand, param: dyanamicParam);

                //4. Trả về cho client
                var response = StatusCode(200, rowsEffects);
                return response;


            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex.Message,
                    userMsg = Properties.Resources.Exception_ErrMsg,
                    errorCode = "misa-001",
                    moreInfo = Properties.Resources.Exception_MoreInfo
                };
                return StatusCode(400, errorObj);
            }

        }

    }



    //public class CustomersController : ControllerBase
    //{
    //    //GET, POST, PUT, DELETE
    //    [HttpGet()]
    //    public IActionResult GetCustomers()
    //    {
    //        // Truy cập vào db:
    //        // 1. Khai báo thông tin kết nối db:
    //        var connectionString = "Host = 47.241.69.179;" +
    //            "Database = MISA.CukCuk_Demo_NVMANH;" +
    //            "User Id = dev;" +
    //            "Password = 12345678";

    //        // 2. Khởi tạo đối tượng kết nối với db:
    //        IDbConnection dbConnection = new MySqlConnection(connectionString);

    //        // 3. Lấy dữ liệu:
    //        var sqlCommand = "SELECT * FROM Customer";
    //        var customers = dbConnection.Query<Customer>(sqlCommand);

    //        // 4. Trả về cho client:
    //        return Ok(customers);
    //    }
    //    [HttpPost]
    //    public IActionResult InsertCustomer(Customer customer)
    //    {
    //        try
    //        {
    //            //Truy cập vào database:
    //            // 1.Khai báo đối tượng
    //            var connectionString = "Host = 47.241.69.179;" +
    //                "Database = MISA.CukCuk_Demo_NVMANH;" +
    //                "User Id = dev;" +
    //                "Password = 12345678";
    //            // 2.Khởi tạo đối tượng kết nối với database
    //            IDbConnection dbConnection = new MySqlConnection(connectionString);
    //            //khai báo dynamicParam:
    //            var dynamicParam = new DynamicParameters();

    //            // 3.Thêm dữ liệu vào database
    //            var columnsName = string.Empty;
    //            var columnsParam = string.Empty;

    //            //Đọc từng property của object:
    //            var properties = customer.GetType().GetProperties();


    //            //Duyệt từng property:
    //            foreach (var prop in properties)
    //            {
    //                //lấy tên của prop:
    //                var propName = prop.Name;

    //                //Lấy value của prop:
    //                var propValue = prop.GetValue(customer);

    //                //Lấy kiểu dữ liệu của prop:
    //                var propType = prop.PropertyType;

    //                //thêm param tương ứng với mỗi property của đối tượng
    //                dynamicParam.Add($"@{propName}", propValue);

    //                columnsName += $"@{propName},";
    //                columnsParam += $"@{propName},";
    //            }
    //            columnsName = columnsName.Remove(columnsName.Length - 1, 1);
    //            columnsParam = columnsParam.Remove(columnsParam.Length - 1, 1);
    //            var sqlCommand = $"INSERT INTO Customer({columnsName}) VALUES({columnsParam}) ";

    //            // Trả về cho client

    //            var response = StatusCode(200, customer);
    //            return response;
    //        }
    //        catch (Exception ex)
    //        {
    //            var errorObj = new
    //            {
    //                devMsg = ex,
    //                userMsg = "Có lỗi xảy ra! vui lòng liên hệ với MISA.",
    //                errorCode = "misa-001",
    //                moreInfo = "https://openapi.misa.com.vn/errorcode/misa-001",
    //                traceId = "ba9587fd-1a79-4ac5-a0ca-2c9f74dfd3fb"
    //            };
    //            return StatusCode(500, errorObj);
    //    }


    //        //200 OK - Thành công.
    //        //201 CREATED - Tạo dữ liệu thành công(POST method).
    //        //400 BAD REQUEST -Input của Client gửi không hợp lệ.
    //        //401 UNAUTHORIZED - Hệ thống chưa được ủy quyền.
    //        //403 FORBIDDEN - Máy chủ từ chối yêu cầu(ngay cả khi Client đã được ủy quyền).
    //        //404 NOT FOUND -Máy chủ không tìm thấy bất kỳ thứ gì với request-URL này.
    //        //500 INTERNAL SERVER ERROR - Có lỗi xảy ra phía máy chủ.




    //    }
    //}
}
