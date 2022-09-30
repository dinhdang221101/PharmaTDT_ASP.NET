using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using WebMVC1.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebMVC1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        readonly string ConnectionString = "Data Source=TRID\\SERVERTEST;Initial Catalog=NHATHUOC;Persist Security Info=True;User ID=sa;Password=123";

        [HttpPost]
        public IActionResult Buy([FromForm] Order order)
        {
            string query = @"insert into orderT values(@email, @total, @thuocId, @quantity);";
            string sqlDataSource = ConnectionString;
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@email", order.email);
                    myCommand.Parameters.AddWithValue("@total", order.total);
                    myCommand.Parameters.AddWithValue("@thuocId", order.thuocId);
                    myCommand.Parameters.AddWithValue("@quantity", order.quantity);
                    myReader = myCommand.ExecuteReader();
                    myReader.Close();
                    myCon.Close();
                }
            }

            return Ok(new
            {
                status = 200,
                message = "Đặt hàng thành công",
                Data = order
            });
        }

        [HttpGet("{email}")]
        public IActionResult GetAllOrder(string email)
        {
            string query = @"select * from orderT where email = @email;";
            string sqlDataSource = ConnectionString;
            SqlDataReader myReader;
            DataTable orderList = new DataTable();
            var ordersList = new List<object>();
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@email", email);
                    myReader = myCommand.ExecuteReader();
                    orderList.Load(myReader);
                    
                    foreach (DataRow row in orderList.Rows)
                    {
                        var order = new {
                            orderId = row["orderId"], 
                            email = row["email"], 
                            total = row["total"], 
                            thuocId = row["thuocId"], 
                            quantity = row["quantity"], 
                        };
                        //Console.WriteLine(order);
                        //Console.WriteLine(row["orderId"] + ",  " + row["total"] + ",  " + row["email"] + ",  " + row["thuocId"] + ",  " + row["quantity"]);
                        ordersList.Add(order);
                        //Console.WriteLine(ordersList);
                    }
                    myReader.Close();
                    myCon.Close();
                }
            }

            return Ok(new
            {
                status = 200,
                message = "Danh sách orders",
                Data = ordersList
            });
        }
    }
}
