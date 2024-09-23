using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Sispae.Entities.MHistorial;
using Sispae.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Repositories
{
    public class RepositorioHistoriales : IRepositorioHistoriales
    {
        private readonly string _connectionString;

        public RepositorioHistoriales(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DatabaseConnection"); ;
        }

        public async Task<List<HistorialProyectos>> getHistorialByProyecto(int proyecto)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getHistorialByProyetco", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@proyecto", proyecto));
                        var response = new List<HistorialProyectos>();
                        await sql.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response.Add(MapToValue(reader));
                            }
                        }

                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }

        public async Task<int> capturaHistorial(HistorialProyectos historial)
        {
            int id = 0;
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_insertaHistorialProyecto", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.BigInt)).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(new SqlParameter("@integracion", historial.IntegracionId));
                        cmd.Parameters.Add(new SqlParameter("@usuario", historial.UsuarioId));
                        cmd.Parameters.Add(new SqlParameter("@tipo", historial.Tipo));
                        cmd.Parameters.Add(new SqlParameter("@estatus", historial.Estatus));
                        cmd.Parameters.Add(new SqlParameter("@comentarios", historial.Comentarios));

                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        id = Convert.ToInt32(cmd.Parameters["@id"].Value);

                        return id;
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return -1;
            }
        }

        private HistorialProyectos MapToValue(SqlDataReader reader)
        {
            return new HistorialProyectos
            {
                Id = reader["Id"] != DBNull.Value ? (int)reader["Id"]:0,
                IntegracionId = reader["IntegracionId"] != DBNull.Value ? (int)reader["IntegracionId"] :0,
                UsuarioId = reader["UsuarioId"] != DBNull.Value ? (int)reader["UsuarioId"]:0,
                Tipo = reader["Tipo"] != DBNull.Value ? reader["Tipo"].ToString():"",
                Estatus = reader["Estatus"] != DBNull.Value ? reader["Estatus"].ToString():"",
                Comentarios = reader["Comentarios"] != DBNull.Value ? reader["Comentarios"].ToString():"",
                FechaCreacion = reader["FechaCreacion"] != DBNull.Value ? Convert.ToDateTime(reader["FechaCreacion"]) : Convert.ToDateTime("2001-01-01T00:00:00")
            };
        }
    }
}
