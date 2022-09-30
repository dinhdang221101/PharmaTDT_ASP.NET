using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using WebMVC1.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace WebMVC1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        readonly string ConnectionString = "Data Source=TRID\\SERVERTEST;Initial Catalog=NHATHUOC;Persist Security Info=True;User ID=sa;Password=123";

        [Route("add")]
        [HttpPost]
        public IActionResult AddProduct([FromForm] Thuoc thuoc)
        {
            string query = @"insert into thuoc values(@thuocId, @name, @price, @description, @image);";
            String Cquery = @"select * from thuoc where thuocId = @thuocId";
            string sqlDataSource = ConnectionString;
            SqlDataReader myReader;
            DataTable dt = new DataTable();
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(Cquery, myCon))
                {
                    myCommand.Parameters.AddWithValue("@thuocId", thuoc.thuocId);
                    myReader = myCommand.ExecuteReader();
                    dt.Load(myReader);
                    if (dt.Rows.Count > 0)
                    {
                        myReader.Close();
                        myCon.Close();
                        return Ok(new { status = 400, message = "Id này đã có trên kệ hàng" });
                    }

                }

                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@thuocId", thuoc.thuocId);
                    myCommand.Parameters.AddWithValue("@name", thuoc.name);
                    myCommand.Parameters.AddWithValue("@price", thuoc.price);
                    myCommand.Parameters.AddWithValue("@description", thuoc.description);
                    myCommand.Parameters.AddWithValue("@image", thuoc.image);
                    myReader = myCommand.ExecuteReader();
                    myReader.Close();
                    myCon.Close();
                }
            }

            return Ok(new
            {
                status = 200,
                message = "Thêm sản phẩm thành công",
            });
        }

        [HttpGet]
        public IActionResult GetAllProduct()
        {
            string query = @"select * from thuoc;";
            string sqlDataSource = ConnectionString;
            SqlDataReader myReader;
            DataTable productList = new DataTable();
            var productsList = new List<object>();
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    productList.Load(myReader);

                    foreach (DataRow row in productList.Rows)
                    {
                        var product = new
                        {
                            thuocId = row["thuocId"],
                            name = row["name"],
                            price = row["price"],
                            description = row["description"],
                            image = row["image"],
                        };

                        productsList.Add(product);
                    }
                    myReader.Close();
                    myCon.Close();
                }
            }
            return Ok(new
            {
                status = 200,
                message = "Danh sách sản phẩm",
                Data = productsList
            });
        }

        [HttpGet("{thuocId}")]
        public IActionResult GetProductById(string thuocId)
        {
            string query = @"select * from thuoc where thuocId = @thuocId;";
            string sqlDataSource = ConnectionString;
            SqlDataReader myReader;
            DataTable orderList = new DataTable();
            var productD = new object();
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@thuocId", thuocId);
                    myReader = myCommand.ExecuteReader();
                    orderList.Load(myReader);
                    bool check = false;
                    foreach (DataRow row in orderList.Rows)
                    {
                        check = true;
                        var product = new
                        {
                            thuocId = row["thuocId"],
                            name = row["name"],
                            price = row["price"],
                            description = row["description"],
                            image = row["image"],
                        };

                        productD = product;
                    }
                    if (!check)
                    {
                        return Ok(new
                        {
                            status = 400,
                            message = "Không có thuốc ứng với id " + thuocId,
                        });
                    }
                    myReader.Close();
                    myCon.Close();
                }
            }

            return Ok(new
            {
                status = 200,
                message = "Lấy chi tiết thuốc thành công với id " + thuocId,
                Data = productD
            });
        }

        [Route("delete/{thuocId}")]
        [HttpDelete]
        public IActionResult DeleteProductById(string thuocId)
        {
            string query = @"delete from thuoc where thuocId = @thuocId;";
            string sqlDataSource = ConnectionString;
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@thuocId", thuocId);
                    myReader = myCommand.ExecuteReader();
                    myReader.Close();
                    myCon.Close();
                }
            }

            return Ok(new
            {
                status = 200,
                message = "Xóa sản phẩm thuốc thành công với id " + thuocId
            });
        }

        [Route("edit/{thuocId}")]
        [HttpPut]
        public IActionResult EditProductById(string thuocId, [FromForm] Thuoc thuoc)
        {
            string query = @"update thuoc 
                            set name = @name, price = @price, description = @description, image = @image
                            where thuocId = @thuocId";
            string sqlDataSource = ConnectionString;
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@name", thuoc.name);
                    myCommand.Parameters.AddWithValue("@price", thuoc.price);
                    myCommand.Parameters.AddWithValue("@description", thuoc.description);
                    myCommand.Parameters.AddWithValue("@image", thuoc.image);
                    myCommand.Parameters.AddWithValue("@thuocId", thuocId);
                    myReader = myCommand.ExecuteReader();
                    myReader.Close();
                    myCon.Close();
                }
            }

            return Ok(new
            {
                status = 200,
                message = "Chỉnh sửa phẩm thuốc thành công với id " + thuocId
            });
        }
    }
}
