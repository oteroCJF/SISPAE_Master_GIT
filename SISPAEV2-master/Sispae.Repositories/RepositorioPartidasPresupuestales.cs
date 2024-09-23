using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Sispae.Entities.MPartidasPresupuestales;
using Sispae.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Repositories
{
    public class RepositorioPartidasPresupuestales: IRepositorioPartidasPresupuestales
    {
        private readonly string _connectionString;
        public RepositorioPartidasPresupuestales(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DatabaseConnection"); ;
        }

        public async Task<List<PartidasPresupuestales>> GetPartidasPresupuestales(int integracion)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getPartidasPresupuestales", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@integracion", integracion));
                        var response = new List<PartidasPresupuestales>();
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
        public async Task<int> insertaPartidasByProyecto(PartidasProyecto partidasProyecto)
        {
            int i = -1;
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_insertaPartidaByProyecto", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@partida", partidasProyecto.PartidaId));
                        cmd.Parameters.Add(new SqlParameter("@integracion", partidasProyecto.IntegracionId));
                        cmd.Parameters.Add(new SqlParameter("@monto", partidasProyecto.Monto));
                        await sql.OpenAsync();

                        i = await cmd.ExecuteNonQueryAsync();
                        return i;
                    }
                }
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                return -1;
            }
        }        
        public async Task<int> DeletePartidasProyecto(int integracion, int partida)
        {
            int i = -1;
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_eliminaPartidasProyecto", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@partida", partida));
                        cmd.Parameters.Add(new SqlParameter("@integracion", integracion));
                        await sql.OpenAsync();

                        i = await cmd.ExecuteNonQueryAsync();
                        return i;
                    }
                }
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                return -1;
            }
        }
        public async Task<List<PartidasPresupuestales>> GetPartidasProyecto(int integracion)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getPartidasProyecto", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@integracion", integracion));
                        var response = new List<PartidasPresupuestales>();
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
        private PartidasPresupuestales MapToValue(SqlDataReader reader)
        {
            return new PartidasPresupuestales { 
                Id = reader["Id"] != DBNull.Value ? (int)reader["Id"]:0,
                Nombre = reader["Nombre"] != DBNull.Value ? reader["Nombre"].ToString():"",
                Monto = reader["Monto"] != DBNull.Value ? Convert.ToDecimal(reader["Monto"]): 0
            };
        }
    }
}
