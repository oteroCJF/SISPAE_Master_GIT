using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Sispae.Entities.MEntregables;
using Sispae.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Repositories
{
    public class RepositorioEntregables : IRepositorioEntregables
    {
        private readonly string _connectionString;
        public RepositorioEntregables(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DatabaseConnection");
        }

        public async Task<List<Entregables>> getEntregables(int seguimiento)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getEntregablesBySeguimiento", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@seguimiento", seguimiento));
                        var response = new List<Entregables>();
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

        public async Task<int> insertaEntregable(Entregables entregable)
        {
            DateTime date = DateTime.Now;
            string date_str = date.ToString("yyyyMMddHHmmss");
            int id = 0;
            string saveFile = "";

            if (entregable.Archivo != null)
            {
                saveFile = await guardaArchivo(entregable.Archivo, entregable.SeguimientoId.ToString(), date_str);
            }
            try
            {
                if (saveFile.Equals("Ok") || entregable.Archivo == null)
                {
                    using (SqlConnection sql = new SqlConnection(_connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_insertaEntregable", sql))
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@id", entregable.Id)).Direction = ParameterDirection.Output;
                            cmd.Parameters.Add(new SqlParameter("@seguimiento", entregable.SeguimientoId));
                            cmd.Parameters.Add(new SqlParameter("@usuario", entregable.UsuarioId));
                            cmd.Parameters.Add(new SqlParameter("@tipo", entregable.Tipo));
                            if (entregable.Archivo != null)
                            {
                                cmd.Parameters.Add(new SqlParameter("@archivo", (date_str + "_" + entregable.Archivo.FileName)));
                            }
                            cmd.Parameters.Add(new SqlParameter("@observaciones", entregable.Observaciones));

                            await sql.OpenAsync();
                            await cmd.ExecuteNonQueryAsync();
                            id = Convert.ToInt32(cmd.Parameters["@id"].Value);
                        }
                    }
                }
                return id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return 0;
            }
        }

        public async Task<int> actualizaEntregable(Entregables entregable)
        {
            DateTime date = DateTime.Now;
            string date_str = date.ToString("yyyyMMddHHmmss");
            int id = 0;
            string saveFile = "";

            if (entregable.Id != 0 && entregable.Archivo != null)
            {
                int isDeleted = await eliminaEntregable(entregable);
            }

            if (entregable.Archivo != null)
            {
                saveFile = await guardaArchivo(entregable.Archivo, entregable.SeguimientoId.ToString(), date_str);
            }
            try
            {
                if (saveFile.Equals("Ok") || entregable.Archivo == null)
                {
                    using (SqlConnection sql = new SqlConnection(_connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_actualizaEntregable", sql))
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@id", entregable.Id));
                            cmd.Parameters.Add(new SqlParameter("@seguimiento", entregable.SeguimientoId));
                            cmd.Parameters.Add(new SqlParameter("@usuario", entregable.UsuarioId));
                            cmd.Parameters.Add(new SqlParameter("@tipo", entregable.Tipo));
                            if (entregable.Archivo != null)
                            {
                                cmd.Parameters.Add(new SqlParameter("@archivo", (date_str + "_" + entregable.Archivo.FileName)));
                            }
                            cmd.Parameters.Add(new SqlParameter("@observaciones", entregable.Observaciones));

                            await sql.OpenAsync();
                            id = await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }
                return id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return 0;
            }
        }

        public async Task<string> guardaArchivo(IFormFile archivo, string seguimiento, string date)
        {
            long size = archivo.Length;
            string folderCedula = seguimiento;

            string newPath = Directory.GetCurrentDirectory() + "\\Entregables\\" + folderCedula;
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            using (var stream = new FileStream(newPath + "\\" + (date + "_" + archivo.FileName), FileMode.Create))
            {
                try
                {
                    await (archivo).CopyToAsync(stream);
                    return "Ok";
                }
                catch (Exception ex)
                {
                    return ex.Message.ToString();
                }
            }
        }

        public async Task<int> eliminaEntregables(Entregables entregable)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_eliminaEntregablesSeguimiento", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@id", entregable.Id));
                        cmd.Parameters.Add(new SqlParameter("@archivo", SqlDbType.VarChar, 500)).Direction = ParameterDirection.Output;
                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        string archivo = (cmd.Parameters["@archivo"].Value).ToString();
                        if (archivo != null && !archivo.Equals(""))
                        {
                            string newPath = Directory.GetCurrentDirectory() + "\\Entregables\\" + entregable.SeguimientoId.ToString() + "\\" + archivo;
                            File.Delete(newPath);
                        }

                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return 0;
            }
        }
        
        public async Task<int> eliminaEntregable(Entregables entregable)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_eliminaEntregableSeguimiento", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@id", entregable.Id));
                        cmd.Parameters.Add(new SqlParameter("@archivo", SqlDbType.VarChar, 500)).Direction = ParameterDirection.Output;
                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        string archivo = (cmd.Parameters["@archivo"].Value).ToString();
                        if (archivo != null && !archivo.Equals(""))
                        {
                            string newPath = Directory.GetCurrentDirectory() + "\\Entregables\\" + entregable.SeguimientoId.ToString() + "\\" + archivo;
                            File.Delete(newPath);
                        }

                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return 0;
            }
        }

        private Entregables MapToValue(SqlDataReader reader)
        {
            return new Entregables
            {
                Id = (int)reader["Id"],
                SeguimientoId = reader["SeguimientoId"] != DBNull.Value ? (int)reader["SeguimientoId"]:0,
                Tipo = reader["Tipo"] != DBNull.Value ? reader["Tipo"].ToString():"",
                NombreArchivo = reader["Archivo"].ToString(),
                Observaciones = reader["Observaciones"] != DBNull.Value ? reader["Observaciones"].ToString() : "",
                FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"].ToString()),
                FechaActualizacion = reader["FechaActualizacion"] != DBNull.Value ? Convert.ToDateTime(reader["FechaActualizacion"].ToString()) : Convert.ToDateTime("01/01/1990"),
            };
        }
    }
}
