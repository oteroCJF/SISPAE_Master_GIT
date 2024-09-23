using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Sispae.Entities.Vistas;
using Sispae.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Repositories
{
    public class RepositorioAhorros : IRepositorioAhorros
    {
        private readonly string _connectionString;

        public RepositorioAhorros(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DatabaseConnection"); ;
        }

        public async Task<VProyectos> GetProyectoById(int id)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getProyectoAhorrosById", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@id", id));
                        var response = new VProyectos();
                        await sql.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response = MapToValue(reader);
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

        private VProyectos MapToValue(SqlDataReader reader)
        {
            return new VProyectos
            {
                Id = reader["Id"] != DBNull.Value ? (int)reader["Id"] : 0,
                ProyectoId = reader["ProyectoId"] != DBNull.Value ? (int)reader["ProyectoId"] : 0,
                Proyecto = reader["Proyecto"] != DBNull.Value ? reader["Proyecto"].ToString() : "",
                UEGId = reader["UEGId"] != DBNull.Value ? (int)reader["UEGId"] : 0,
                //NumeroUEG = reader["NumeroUEG"] != DBNull.Value ? (int)reader["NumeroUEG"] : 0,
                Evento = reader["Evento"] != DBNull.Value ? (bool)reader["Evento"] : false,
                UsuarioId = reader["UsuarioId"] != DBNull.Value ? (int)reader["UsuarioId"] : 0,
                Ejercicio = reader["Ejercicio"] != DBNull.Value ? (int)reader["Ejercicio"] : 0,
                Capitulo = reader["Capitulo"] != DBNull.Value ? (int)reader["Capitulo"] : 0,
                ClaveProyecto = reader["ClaveProyecto"] != DBNull.Value ? (int)reader["ClaveProyecto"] : 0,
                Tipo = reader["Tipo"] != DBNull.Value ? reader["Tipo"].ToString() : "",
                Icono = reader["Icono"] != DBNull.Value ? reader["Icono"].ToString() : "",
                Fondo = reader["Fondo"] != DBNull.Value ? reader["Fondo"].ToString() : "",
                Estatus = reader["Estatus"] != DBNull.Value ? reader["Estatus"].ToString() : "",
                RecursosDisponibles = reader["RecursosDisponibles"] != DBNull.Value ? (decimal)reader["RecursosDisponibles"] : 0,
                Importe = reader["Importe"] != DBNull.Value ? (decimal)reader["Importe"] : 0,
                TotalSeguimientos = reader["TotalSeguimientos"] != DBNull.Value ? (int)reader["TotalSeguimientos"] : 0,
                FechaCreacion = reader["FechaCreacion"] != DBNull.Value ? Convert.ToDateTime(reader["FechaCreacion"]) : Convert.ToDateTime("2001-01-01T00:00:00"),
                FechaActualizacion = reader["FechaActualizacion"] != DBNull.Value ? Convert.ToDateTime(reader["FechaActualizacion"]) :
                                    reader["FechaCreacion"] != DBNull.Value ? Convert.ToDateTime(reader["FechaCreacion"]) : Convert.ToDateTime("2001-01-01T00:00:00")
            };
        }
    }
}
