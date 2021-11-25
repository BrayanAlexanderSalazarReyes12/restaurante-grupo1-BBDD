using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using restaurante_grupo1_BBDD.models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace restaurante_grupo1_BBDD.Controllers
{
        [Route("api/[controller]")]
        [ApiController]
    public class ReservasController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public ReservasController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        //consultar reservas
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select servicios,fecha,hora,nombre,email,telefono
                        from
                        reservas
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


        //insertar reserva
        [HttpPost]
        public JsonResult Post(Reservas rs)
        {
            try
            {
                string query = @"insert into reservas(servicios,fecha,hora,nombre,email,telefono)
                            values
                            (@servicios,@fecha,@hora,@nombre,@email,@telefono)
                ";
                string sqlDataSource = _configuration.GetConnectionString("TestAppCon");
                using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
                {

                    using (MySqlCommand myCommand = new MySqlCommand())
                    {
                        myCommand.Connection = mycon;
                        myCommand.CommandType = CommandType.Text;
                        myCommand.CommandText = query;
                        myCommand.Parameters.AddWithValue("@servicios", rs.servicios);
                        myCommand.Parameters.AddWithValue("@fecha", rs.fecha);
                        myCommand.Parameters.AddWithValue("@hora", rs.hora);
                        myCommand.Parameters.AddWithValue("@nombre", rs.nombre);
                        myCommand.Parameters.AddWithValue("@telefono", rs.telefono);
                        myCommand.Parameters.AddWithValue("@email", rs.email);
 
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
                    return new JsonResult("Fue Enviado Correctamente");
                }

            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }


        //eliminar una reserva
        [HttpDelete("{email}")]
        public JsonResult Delete(string email)
        {
            string query = @"
                        delete from reservas 
                        where email=@email;
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TestAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@email", email);

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
