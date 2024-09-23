using Sispae.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Sispae.Entities.MLogin;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using System.Data;

namespace Sispae.Repositories
{
    public class RepositorioAreas: IRepositorioAreas
    {
        private readonly string _connectionString;

        public RepositorioAreas(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DatabaseConnection"); ;
        }

        public async Task<Areas> getAreasById(int area)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getAreaById", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@id", area));
                        var response = new Areas();
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

        public async Task<int> insertaArea(Areas area)
        {
            int id = 0;
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_insertaArea", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.BigInt)).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(new SqlParameter("@claveArea", area.cveArea));
                        cmd.Parameters.Add(new SqlParameter("@claveAdsc", area.cve_adscripcion));
                        cmd.Parameters.Add(new SqlParameter("@nombre", area.nom_area));
                        cmd.Parameters.Add(new SqlParameter("@estado", area.nom_edo));

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

        private Areas MapToValue(SqlDataReader reader)
        {
            return new Areas 
            {
                Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"].ToString()):0,
                cveArea = reader["ClaveArea"] != DBNull.Value ? Convert.ToInt32(reader["ClaveArea"].ToString()):0,
                cve_adscripcion = reader["ClaveAdscripcion"] != DBNull.Value ? reader["ClaveAdscripcion"].ToString():"",
                ClaveInmueble = reader["ClaveInmueble"] != DBNull.Value ? Convert.ToInt32(reader["ClaveInmueble"].ToString()):0,
                nom_area = reader["Nombre"] != DBNull.Value ? reader["Nombre"].ToString():"",
                nom_edo = reader["Estado"] != DBNull.Value ? reader["Estado"].ToString():"",
            };
        }
    }
}
