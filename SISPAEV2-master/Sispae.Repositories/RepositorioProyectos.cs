using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Sispae.Entities.MDashboards;
using Sispae.Entities.MProyectos;
using Sispae.Entities.Vistas;
using Sispae.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Repositories
{
    public class RepositorioProyectos: IRepositorioProyectos
    {
        private readonly string _connectionString;

        public RepositorioProyectos(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DatabaseConnection"); ;
        }

        public async Task<List<DashboardProyectos>> GetProyectosByUEG(int UEG, int ejercicio,int usuario, string catalogo)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getProyectosUEG", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ueg", UEG));
                        cmd.Parameters.Add(new SqlParameter("@ejercicio", ejercicio));
                        cmd.Parameters.Add(new SqlParameter("@user", usuario));
                        cmd.Parameters.Add(new SqlParameter("@tipo", catalogo));
                        var response = new List<DashboardProyectos>();
                        await sql.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response.Add(MapToValueDashboard(reader));
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
        public async Task<List<VProyectos>> GetProyectosEstatusByUEG(int UEG, string estatus, int ejercicio, int usuario, string catalogo)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getProyectosByEstatus", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ueg", UEG));
                        cmd.Parameters.Add(new SqlParameter("@estatus", estatus));
                        cmd.Parameters.Add(new SqlParameter("@ejercicio", ejercicio));
                        cmd.Parameters.Add(new SqlParameter("@user", usuario));
                        cmd.Parameters.Add(new SqlParameter("@tipo", catalogo));
                        var response = new List<VProyectos>();
                        await sql.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response.Add(MapToValueDProyecto(reader));
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
        public async Task<VProyectos> GetProyectoById(int id)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getProyectoById", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@id", id));
                        var response = new VProyectos();
                        await sql.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response = MapToValueProyecto(reader);
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
        private DashboardProyectos MapToValueDashboard(SqlDataReader reader)
        {
            return new DashboardProyectos
            {
                UEGId = reader["UEGId"] != DBNull.Value ? (int)reader["UEGId"] : 0,
                Ejercicio = reader["Ejercicio"] != DBNull.Value ? (int)reader["Ejercicio"] : 0,
                Estatus = reader["Estatus"] != DBNull.Value ? reader["Estatus"].ToString() : "",
                Icono = reader["Icono"] != DBNull.Value ? reader["Icono"].ToString() : "",
                Fondo = reader["Fondo"] != DBNull.Value ? reader["Fondo"].ToString() : "",
                TotalEstatus = reader["TotalEstatus"] != DBNull.Value ? (int)reader["TotalEstatus"] : 0,
            };
        }        
        private VProyectos MapToValueProyecto(SqlDataReader reader)
        {
            return new VProyectos {
                Id = reader["Id"] != DBNull.Value ? (int)reader["Id"] : 0,
                ProyectoId = reader["ProyectoId"] != DBNull.Value ? (int)reader["ProyectoId"] : 0,
                Proyecto = reader["Proyecto"] != DBNull.Value ? reader["Proyecto"].ToString() : "",
                UEGId = reader["UEGId"] != DBNull.Value ? (int)reader["UEGId"] : 0,
                TipoId = reader["TipoId"] != DBNull.Value ? (int)reader["TipoId"] : 0,
                //NumeroUEG = reader["NumeroUEG"] != DBNull.Value ? (int)reader["NumeroUEG"] : 0,
                Clasificacion = reader["Clasificacion"] != DBNull.Value ? reader["Clasificacion"].ToString() : "",
                UsuarioId = reader["UsuarioId"] != DBNull.Value ? (int)reader["UsuarioId"] : 0,
                Evento = reader["Evento"] != DBNull.Value ? (bool)reader["Evento"] : false,
                Ejercicio = reader["Ejercicio"] != DBNull.Value ? (int)reader["Ejercicio"] : 0,                
                Capitulo = reader["Capitulo"] != DBNull.Value ? (int)reader["Capitulo"] : 0,
                ClaveProyecto = reader["ClaveProyecto"] != DBNull.Value ? (int)reader["ClaveProyecto"] : 0,                
                Anio = reader["Anio"] != DBNull.Value ? (int)reader["Anio"] : 0,
                Mes = reader["Mes"] != DBNull.Value ? reader["Mes"].ToString() : "",
                Tipo = reader["Tipo"] != DBNull.Value ? reader["Tipo"].ToString() : "",
                Icono  = reader["Icono"] != DBNull.Value ? reader["Icono"].ToString() : "",
                TipoProyecto = reader["TipoProyecto"] != DBNull.Value ? reader["TipoProyecto"].ToString() : "",
                Fondo = reader["Fondo"] != DBNull.Value ? reader["Fondo"].ToString() : "",
                Estatus = reader["Estatus"] != DBNull.Value ? reader["Estatus"].ToString() : "",
                RecursosDisponibles = reader["RecursosDisponibles"] != DBNull.Value ? (decimal)reader["RecursosDisponibles"] : 0,
                Importe = reader["Importe"] != DBNull.Value ? (decimal)reader["Importe"]: 0,
                TotalSeguimientos = reader["TotalSeguimientos"] != DBNull.Value ? (int)reader["TotalSeguimientos"] : 0,
                FechaCreacion = reader["FechaCreacion"] != DBNull.Value ? Convert.ToDateTime(reader["FechaCreacion"]) : Convert.ToDateTime("2001-01-01T00:00:00"),
                FechaActualizacion = reader["FechaActualizacion"] != DBNull.Value ? Convert.ToDateTime(reader["FechaActualizacion"]) : 
                                    reader["FechaCreacion"] != DBNull.Value ? Convert.ToDateTime(reader["FechaCreacion"]): Convert.ToDateTime("2001-01-01T00:00:00")
            };
        }
        private VProyectos MapToValueDProyecto(SqlDataReader reader)
        {
            return new VProyectos
            {
                Id = reader["Id"] != DBNull.Value ? (int)reader["Id"] : 0,
                Proyecto = reader["Proyecto"] != DBNull.Value ? reader["Proyecto"].ToString() : "",
                NumeroUEG = reader["NumeroUEG"] != DBNull.Value ? (int)reader["NumeroUEG"] : 0,
                ClaveProyecto = reader["ClaveProyecto"] != DBNull.Value ? (int)reader["ClaveProyecto"] : 0,
                Evento = reader["Evento"] != DBNull.Value ? (bool)reader["Evento"] : false,
                Anio = reader["Anio"] != DBNull.Value ? (int)reader["Anio"] : 0,
                Importe = reader["Importe"] != DBNull.Value ? (decimal)reader["Importe"] : 0,
                Mes = reader["Mes"] != DBNull.Value ? reader["Mes"].ToString() : "",
                Estatus = reader["Estatus"] != DBNull.Value ? reader["Estatus"].ToString() : "",
                Tipo = reader["Tipo"] != DBNull.Value ? reader["Tipo"].ToString() : "",
                Fondo = reader["Fondo"] != DBNull.Value ? reader["Fondo"].ToString() : "",
                FechaCreacion = reader["FechaCreacion"] != DBNull.Value ? Convert.ToDateTime(reader["FechaCreacion"]) : Convert.ToDateTime("2001-01-01T00:00:00"),
                FechaActualizacion = reader["FechaActualizacion"] != DBNull.Value ? Convert.ToDateTime(reader["FechaActualizacion"]) :
                                    reader["FechaCreacion"] != DBNull.Value ? Convert.ToDateTime(reader["FechaCreacion"]) : Convert.ToDateTime("2001-01-01T00:00:00")
            };
        }
    }
}
