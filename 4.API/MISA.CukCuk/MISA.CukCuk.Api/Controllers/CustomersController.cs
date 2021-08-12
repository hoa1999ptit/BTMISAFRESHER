using Dapper;
using Microsoft.AspNetCore.Mvc;
using MISA.CukCuk.Api.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
        /// API hiển thị tất cả các nhân viên
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
                var response = StatusCode(200, customers);
                return response;
            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex,
                    userMsg = "Có lỗi xảy ra! vui lòng liên hệ với MISA.",
                    errorCode = "misa-001",
                    moreInfo = "https://openapi.misa.com.vn/errorcode/misa-001",
                    traceId = "ba9587fd-1a79-4ac5-a0ca-2c9f74dfd3fb"
                };
                return StatusCode(500, errorObj);
            }
        }

            /// <summary>
            /// API hiển thị nhân viên theo id
            /// </summary>
            /// <param name="customerId"></param>
            /// <returns></returns>
            [HttpGet("{customerId}")]
        public IActionResult GetẸmployeeById(Guid customerId)
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
                var response = StatusCode(200, customer);
                return response;
            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex,
                    userMsg = "Có lỗi xảy ra! vui lòng liên hệ với MISA.",
                    errorCode = "misa-001",
                    moreInfo = "https://openapi.misa.com.vn/errorcode/misa-001",
                    traceId = "ba9587fd-1a79-4ac5-a0ca-2c9f74dfd3fb"
                };
                return StatusCode(500, errorObj);
            }



        }

        /// <summary>
        /// API thêm mới 1 bản ghi nhân viên
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult InsertCustomer(Customer customer)
        {
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

        /// <summary>
        /// Xóa nhân viên
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpDelete("{customerId}")]
        public IActionResult DeleteCustomerById(Guid customerId)
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
            var response = StatusCode(200, customer);
            return response;
        }

        /// <summary>
        /// Sửa 
        /// </summary>
        [HttpPut("{customerId}")]
        public IActionResult UpdateCustomer(Guid customerId, Customer customer)
        {

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
