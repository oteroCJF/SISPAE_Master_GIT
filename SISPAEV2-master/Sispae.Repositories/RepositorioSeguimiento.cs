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
    public class RepositorioSeguimiento : IRepositorioSeguimiento
    {
        private readonly string _connectionString;

        public RepositorioSeguimiento(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DatabaseConnection"); ;
        }

        public async Task<List<Seguimiento>> getSeguimientoByIntegracion(int integracion)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getSeguimientoByIntegracion", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@integracion", integracion));
                        var response = new List<Seguimiento>();
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

        public async Task<Seguimiento> getSeguimientoById(int seguimiento)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getSeguimientoById", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@seguimiento", seguimiento));
                        var response = new Seguimiento();
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

        public async Task<int> insertaSeguimiento(Seguimiento seguimiento)
        {
            int id = -1;
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_insertaSeguimiento", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.BigInt)).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(new SqlParameter("@integracion", seguimiento.IntegracionId));
                        cmd.Parameters.Add(new SqlParameter("@usuario", seguimiento.UsuarioId));
                        cmd.Parameters.Add(new SqlParameter("@devolver", seguimiento.Devolver));
                        if (seguimiento.PrestadorId != 0)
                            cmd.Parameters.Add(new SqlParameter("@prestador", seguimiento.PrestadorId));
                        if (!seguimiento.Tipo.Equals(""))
                            cmd.Parameters.Add(new SqlParameter("@tipo", seguimiento.Tipo));
                        if(seguimiento.Anio != 0)
                            cmd.Parameters.Add(new SqlParameter("@anio", seguimiento.Anio));
                        if(!seguimiento.Mes.Equals(""))
                            cmd.Parameters.Add(new SqlParameter("@mes", seguimiento.Mes));
                        if(!seguimiento.Contrato.Equals(""))
                            cmd.Parameters.Add(new SqlParameter("@contrato", seguimiento.Contrato));
                        if(!seguimiento.TipoAdjudicacion.Equals(""))
                            cmd.Parameters.Add(new SqlParameter("@tipoAdj", seguimiento.TipoAdjudicacion));
                        if(!seguimiento.Apartado.Equals(""))
                            cmd.Parameters.Add(new SqlParameter("@apartado", seguimiento.Apartado));
                        if(!seguimiento.Estatus.Equals(""))
                            cmd.Parameters.Add(new SqlParameter("@estatus", seguimiento.Estatus));
                        if (seguimiento.MontoPropio != 0)
                            cmd.Parameters.Add(new SqlParameter("@montoPropio", seguimiento.MontoPropio));
                        if (seguimiento.Monto != 0)
                            cmd.Parameters.Add(new SqlParameter("@monto", seguimiento.Monto));
                        if (!seguimiento.Observaciones.Equals(""))
                            cmd.Parameters.Add(new SqlParameter("@observaciones", seguimiento.Observaciones));

                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        id = Convert.ToInt32(cmd.Parameters["@id"].Value);
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

        public async Task<int> actualizaSeguimiento(Seguimiento seguimiento)
        {
            int id = -1;
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_actualizaSeguimiento", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@id", seguimiento.Id));
                        cmd.Parameters.Add(new SqlParameter("@integracion", seguimiento.IntegracionId));
                        cmd.Parameters.Add(new SqlParameter("@usuario", seguimiento.UsuarioId));
                        cmd.Parameters.Add(new SqlParameter("@devolver", seguimiento.Devolver));
                        if (seguimiento.PrestadorId != 0)
                            cmd.Parameters.Add(new SqlParameter("@prestador", seguimiento.PrestadorId));
                        if (!seguimiento.Tipo.Equals(""))
                            cmd.Parameters.Add(new SqlParameter("@tipo", seguimiento.Tipo));
                        if (seguimiento.Anio != 0)
                            cmd.Parameters.Add(new SqlParameter("@anio", seguimiento.Anio));
                        if (!seguimiento.Mes.Equals(""))
                            cmd.Parameters.Add(new SqlParameter("@mes", seguimiento.Mes));
                        if (!seguimiento.Contrato.Equals(""))
                            cmd.Parameters.Add(new SqlParameter("@contrato", seguimiento.Contrato));
                        if (!seguimiento.TipoAdjudicacion.Equals(""))
                            cmd.Parameters.Add(new SqlParameter("@tipoAdj", seguimiento.TipoAdjudicacion));
                        if (!seguimiento.Apartado.Equals(""))
                            cmd.Parameters.Add(new SqlParameter("@apartado", seguimiento.Apartado));
                        if (seguimiento.Estatus != null && !seguimiento.Estatus.Equals(""))
                            cmd.Parameters.Add(new SqlParameter("@estatus", seguimiento.Estatus));
                        if (seguimiento.MontoPropio != 0)
                            cmd.Parameters.Add(new SqlParameter("@montoPropio", seguimiento.MontoPropio));
                        if (seguimiento.Monto != 0)
                            cmd.Parameters.Add(new SqlParameter("@monto", seguimiento.Monto));
                        if (!seguimiento.Observaciones.Equals(""))
                            cmd.Parameters.Add(new SqlParameter("@observaciones", seguimiento.Observaciones));

                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        id = await cmd.ExecuteNonQueryAsync();
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

        public async Task<int> eliminaSeguimiento(int id)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_eliminaSeguimiento", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@id", id));

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

        public async Task<int> enviaSeguimiento(Seguimiento seguimiento)
        {
            int id = -1;
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_enviaSeguimiento", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@id", seguimiento.Id));
                        cmd.Parameters.Add(new SqlParameter("@integracion", seguimiento.IntegracionId));
                        cmd.Parameters.Add(new SqlParameter("@usuario", seguimiento.UsuarioId));
                        cmd.Parameters.Add(new SqlParameter("@estatus", seguimiento.Estatus));

                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        id = await cmd.ExecuteNonQueryAsync();
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

        public async Task<int> autorizaRechazaSeguimiento(Seguimiento seguimiento)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_autorizarRechazarSeguimiento", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@seguimiento", seguimiento.Id));
                        cmd.Parameters.Add(new SqlParameter("@estatus", seguimiento.Estatus));
                        cmd.Parameters.Add(new SqlParameter("@integracion", seguimiento.IntegracionId));

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

        private Seguimiento MapToValue(SqlDataReader reader)
        {
            return new Seguimiento {
                Id = reader["Id"] != DBNull.Value ? (int)reader["Id"] : 0,
                IntegracionId = reader["IntegracionId"] != DBNull.Value ? (int)reader["IntegracionId"] : 0,
                PrestadorId = reader["PrestadorId"] != DBNull.Value ? (int)reader["PrestadorId"] : 0,
                UsuarioId = reader["UsuarioId"] != DBNull.Value ? (int)reader["UsuarioId"] : 0,
                Tipo = reader["Tipo"] != DBNull.Value ? reader["Tipo"].ToString() : "",
                Anio = reader["Anio"] != DBNull.Value ? (int)reader["Anio"] : 0,
                Mes = reader["Mes"] != DBNull.Value ? reader["Mes"].ToString() : "",
                Contrato = reader["Contrato"] != DBNull.Value ? reader["Contrato"].ToString() : "",
                TipoAdjudicacion = reader["TipoAdjudicacion"] != DBNull.Value ? reader["TipoAdjudicacion"].ToString() : "",
                Apartado = reader["Apartado"] != DBNull.Value ? reader["Apartado"].ToString() : "",
                Estatus = reader["Estatus"] != DBNull.Value ? reader["Estatus"].ToString() : "",
                MontoPropio = reader["MontoPropio"] != DBNull.Value ? (decimal)reader["MontoPropio"] : 0,
                Monto = reader["Monto"] != DBNull.Value ? (decimal)reader["Monto"] : 0,
                Pendiente = reader["Monto"] != DBNull.Value ? (decimal)reader["Monto"] : 0,
                ImporteEstimado = reader["ImporteEstimado"] != DBNull.Value ? (decimal)reader["ImporteEstimado"] : 0,
                RecursosDisponibles = reader["RecursosDisponibles"] != DBNull.Value ? (decimal)reader["RecursosDisponibles"] : 0,
                Devolver = reader["Devolver"] != DBNull.Value ? (bool)reader["Devolver"] : false,
                Observaciones = reader["Observaciones"] != DBNull.Value ? reader["Observaciones"].ToString() : "",
                FechaCreacion = reader["FechaCreacion"] != DBNull.Value ? Convert.ToDateTime(reader["FechaCreacion"]) : Convert.ToDateTime("2001-01-01T00:00:00"),
                FechaActualizacion = reader["FechaActualizacion"] != DBNull.Value ? Convert.ToDateTime(reader["FechaActualizacion"]) :
                                    reader["FechaCreacion"] != DBNull.Value ? Convert.ToDateTime(reader["FechaCreacion"]) : Convert.ToDateTime("2001-01-01T00:00:00")
            };
        }

    }
}
