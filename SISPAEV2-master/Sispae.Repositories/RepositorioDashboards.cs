using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Sispae.Entities.MDashboards;
using Sispae.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Repositories
{
    public class RepositorioDashboards : IRepositorioDashboards
    {
        private readonly string _connectionString;

        public RepositorioDashboards(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DatabaseConnection"); ;
        }

        public async Task<List<Dashboard>> GetDashboardEstatus(int anio)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_dashboardEstatus", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@anio", anio));
                        var response = new List<Dashboard>();
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

        public async Task<List<Dashboard>> GetDashboardEstatusAhorros(int anio)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_dashboardEstatusAhorros", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@anio", anio));
                        var response = new List<Dashboard>();
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

        public async Task<List<DashboardPUEG>> GetDashboardProyectosUEG(int anio, string tipo, string estatus)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_dashboardProyectosUEG", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@anio", anio));
                        cmd.Parameters.Add(new SqlParameter("@tipo", tipo));
                        cmd.Parameters.Add(new SqlParameter("@estatus", estatus));
                        var response = new List<DashboardPUEG>();
                        await sql.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response.Add(MapToValuePUEG(reader));
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

        private Dashboard MapToValue(SqlDataReader reader)
        {
            return new Dashboard { 
                Estatus = reader["Estatus"] != DBNull.Value ? reader["Estatus"].ToString() :"",
                Fondo = reader["Fondo"] != DBNull.Value ? reader["Fondo"].ToString() :"",
                Icono = reader["Icono"] != DBNull.Value ? reader["Icono"].ToString() :"",
                TotalProyectos = reader["TotalProyectos"] != DBNull.Value ? (int) reader["TotalProyectos"] : 0
            };
        }
        private DashboardPUEG MapToValuePUEG(SqlDataReader reader)
        {
            return new DashboardPUEG
            { 
                Id = reader["Id"] != DBNull.Value ? (int) reader["Id"] : 0,
                NumeroUEG = reader["NumeroUEG"] != DBNull.Value ? (int) reader["NumeroUEG"] : 0,
                Nombre = reader["Nombre"] != DBNull.Value ? reader["Nombre"].ToString() : "",
                Icono = reader["Icono"] != DBNull.Value ? reader["Icono"].ToString() : "",
                Fondo = reader["Fondo"] != DBNull.Value ? reader["Fondo"].ToString() : "",
                FondoHexadecimal = reader["FondoHexadecimal"] != DBNull.Value ? reader["FondoHexadecimal"].ToString() : "",
                TotalProyectos = reader["TotalProyectos"] != DBNull.Value ? (int) reader["TotalProyectos"] : 0,
                TotalProyectosUEG = reader["TotalProyectosUEG"] != DBNull.Value ? (int) reader["TotalProyectosUEG"] : 0
            };
        }
    }
}
