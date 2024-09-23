using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Sispae.Entities.MRecursos;
using Sispae.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Repositories
{
    public class RepositorioRecursos : IRepositorioRecursos
    {
        private readonly string _connectionString;

        public RepositorioRecursos(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DatabaseConnection"); ;
        }

        public async Task<List<Recursos>> getRecursosBySeguimiento(int seguimiento)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getRecursosBySeguimiento", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@seguimiento", seguimiento));
                        var response = new List<Recursos>();
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

        public async Task<int> RecursosSinOficio(int seguimiento)
        {
            int total = -1;
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getRecursosSinOficio", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@total", SqlDbType.Int)).Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters.Add(new SqlParameter("@seguimiento", seguimiento));
                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        total = Convert.ToInt32(cmd.Parameters["@total"].Value);


                        return total;
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return -1;
            }
        }

        public async Task<Recursos> getRecursoById(int recurso)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getRecursoById", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@id", recurso));
                        var response = new Recursos();
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

        public async Task<int> insertaRecurso(Recursos recursos)
        {
            DateTime date = DateTime.Now;
            string date_str = date.ToString("yyyyMMddHHmmss");
            int id = 0;
            string saveFile = "";

            if (recursos.Archivo != null)
            {
                saveFile = await guardaArchivo(recursos.Archivo, recursos.NumeroOficio, date_str);
            }

            try
            {
                if (saveFile.Equals("Ok") || recursos.Archivo == null)
                {
                    using (SqlConnection sql = new SqlConnection(_connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_insertarRecurso", sql))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@id", recursos.Id)).Direction = ParameterDirection.Output;
                            cmd.Parameters.Add(new SqlParameter("@seguimiento", recursos.SeguimientoId));
                            cmd.Parameters.Add(new SqlParameter("@tipo", recursos.Tipo));
                            cmd.Parameters.Add(new SqlParameter("@oficio", recursos.NumeroOficio));
                            if (!recursos.FechaOficio.ToString("yyyy").Equals("0001"))
                                cmd.Parameters.Add(new SqlParameter("@fechaOficio", recursos.FechaOficio));
                            cmd.Parameters.Add(new SqlParameter("@monto", recursos.Monto));
                            if (recursos.Oficio != null && !recursos.Oficio.Equals(""))
                            {
                                cmd.Parameters.Add(new SqlParameter("@nombreOficio", recursos.Oficio));
                            }
                            if (recursos.Archivo != null)
                            {
                                cmd.Parameters.Add(new SqlParameter("@archivo", (date_str + "_" + recursos.Archivo.FileName)));
                            }
                            cmd.Parameters.Add(new SqlParameter("@observaciones", recursos.Observaciones));

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
        public async Task<int> actualizaRecurso(Recursos recursos)
        {
            DateTime date = DateTime.Now;
            string date_str = date.ToString("yyyyMMddHHmmss");
            int id = 0;
            string saveFile = "";

            if (recursos.Id != 0 && recursos.Archivo != null)
            {
                int isDeleted = await eliminaRecursos(recursos);
            }

            if (recursos.Archivo != null)
            {
                saveFile = await guardaArchivo(recursos.Archivo, recursos.NumeroOficio, date_str);
            }
            try
            {
                if (saveFile.Equals("Ok") || recursos.Archivo == null)
                {
                    using (SqlConnection sql = new SqlConnection(_connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_actualizarRecurso", sql))
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@id", recursos.Id));
                            cmd.Parameters.Add(new SqlParameter("@tipo", recursos.Tipo));
                            cmd.Parameters.Add(new SqlParameter("@oficio", recursos.NumeroOficio));
                            if (recursos.Oficio != null && !recursos.Oficio.Equals(""))
                            {
                                cmd.Parameters.Add(new SqlParameter("@nombreOficio", recursos.Oficio));
                            }
                            if (!recursos.FechaOficio.ToString("yyyy").Equals("0001"))
                                cmd.Parameters.Add(new SqlParameter("@fechaOficio", recursos.FechaOficio));
                            cmd.Parameters.Add(new SqlParameter("@monto", recursos.Monto));
                            if (recursos.Archivo != null)
                            {
                                cmd.Parameters.Add(new SqlParameter("@archivo", (date_str + "_" + recursos.Archivo.FileName)));
                            }
                            cmd.Parameters.Add(new SqlParameter("@observaciones", recursos.Observaciones));

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

        public async Task<string> guardaArchivo(IFormFile archivo, long oficio, string date)
        {
            long size = archivo.Length;
            string folderOficio = oficio+"";

            string newPath = Directory.GetCurrentDirectory() + "\\Oficios\\" + folderOficio;
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

        public async Task<int> eliminaRecursos(Recursos recursos)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_eliminarRecursos", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@id", recursos.Id));
                        cmd.Parameters.Add(new SqlParameter("@archivo", System.Data.SqlDbType.VarChar, 1024)).Direction = System.Data.ParameterDirection.Output;
                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        string archivo = (cmd.Parameters["@archivo"].Value).ToString();
                        if (archivo != null && !archivo.Equals("")) {
                            string newPath = Directory.GetCurrentDirectory() + "\\Oficios\\" + recursos.NumeroOficio + "\\" + archivo;
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
        
        public async Task<int> eliminaRecurso(Recursos recursos)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_eliminarRecurso", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@id", recursos.Id));
                        cmd.Parameters.Add(new SqlParameter("@archivo", System.Data.SqlDbType.VarChar, 1024)).Direction = System.Data.ParameterDirection.Output;
                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        string archivo = (cmd.Parameters["@archivo"].Value).ToString();
                        if (archivo != null && !archivo.Equals("")) {
                            string newPath = Directory.GetCurrentDirectory() + "\\Oficios\\" + recursos.NumeroOficio + "\\" + archivo;
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

        private Recursos MapToValue(SqlDataReader reader)
        {
            return new Recursos {
                Id = reader["Id"] != DBNull.Value ? (int)reader["Id"] : 0,
                SeguimientoId = reader["SeguimientoId"] != DBNull.Value ? (int)reader["SeguimientoId"] : 0,
                Tipo = reader["Tipo"] != DBNull.Value ? reader["Tipo"].ToString() : "",
                TipoRecurso = reader["TipoRecurso"] != DBNull.Value ? reader["TipoRecurso"].ToString() : "",
                NumeroOficio = reader["NumeroOficio"] != DBNull.Value ? (long)reader["NumeroOficio"] : 0,
                FechaOficio = reader["FechaOficio"] != DBNull.Value ? Convert.ToDateTime(reader["FechaOficio"]) : Convert.ToDateTime("2001-01-01T00:00:00"),
                Monto = reader["Monto"] != DBNull.Value ? (decimal)reader["Monto"] : 0,
                NombreArchivo = reader["Archivo"] != DBNull.Value ? reader["Archivo"].ToString() : "",
                Oficio = reader["Oficio"] != DBNull.Value ? reader["Oficio"].ToString() : "",
                Observaciones = reader["Observaciones"] != DBNull.Value ? reader["Observaciones"].ToString() : "",
                FechaCreacion = reader["FechaCreacion"] != DBNull.Value ? Convert.ToDateTime(reader["FechaCreacion"]) : Convert.ToDateTime("2001-01-01T00:00:00"),
                FechaActualizacion = reader["FechaActualizacion"] != DBNull.Value ? Convert.ToDateTime(reader["FechaActualizacion"]) :
                                    reader["FechaCreacion"] != DBNull.Value ? Convert.ToDateTime(reader["FechaCreacion"]) : Convert.ToDateTime("2001-01-01T00:00:00")
            };
        }
    }
}
