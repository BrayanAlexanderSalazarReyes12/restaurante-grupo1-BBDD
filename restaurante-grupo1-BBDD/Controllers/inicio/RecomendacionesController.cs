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
    public class RecomendacionesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public RecomendacionesController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }


        // CONSULTAR RECOMENDACIONES DEL CHEFT
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select *
                        from
                        recomendaciones
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


        


        //Acutalizar img_acordion
        [HttpPut]
        public JsonResult Put(Recomendaciones rc)
        {
            string query = @"
                        update recomendaciones set 
                        TituloRecom = @TituloRecom,
                        TextoRecom = @TextoRecom,
                        ImgRecom = @ImgRecom
                        where IdRecom = @IdRecom
                        
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
                        myCommand.Parameters.AddWithValue("@TituloRecom", rc.TituloRecom);
                        myCommand.Parameters.AddWithValue("@TextoRecom", rc.TextoRecom);
                        myCommand.Parameters.AddWithValue("@ImgRecom", rc.ImgRecom);
                        myCommand.Parameters.AddWithValue("@IdRecom", rc.IdRecom);
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

    }
}
