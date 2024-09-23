using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Sispae.Entities.MProyectos;
using Sispae.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Repositories
{
    public class RepositorioCatalogoProyectos : IRepositorioCatalogoProyectos
    {
        private readonly string _connectionString;

        public RepositorioCatalogoProyectos(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DatabaseConnection"); ;
        }
        public async Task<List<Proyecto>> GetCatalogoProyectosByUEGTipo(int ueg,string tipo)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getCatalogoProyectosByUEGTipo", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ueg", ueg));
                        cmd.Parameters.Add(new SqlParameter("@tipo", tipo));
                        var response = new List<Proyecto>();
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
        public async Task<List<Proyecto>> GetProyectosSinUEG()
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getProyectosSinUEG", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        var response = new List<Proyecto>();
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
        public async Task<List<Proyecto>> GetCatalogoProyectos()
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getCatalogoProyectos", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        var response = new List<Proyecto>();
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
        public async Task<int> InsertaProyecto(Proyecto proyecto)
        {
            int id = 0;
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_insertaProyectoCatalogo", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@id", proyecto.Id)).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(new SqlParameter("@tipo", proyecto.Tipo));
                        cmd.Parameters.Add(new SqlParameter("@proyecto", proyecto.Nombre));
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
        public async Task<int> actualizaProyecto(Proyecto proyecto)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_actualizaProyectoCatalogo", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@id", proyecto.Id));
                        cmd.Parameters.Add(new SqlParameter("@tipo", proyecto.Tipo));
                        cmd.Parameters.Add(new SqlParameter("@nombre", proyecto.Nombre));
                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return -1;
            }
        }
        public async Task<Proyecto> ActualizarProyectoById(int proyecto)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getProyectoByActualizar", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@proyecto", proyecto));
                        var response = new Proyecto();
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
        private Proyecto MapToValue(SqlDataReader reader)
        {
            return new Proyecto
            {
                Id = reader["Id"] != DBNull.Value ? (int)reader["Id"] : 0,
                Tipo = reader["Tipo"] != DBNull.Value ? reader["Tipo"].ToString() : "",
                Nombre = reader["Nombre"] != DBNull.Value ? reader["Nombre"].ToString() : "",
            };
        }

    }
}
