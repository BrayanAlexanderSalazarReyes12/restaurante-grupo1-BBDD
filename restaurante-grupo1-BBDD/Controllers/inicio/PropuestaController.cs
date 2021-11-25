using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using Practica.models;
using restaurante_grupo1_BBDD.models.Inicio;
using System.Data.SqlClient;
using System;

namespace restaurante_grupo1_BBDD.Controllers.inicio
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropuestaController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public PropuestaController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }




        // CONSULTAR DATOS DE PROPUESTA
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select *
                        from
                        propuesta
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


        //Acutalizar propuesta
        [HttpPut]
        public JsonResult Put(Propuesta pr)
        {
            string query = @"
                        update propuesta set 
                        ImgPropuesta = @Img,
                        TextoPropuesta = @Texto,
                        TituloPropuesta = @Titulo
                        where IdPropuesta = @Id";
     
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TestAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    try
                    {
                        myCommand.Parameters.AddWithValue("@Img", pr.ImgPropuesta);
                        myCommand.Parameters.AddWithValue("@Texto", pr.TextoPropuesta);
                        myCommand.Parameters.AddWithValue("@Titulo", pr.TituloPropuesta);
                        myCommand.Parameters.AddWithValue("@Id", pr.IdPropuesta);
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
    }
}
