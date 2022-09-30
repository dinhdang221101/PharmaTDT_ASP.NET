using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using WebMVC1.Models;
using Microsoft.AspNetCore.Http;

namespace WebMVC1.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        readonly string ConnectionString = "Data Source=TRID\\SERVERTEST;Initial Catalog=NHATHUOC;Persist Security Info=True;User ID=sa;Password=123";

        [Route("register")]
        [HttpPost]
        public IActionResult Register([FromForm] Account user, [FromForm] string confirm_password)
        {
            if(user.email.Length == 0 || user.password.Length == 0 || confirm_password.Length == 0)
            {
                return Ok( new { status = 400, message = "Đăng ký thất bại vui lòng nhập đầy đủ thông tin"});
            } else if(confirm_password != user.password)
            {
                return Ok(new { status = 400, message = "Đăng ký thất bại mật khẩu và mật khẩu xác nhận không trùng khớp" });
            }

            String qString = @"select * from Account where email = @email";
            
            DataTable dt = new DataTable();
            string query = @"insert into Account values(@email, @password, 'false');";
            string sqlDataSource = ConnectionString;
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(qString, myCon))
                {
                    myCommand.Parameters.AddWithValue("@email", user.email);
                    myReader = myCommand.ExecuteReader();
                    dt.Load(myReader);
                    if(dt.Rows.Count > 0)
                    {
                        myReader.Close();
                        myCon.Close();
                        return Ok(new { status = 400, message = "Email này đã được đăng ký" });
                    }
                    
                }

                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@email", user.email);
                    myCommand.Parameters.AddWithValue("@password", user.password);
                    myReader = myCommand.ExecuteReader();
                    myReader.Close();
                    myCon.Close();
                }
            }

            return Ok(new
            {   status = 200,
                message = "Đăng ký thành công", Data = user
            });
        }

        [Route("login")]
        [HttpPost]
        public IActionResult Login([FromForm]Account user)
        {
            if (user.email.Length == 0 || user.password.Length == 0)
            {
                return Ok(new { status = 400, message = "Đăng nhập thất bại vui lòng nhập đầy đủ thông tin" });
            }
            String query = @"select * from Account where email = @email and password = @password";

            DataTable dt = new DataTable();
            string sqlDataSource = ConnectionString;
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@email", user.email);
                    myCommand.Parameters.AddWithValue("@password", user.password);
                    myReader = myCommand.ExecuteReader();
                    
                    if (myReader.Read() == false)
                    {
                        return Ok(new { status = 400, message = "Email hoặc mật khẩu không chính xác" });
                    }
                    user.isAdmin = (bool)myReader["isAdmin"];
                    myReader.Close();
                    myCon.Close();
                }
            }
            HttpContext.Session.SetString("_User", user.email);
            var sessionData = HttpContext.Session.GetString("_User");
            Console.WriteLine(sessionData);
            return Ok(new
            {
                status = 200,
                message = "Đăng nhập thành công",
                Data = new {email = user.email, isAdmin = user.isAdmin}
            });
        }
    }
}
