using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Sispae.Entities.MRecursosProyecto;
using Sispae.Entities.MTiposProyecto;
using Sispae.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Repositories
{
    public class RepositorioCTTipoProyecto : IRepositorioCTTipoProyecto
    {
        private readonly string _connectionString;

        public RepositorioCTTipoProyecto(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DatabaseConnection"); ;
        }

        public async Task<List<CTTipoProyecto>> GetCTTiposProyecto()
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getCTTiposProyecto", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        var response = new List<CTTipoProyecto>();
                        
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

        public async Task<CTTipoProyecto> GetCategoriaById(int id)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getCTTiposProyectoById", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@id", id));
                        var response = new CTTipoProyecto();
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

        private CTTipoProyecto MapToValue(SqlDataReader reader)
        {
            return new CTTipoProyecto
            {
                Id = reader["Id"] != DBNull.Value ? (int)reader["Id"] : 0,
                Nombre = reader["Nombre"] != DBNull.Value ? reader["Nombre"].ToString() : ""
            };
        }
    }
}
