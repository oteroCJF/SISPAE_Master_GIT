using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Sispae.Entities.MProyectos;
using Sispae.Entities.MUEGS;
using Sispae.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Repositories
{
    public class RepositorioUnidadesEjecutoras: IRepositorioUnidadesEjecutoras
    {
        private readonly string _connectionString;
        public RepositorioUnidadesEjecutoras(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DatabaseConnection"); ;
        }

        /**************************** CRUD UEGS ****************************/
        public async Task<List<UEG>> GetUnidadesEjecutoras()
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getUnidadesEjecutoras", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        var response = new List<UEG>();
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
        public async Task<UEG> GetUnidadEjecutoraById(int id)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getUnidadEjecutoraById", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@id", id));
                        var response = new UEG();
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
        public async Task<int> UpdateUnidadEjecutora(UEG ueg)
        {
            int i = 0;
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_actualizaUEG", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@id", ueg.Id));
                        cmd.Parameters.Add(new SqlParameter("@ueg", ueg.NumeroUEG));
                        cmd.Parameters.Add(new SqlParameter("@nombre", ueg.Nombre));
                        cmd.Parameters.Add(new SqlParameter("@descripcion", ueg.Descripcion));
                        await sql.OpenAsync();

                        i = await cmd.ExecuteNonQueryAsync();
                        
                        return i == 1 ? i: -1;
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return -1;
            }
        }
        public async Task<int> DeleteUnidadEjecutora(int id)
        {
            int i = 0;
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_eliminaUEG", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@id", id));
                        await sql.OpenAsync();

                        i = await cmd.ExecuteNonQueryAsync();

                        return i == 1 ? i : -1;
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return -1;
            }
        }

        /*************************** Consultas UEGS ************************/
        public async Task<List<UEG>> GetUnidadesEjecutorasByUser(int user)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getUnidadesByUser", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@user", user));
                        var response = new List<UEG>();
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

        private UEG MapToValue(SqlDataReader reader)
        {
            return new UEG
            {
                Id = reader["Id"] != DBNull.Value ? (int)reader["Id"] : 0,
                NumeroUEG = reader["NumeroUEG"] != DBNull.Value ? (int)reader["NumeroUEG"] : 0,
                Orden = reader["Orden"] != DBNull.Value ? (int)reader["Orden"] : 0,
                Nombre = reader["Nombre"] != DBNull.Value ? reader["Nombre"].ToString() : "",
                Descripcion = reader["Descripcion"] != DBNull.Value ? reader["Descripcion"].ToString() : "",
                Fondo  = reader["Fondo"] != DBNull.Value ? reader["Fondo"].ToString() : "",
                FondoHexadecimal = reader["FondoHexadecimal"] != DBNull.Value ? reader["FondoHexadecimal"].ToString() : "",
                Icono = reader["Icono"] != DBNull.Value ? reader["Icono"].ToString() : "",

            };
        }
    }
}
