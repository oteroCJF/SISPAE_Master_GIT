using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Sispae.Entities.MRecursosProyecto;
using Sispae.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Repositories
{
    public class RepositorioRecursosProyecto : IRepositorioRecursosProyecto
    {
        private readonly string _connectionString;

        public RepositorioRecursosProyecto(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DatabaseConnection"); ;
        }

        public async Task<int> eliminaRecursoProyecto(int integracion, int mes, decimal monto)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_deleteRecursosProyecto", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@integracion", integracion));
                        cmd.Parameters.Add(new SqlParameter("@mesId", mes));
                        cmd.Parameters.Add(new SqlParameter("@monto", monto));

                        await sql.OpenAsync();
                        int i = await cmd.ExecuteNonQueryAsync();
                        return i;
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return -1;
            }
        }

        public async Task<List<VRecursosProyecto>> GetRecursosProyecto(int integracion)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getRecursosProyecto", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@integracion", integracion));
                        var response = new List<VRecursosProyecto>();
                        
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

        public async Task<int> insertaRecursosProyecto(RecursosProyectos recurso)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_insertaRecursosProyecto", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@integracion", recurso.IntegracionId));
                        cmd.Parameters.Add(new SqlParameter("@partida", recurso.PartidaId));
                        cmd.Parameters.Add(new SqlParameter("@mesId", recurso.MesId));
                        cmd.Parameters.Add(new SqlParameter("@monto", recurso.Monto));                       

                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                return 1;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return -1;
            }
        }

        private VRecursosProyecto MapToValue(SqlDataReader reader)
        {
            return new VRecursosProyecto
            {
                IntegracionId = reader["IntegracionId"] != DBNull.Value ? (int)reader["IntegracionId"] : 0,
                MesId = reader["MesId"] != DBNull.Value ? (int)reader["MesId"] : 0,
                PartidaId = reader["PartidaId"] != DBNull.Value ? (int)reader["PartidaId"] : 0,
                Mes = reader["Mes"] != DBNull.Value ? reader["Mes"].ToString() : "",
                Partida = reader["Partida"] != DBNull.Value ? reader["Partida"].ToString() : "",
                Monto = reader["Monto"] != DBNull.Value ? (decimal)reader["Monto"] : 0,
            };
        }
    }
}
