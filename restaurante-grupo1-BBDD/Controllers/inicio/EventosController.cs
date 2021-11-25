using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using restaurante_grupo1_BBDD.models.Inicio;
using System.Data;
using System.Data.SqlClient;
using Practica.models;

namespace restaurante_grupo1_BBDD.Controllers.inicio
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public EventosController(IConfiguration configuration, IWebHostEnvironment env)
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
                        eventos
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


        //Acutalizar evento
        [HttpPut]
        public JsonResult Put(Evento ev)
        {
            string query = @"
                        update eventos set 
                        ImgEventos = @ImgEvento,
                        TextoEvento = @TextoEvento,
                        TituloEvento = @TituloEvento
                        where IdEventos = @IdEvento
                        
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
                        myCommand.Parameters.AddWithValue("@ImgEvento", ev.ImgEventos);
                        myCommand.Parameters.AddWithValue("@TextoEvento", ev.TextoEvento);
                        myCommand.Parameters.AddWithValue("@TituloEvento", ev.TituloEvento);
                        myCommand.Parameters.AddWithValue("@IdEvento", ev.IdEventos);
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
