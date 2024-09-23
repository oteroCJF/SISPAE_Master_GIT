using Microsoft.Data.SqlClient;
using System.Data;
using Sispae.Entities.MPerfiles;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Sispae.Interfaces;

namespace Sispae.Repositories
{
    public class RepositorioOperacionesPerfil: IRepositorioOperacionesPerfil
    {
        private readonly string _connectionString;

        public RepositorioOperacionesPerfil(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DatabaseConnection"); ;
        }

        //insertamos el perfil
        public async Task<int> insertarOperacionesPerfil(OperacionesPerfil operacionesPerfil)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_insertarOperacionesPerfil", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@perfilId", operacionesPerfil.PerfilId));
                        cmd.Parameters.Add(new SqlParameter("@operacionId", operacionesPerfil.OperacionId));
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

        //Obtenemos los permisos por perfil
        public async Task<List<OperacionesPerfil>> getOperacionesByPerfil(int perfil)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getOperacionesByPerfil", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@id", perfil));
                        var response = new List<OperacionesPerfil>();
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

        public async Task<PermisosPerfil> GetPermisoModuloByUser(string permiso, int usuario)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getPermisoModuloByUser", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@usuario", usuario));
                        cmd.Parameters.Add(new SqlParameter("@permiso", permiso));
                        var response = new PermisosPerfil();
                        await sql.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response = MapToValuePermiso(reader);
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

        //eliminamos las operaciones por perfil
        public async Task<int> eliminaOpPerfil(int perfil)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_eliminaOpPerfil", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@id", perfil));

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

        private OperacionesPerfil MapToValue(SqlDataReader reader)
        {
            return new OperacionesPerfil
            {
                PerfilId = (int)reader["PerfilId"],
                OperacionId = (int)reader["OperacionId"]
            };
        }
        private PermisosPerfil MapToValuePermiso(SqlDataReader reader)
        {
            return new PermisosPerfil
            {
                Id = (int)reader["Id"],
                Modulo = reader["Modulo"].ToString(),
                Permiso = reader["Permiso"].ToString()
            };
        }
    }
}
