using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Practica.models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Practica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InicioController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public InicioController(IConfiguration configuration, IWebHostEnvironment env)
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
                        acordion
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

        // CONSULTAR UNA SOLA IMAGEN
        [HttpGet("{idacordion}")]
        public JsonResult Get(int idacordion)
        {
            string query = @"select *
                        from
                        acordion 
                        where idacordion = @id
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TestAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Connection = mycon;
                    myCommand.CommandType = CommandType.Text;
                    myCommand.CommandText = query;
                    myCommand.Parameters.AddWithValue("@id", idacordion);
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
        public JsonResult Post(Acordion ac )
        {
            try
            {
                string query = @"insert into acordion (img_acordion)
                            values
                            (@img_acordion)
                ";
                string sqlDataSource = _configuration.GetConnectionString("TestAppCon");
                using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
                {
                    using (MySqlCommand myCommand = new MySqlCommand())
                    {
                        myCommand.Connection = mycon;
                        myCommand.CommandType = CommandType.Text;
                        myCommand.CommandText = query;
                        myCommand.Parameters.AddWithValue("@img_acordion", ac.img_acordion);
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
                }
                return new JsonResult("Actualizado correctamente");
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
               
            }
        }


        //Acutalizar img_acordion
        [HttpPut]
        public JsonResult Put(Acordion img)
        {
            string query = @"
                        update acordion set 
                        img_acordion = @Acordionimg_acordion
                        where idacordion = @Acordionidacordion
                        
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
                        myCommand.Parameters.AddWithValue("@Acordionidacordion", img.idacordion);
                        myCommand.Parameters.AddWithValue("@Acordionimg_acordion", img.img_acordion);
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
                    catch(SqlException ex)
                    {
                        return new JsonResult(ex);
                    }
                    
                }
                    
            }
            return new JsonResult("Imagen actualizada correctamente");
        }

        //Eliminar un plato de ensalada
        [HttpDelete]
        public JsonResult Delete(Acordion ac)
        {
            string query = @"
                        delete from acordion 
                        where idacordion = @Idacordion;
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TestAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@Idacordion", ac.idacordion);

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