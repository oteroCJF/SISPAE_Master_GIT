using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Sispae.Entities.MDashboards;
using Sispae.Entities.MLogin;
using Sispae.Entities.Vistas;
using Sispae.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Repositories
{
    public class RepositorioLogin : IRepositorioLogin
    {
        private readonly string _connectionString;

        public RepositorioLogin(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DatabaseConnection"); ;
        }

        public async Task<int> buscaUsuario(string usuario, string password)
        {
            int success = -1;
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_buscaActualizaUsuario", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@usuario", usuario));
                        cmd.Parameters.Add(new SqlParameter("@password", password));
                        var response = new List<Dashboard>();

                        await sql.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                success = 1;
                                break;
                            }
                        }
                    }
                    return success;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return success;
            }
        }

        public async Task<DatosUsuario> login(string usuario, string password)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_validaUsuario", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@usuario", usuario));
                        cmd.Parameters.Add(new SqlParameter("@password", password));
                        var response = new DatosUsuario();
                        await sql.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response = MapToValueDU(reader);
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

        public async Task<List<VModulosUsuario>> getModulosByUser(int user)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getModulosByUser", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@usuario", user));
                        var response = new List<VModulosUsuario>();
                        await sql.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response.Add(MapToValueVModulos(reader));
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
        public async Task<List<ResponsablesDAS>> GetResponsablesDAS()
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getResonsablesDAS", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        var response = new List<ResponsablesDAS>();
                        await sql.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response.Add(MapToValueResponsables(reader));
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
        private DatosUsuario MapToValueDU(SqlDataReader reader)
        {
            return new DatosUsuario
            {
                Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]):0,
                Expediente = reader["Expediente"] != DBNull.Value ? Convert.ToInt32(reader["Expediente"]):0,
                Usuario = reader["Usuario"] != DBNull.Value ? reader["Usuario"].ToString():"",
                Empleado = reader["Empleado"] != DBNull.Value ? reader["Empleado"].ToString():"",
                CorreoElectronico = reader["CorreoElectronico"] != DBNull.Value ? reader["CorreoElectronico"].ToString():"",
                Puesto = reader["Puesto"] != DBNull.Value ? reader["Puesto"].ToString():"",
                Area = reader["Area"] != DBNull.Value ? reader["Area"].ToString():"",
                ClaveInmueble = reader["ClaveInmueble"] != DBNull.Value ? Convert.ToInt32(reader["ClaveInmueble"]):0,
                Estatus = reader["Estatus"] != DBNull.Value ? reader["Estatus"].ToString(): "",
                Perfiles = reader["Perfiles"] != DBNull.Value ? reader["Perfiles"].ToString():"",
            };
        }
        private VModulosUsuario MapToValueVModulos(SqlDataReader reader)
        {
            return new VModulosUsuario
            {
                Id = (int)reader["Id"],
                PerfilId = (int)reader["PerfilId"],
                Empleado = reader["Empleado"].ToString(),
                ModuloId = (int)reader["ModuloId"],
                Modulo = reader["Modulo"].ToString(),
                Icono = reader["Icono"] != DBNull.Value ? reader["Icono"].ToString() : "",
                URL = reader["URL"].ToString(),
            };
        }
        private ResponsablesDAS MapToValueResponsables(SqlDataReader reader)
        {
            return new ResponsablesDAS()
            {
                Id = (int)reader["Id"],
                Nombre = reader["Nombre"].ToString(),
                Email = reader["Email"].ToString(),
                Extension = (int)reader["Extension"],
                Servicios = reader["Servicios"].ToString(),
                Puesto = reader["Puesto"].ToString(),
                Horario = reader["Horario"].ToString(),
            };
        }
    }
}
