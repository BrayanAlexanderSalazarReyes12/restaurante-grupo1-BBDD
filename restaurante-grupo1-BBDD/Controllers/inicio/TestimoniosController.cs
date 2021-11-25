using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using restaurante_grupo1_BBDD.models.Inicio;
using System;
using System.Data;
using System.Data.SqlClient;

namespace restaurante_grupo1_BBDD.Controllers.inicio
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestimoniosController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public TestimoniosController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        // CONSULTAR IMAGENES DEL SCROLL
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select *
                        from
                        testimonios
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


        //Insertar imagen al scroll
        [HttpPost]
        public JsonResult Post(Testimonios ts)
        {
            try
            {
                string query = @"insert into testimonios (ImgTest,TextTest)
                            values
                            (@Img,@Texto)
                ";
                string sqlDataSource = _configuration.GetConnectionString("TestAppCon");
                using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
                {
                    using (MySqlCommand myCommand = new MySqlCommand())
                    {
                        myCommand.Connection = mycon;
                        myCommand.CommandType = CommandType.Text;
                        myCommand.CommandText = query;
                        myCommand.Parameters.AddWithValue("@Img", ts.ImgTest);
                        myCommand.Parameters.AddWithValue("@Texto", ts.TextTest);
                        
                        try
                        {
                            mycon.Open();
                            myCommand.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            return new JsonResult(ex);
                            // other codes here
                            // do something with the exception
                            // don't swallow it.
                        }
                    }
                }
                return new JsonResult("Agregado correctamente");
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);

            }
        }


        //Acutalizar img_acordion
        [HttpPut]
        public JsonResult Put(Testimonios ts)
        {
            string query = @"
                        update testimonios set 
                        ImgTest = @ImgTest,
                        TextTest = @TextTest
                        where IdTest = @IdTest
                        
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TestAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {

                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    try
                    {
                        myCommand.Parameters.AddWithValue("@ImgTest", ts.ImgTest);
                        myCommand.Parameters.AddWithValue("@TextTest", ts.TextTest);
                        myCommand.Parameters.AddWithValue("@IdTest", ts.IdTest);
                        try
                        {
                            mycon.Open();
                            myReader = myCommand.ExecuteReader();
                            table.Load(myReader);

                            myReader.Close();
                            mycon.Close();
                        }
                        catch (SqlException ex)
                        {
                            return new JsonResult(ex);
                        }


                    }
                    catch (SqlException ex)
                    {
                        return new JsonResult(ex);
                    }

                }

            }
            return new JsonResult("Imagen actualizada correctamente");
        }


        //Eliminar un plato de ensalada
        [HttpDelete]
        public JsonResult Delete(Testimonios ts)
        {
            string query = @"
                        delete from testimonios 
                        where IdTest = @Id;
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TestAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@Id", ts.IdTest);

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
