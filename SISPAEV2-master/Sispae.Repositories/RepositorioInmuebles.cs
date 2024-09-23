using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Sispae.Entities.MInmuebles;
using Sispae.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Repositories
{
    public class RepositorioInmuebles : IRepositorioInmuebles
    {
        private readonly string _connectionString;

        public RepositorioInmuebles(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DatabaseConnection"); ;
        }

        //metodo que trae todas las administraciones de la tabla Inmuebles
        public async Task<List<Inmueble>> getAdministraciones()
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getAdministraciones", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        var response = new List<Inmueble>();
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

        //obtenemos una administración por ID
        public async Task<Inmueble> inmuebleById(int id)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getInmuebleById", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@id", id));
                        Inmueble response = null;
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
            }catch(Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }

        //Insertamos nuevas Administraciones
        public async Task<int> insertAdmin(Inmueble inmueble)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_insertaAdministracion", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@administracionId", inmueble.AdministracionId));
                        cmd.Parameters.Add(new SqlParameter("@clave", inmueble.Clave));
                        cmd.Parameters.Add(new SqlParameter("@nombre", inmueble.Nombre));
                        cmd.Parameters.Add(new SqlParameter("@direccion", inmueble.Direccion));
                        await sql.OpenAsync();
                        int i = await cmd.ExecuteNonQueryAsync();
                        return i == 1 ? 1 : -1;
                    }
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }

        //Actualizamos nuevas Administraciones
        public async Task<int> updateAdmin(Inmueble inmueble)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_updateAdministracion", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@id", inmueble.Id));
                        cmd.Parameters.Add(new SqlParameter("@administracionId", inmueble.AdministracionId));
                        cmd.Parameters.Add(new SqlParameter("@clave", inmueble.Clave));
                        cmd.Parameters.Add(new SqlParameter("@nombre", inmueble.Nombre));
                        cmd.Parameters.Add(new SqlParameter("@direccion", inmueble.Direccion));
                        await sql.OpenAsync();
                        int i = await cmd.ExecuteNonQueryAsync();
                        return i == 1 ? 1 : -1;
                    }
                }
            }catch (Exception)
            {
                return -1;   
            }
        }

        //Actualizamos nuevas Administraciones
        public async Task<int> deleteAdmin(int id)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_eliminarAdministracion", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@id", id));
                        await sql.OpenAsync();
                        int i = await cmd.ExecuteNonQueryAsync();
                        return i == 1 ? 1 : -1;
                    }
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }

        //listado de estados de la República Mexicana
        public async Task<List<Inmueble>> getEstadosRM()
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getEstadosRM", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        var response = new List<Inmueble>();
                        await sql.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response.Add(MapToValueRM(reader));
                            }
                        }

                        return response;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        //obtenemos los inmuebles en base a la administración
        public async Task<List<Inmueble>> getInmueblesByAdmin(int id)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getInmueblesByAdmin", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@id", id));
                        var response = new List<Inmueble>();
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
            catch (Exception)
            {
                return null;
            }
        }
        
        //obtenemos los inmuebles a para los filtros de búsqueda en la cédula
        public async Task<List<Inmueble>> getFiltrosInmuebles(int user, int servicio)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getInmueblesFiltros", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@usuario", user));
                        cmd.Parameters.Add(new SqlParameter("@servicio", servicio));
                        var response = new List<Inmueble>();
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
            catch (Exception)
            {
                return null;
            }
        }

        private Inmueble MapToValue(SqlDataReader reader)
        {
            return new Inmueble()
            {
                Id = reader["Id"] != DBNull.Value ? (int)reader["Id"]:0,
                AdministracionId = reader["AdministracionId"] != DBNull.Value ? (int)reader["AdministracionId"]: 0,
                Clave = reader["Clave"] != DBNull.Value ? (int)reader["Clave"]:0,
                Nombre = reader["Nombre"] != DBNull.Value ? reader["Nombre"].ToString():"",
                Direccion = reader["Direccion"] != DBNull.Value ? reader["Direccion"].ToString():""
            };
        }
        private Inmueble MapToValueRM(SqlDataReader reader)
        {
            return new Inmueble()
            {
                Estado = reader["Estado"].ToString(),
            };
        }

    }
}