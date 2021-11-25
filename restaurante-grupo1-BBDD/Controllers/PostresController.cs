using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using restaurante_grupo1_BBDD.models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace restaurante_grupo1_BBDD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostresController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public PostresController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        //consultar postres
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select id,categoria,nombre,img,tokenimg,descrip,precio,actualizarinfo,nomsinespacio
                        from
                        platos
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TestAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult(table);
        }

        //insertar postres
        [HttpPost("{categoria},{nombre},{img},{tokenimg},{descrip},{precio}")]
        public object PostAperitivos(string categoria, string nombre, string img, string tokenimg, string descrip, int precio)
        {
            try
            {
                string query = @"insert into platos (categoria,nombre,img,tokenimg,descrip,precio,actualizarinfo,nomsinespacio)
                            values
                            (@categoria,@nombre,@img,@tokenimg,@descrip,@precio,@actualizarinfo,@nomsinespacio)
                ";
                string sqlDataSource = _configuration.GetConnectionString("TestAppCon");
                using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
                {

                    using (MySqlCommand myCommand = new MySqlCommand())
                    {
                        myCommand.Connection = mycon;
                        myCommand.CommandType = CommandType.Text;
                        myCommand.CommandText = query;
                        myCommand.Parameters.AddWithValue("@categoria", categoria);
                        myCommand.Parameters.AddWithValue("@nombre", nombre);
                        myCommand.Parameters.AddWithValue("@img", "https://firebasestorage.googleapis.com/v0/b/restaurantetic21.appspot.com/o/postres%2F" + img);
                        myCommand.Parameters.AddWithValue("@tokenimg", tokenimg);
                        myCommand.Parameters.AddWithValue("@descrip", descrip);
                        myCommand.Parameters.AddWithValue("@precio", precio);
                        myCommand.Parameters.AddWithValue("@actualizarinfo", "#" + Regex.Replace(nombre, @" ", "_"));
                        myCommand.Parameters.AddWithValue("@nomsinespacio", Regex.Replace(nombre, @" ", "_"));
                        try
                        {
                            mycon.Open();
                            myCommand.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            // other codes here
                            // do something with the exception
                            // don't swallow it.
                        }
                    }
                    return null;
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Acutalizar postres
        [HttpPut]
        public JsonResult Put(Postres pos)
        {

            string query = @"
                        update platos set 
                        nombre = @EnsaladasNombre,
                        img = @EnsaladasImagen,
                        tokenimg = @tokenimg,
                        descrip =@EnsaladasDescrip,
                        precio = @EnsaladasPrecio,
                        actualizarinfo =@EnsaladasActualizarinfo,
                        nomsinespacio =@Ensaladasnomsinespacio
                        where Id =@EnsaladasId;
                        
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TestAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@EnsaladasId", pos.id);
                    myCommand.Parameters.AddWithValue("@EnsaladasNombre", pos.nombre);
                    myCommand.Parameters.AddWithValue("@EnsaladasImagen", "https://firebasestorage.googleapis.com/v0/b/restaurantetic21.appspot.com/o/postres%2F" + pos.url);
                    myCommand.Parameters.AddWithValue("@tokenimg", pos.tokenimg);
                    myCommand.Parameters.AddWithValue("@EnsaladasDescrip", pos.descripcion);
                    myCommand.Parameters.AddWithValue("@EnsaladasPrecio", pos.precio);
                    myCommand.Parameters.AddWithValue("@EnsaladasActualizarinfo", "#" + Regex.Replace(pos.nombre, @" ", "_"));
                    myCommand.Parameters.AddWithValue("@Ensaladasnomsinespacio", Regex.Replace(pos.nombre, @" ", "_"));

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Updated Successfully");
        }

        //eliminar un plato de postres
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                        delete from platos 
                        where Id=@EnsaladaId;
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TestAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@EnsaladaId", id);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Deleted Successfully");
        }
    }
}
