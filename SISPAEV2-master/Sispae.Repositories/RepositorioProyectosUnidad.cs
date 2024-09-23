using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Sispae.Entities.MProyectos;
using Sispae.Entities.Vistas;
using Sispae.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Repositories
{
    public class RepositorioProyectosUnidad: IRepositorioProyectosUnidad
    {
        private readonly string _connectionString;

        public RepositorioProyectosUnidad(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DatabaseConnection"); ;
        }
        public async Task<List<VProyectosUEG>> GetProyectosByUEG(int proyecto)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getUEGByProyecto", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@proyecto", proyecto));
                        var response = new List<VProyectosUEG>();
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
        public async Task<int> insertaUEGProyecto(List<ProyectosUeg> unidad)
        {
            int id = -1;
            try
            {
                foreach (var r in unidad)
                {
                    using (SqlConnection sql = new SqlConnection(_connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_insertaProyectoUEG", sql))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@ueg", r.UnidadId));
                            cmd.Parameters.Add(new SqlParameter("@proyecto", r.ProyectoId));
                            cmd.Parameters.Add(new SqlParameter("@ejercicio", r.Ejercicio));
                            await sql.OpenAsync();

                            int i = await cmd.ExecuteNonQueryAsync();
                            if (i <= 0)
                            {
                                return i;
                            }else
                            {
                                id = i;
                            }
                        }
                    }
                }
                return id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return -1;
            }
        }        
        public async Task<int> eliminaUEGProyecto(int proyecto , int unidad, int ejercicio)
        {
            int i = 0;
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_eliminaProyectoUEG", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ueg", unidad));
                        cmd.Parameters.Add(new SqlParameter("@proyecto", proyecto));
                        cmd.Parameters.Add(new SqlParameter("@ejercicio", ejercicio));
                        await sql.OpenAsync();

                        i = await cmd.ExecuteNonQueryAsync();
                        if (i < 0)
                        {
                            return -1;
                        }
                    }
                }
                return i;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return -1;
            }
        }
        private VProyectosUEG MapToValue(SqlDataReader reader)
        {
            return new VProyectosUEG
            {
                UnidadId = reader["UnidadId"] != DBNull.Value ? (int)reader["UnidadId"] : 0,
                NumeroUEG = reader["NumeroUEG"] != DBNull.Value ? (int)reader["NumeroUEG"] : 0,
                Ejercicio = reader["Ejercicio"] != DBNull.Value ? (int)reader["Ejercicio"] : 0,
                Nombre = reader["Descripcion"] != DBNull.Value ? reader["Descripcion"].ToString() : "",
                FechaAsignacion = reader["FechaAsignacion"] != DBNull.Value ? Convert.ToDateTime(reader["FechaAsignacion"].ToString()) : Convert.ToDateTime("01/01/2001")

            };
        }
    }
}
