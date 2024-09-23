using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Sispae.Entities.MPrestador;
using Sispae.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Repositories
{
    public class RepositorioPrestadorServicios : IRepositorioPrestadorServicios
    {
        private readonly string _connectionString;

        public RepositorioPrestadorServicios(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DatabaseConnection"); ;
        }

        public async Task<List<PrestadorServicios>> GetCatalogoPrestadores()
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getPrestadoresServicios", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        var response = new List<PrestadorServicios>();
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

        public async Task<PrestadorServicios> GetPrestadorServiciosById(int id)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getPrestadorServiciosById", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@id", id));
                        var response = new PrestadorServicios();
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

        private PrestadorServicios MapToValue(SqlDataReader reader)
        {
            return new PrestadorServicios { 
                Id = reader["Id"] != DBNull.Value ? (int) reader["Id"]:0,
                Nombre = reader["Nombre"] != DBNull.Value ? reader["Nombre"].ToString() : "",
            };
        }
    }
}
