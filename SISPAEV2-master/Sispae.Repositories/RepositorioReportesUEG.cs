using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Sispae.Entities.MReportes;
using Sispae.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Sispae.Repositories
{
    public class RepositorioReportesUEG : IRepositorioReportesUEG
    {
        private readonly string _connectionString;
        private List<string> noProyecto = new List<string>();

        public RepositorioReportesUEG(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DatabaseConnection");
        }

        public async Task<List<ReporteUEG>> GetProyectosUEG(int ueg, int ejercicio, int capitulo, string presupuesto, int id)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getReporteUEG", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ueg", ueg));
                        cmd.Parameters.Add(new SqlParameter("@ejercicio", ejercicio));
                        cmd.Parameters.Add(new SqlParameter("@capitulo", capitulo));
                        cmd.Parameters.Add(new SqlParameter("@presupuesto", presupuesto));
                        var response = new List<ReporteUEG>();
                        await sql.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                id++;
                                response.Add(MapToValue(reader,id));
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
        public async Task<List<ReporteUEG>> GetProyectosByUEG(int ueg, int ejercicio, string presupuesto)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getReporteGeneralUEG", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ueg", ueg));
                        cmd.Parameters.Add(new SqlParameter("@ejercicio", ejercicio));
                        cmd.Parameters.Add(new SqlParameter("@presupuesto", presupuesto));
                        var response = new List<ReporteUEG>();
                        await sql.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response.Add(MapToValueRGUEG(reader));
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
        public async Task<List<ReporteGeneral>> GetReporteGeneralUEG(int ejercicio, string presupuesto)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getReporteGeneral", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ejercicio", ejercicio));
                        cmd.Parameters.Add(new SqlParameter("@tipo", presupuesto));
                        var response = new List<ReporteGeneral>();
                        await sql.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response.Add(MapToValueGeneral(reader));
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
        public async Task<List<ReporteGeneral>> GetReporteGeneralAhorrosUEG(int ejercicio, string presupuesto)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getReporteGeneralAhorros", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ejercicio", ejercicio));
                        cmd.Parameters.Add(new SqlParameter("@tipo", presupuesto));
                        var response = new List<ReporteGeneral>();
                        await sql.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response.Add(MapToValueGeneralA(reader));
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
        public async Task<List<ReportePunto>> GetReportePunto(int ejercicio)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getReportePunto", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ejercicio", ejercicio));
                        var response = new List<ReportePunto>();
                        await sql.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response.Add(MapToValueRPunto(reader));
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
        public async Task<List<ReporteCalendario>> GetCalendarizacion()
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getCalendarizacion", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        var response = new List<ReporteCalendario>();
                        await sql.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response.Add(MapToValueReporteCalendario(reader));
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
        public async Task<List<ReportePunto>> GetReporteGeneralTP(int ejercicio)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getReporteGeneralTP", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ejercicio", ejercicio));
                        var response = new List<ReportePunto>();
                        await sql.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response.Add(MapToValueReporteTP(reader));
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
        public async Task<List<ReporteAnticipadas>> GetReporteAnticipadas(int ejercicio, int ueg,int capitulo, int id)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getReporteAnticipadas", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ejercicio", ejercicio));
                        cmd.Parameters.Add(new SqlParameter("@ueg", ueg));
                        cmd.Parameters.Add(new SqlParameter("@capitulo", capitulo));
                        var response = new List<ReporteAnticipadas>();
                        await sql.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                id++;
                                response.Add(MapToValueAnticipadas(reader,id));
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
        public async Task<List<ReportePuntoAhorros>> GetReportePuntoAhorros(int ejercicio)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getReportePuntoAhorros", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ejercicio", ejercicio));
                        var response = new List<ReportePuntoAhorros>();
                        await sql.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response.Add(MapToValueAPunto(reader));
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
        public async Task<List<ReportePunto>> GetReportePuntoByUEG(int ejercicio, int ueg)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getReportePuntoByUEG", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ejercicio", ejercicio));
                        cmd.Parameters.Add(new SqlParameter("@ueg", ueg));
                        var response = new List<ReportePunto>();
                        await sql.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response.Add(MapToValueRPunto(reader));
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
        public async Task<List<ReportePuntoAhorros>> GetReportePuntoAhorrosByUEG(int ejercicio, int ueg)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getReportePuntoAhorrosByUEG", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ejercicio", ejercicio));
                        cmd.Parameters.Add(new SqlParameter("@ueg", ueg));
                        var response = new List<ReportePuntoAhorros>();
                        await sql.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response.Add(MapToValueAPunto(reader));
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
        
        public async Task<List<ReporteCalendarioUEG>> GetReporteCalendarioUEG(int ejercicio, int ueg)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getCalendarioByUEG", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@anio", ejercicio));
                        cmd.Parameters.Add(new SqlParameter("@ueg", ueg));
                        var response = new List<ReporteCalendarioUEG>();
                        await sql.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response.Add(MapToValueRCUEG(reader));
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

        private ReporteGeneral MapToValueGeneral(SqlDataReader reader)
        {
            return new ReporteGeneral
            {
                NumeroUEG = reader["NumeroUEG"] != DBNull.Value ? (int)reader["NumeroUEG"] : 0,
                Descripcion = reader["Descripcion"] != DBNull.Value ? reader["Descripcion"].ToString() : "",
                Capitulo2000 = reader["Capitulo2000"] != DBNull.Value ? (int)reader["Capitulo2000"] : 0,
                Capitulo3000 = reader["Capitulo3000"] != DBNull.Value ? (int)reader["Capitulo3000"] : 0,
                TotalProyectos = reader["TotalProyectos"] != DBNull.Value ? (int)reader["TotalProyectos"] : 0,
                TotalRecursos2000 = reader["TotalRecursos2000"] != DBNull.Value ? (decimal)reader["TotalRecursos2000"] : 0,
                TotalRecursos3000 = reader["TotalRecursos3000"] != DBNull.Value ? (decimal)reader["TotalRecursos3000"] : 0,
                TotalFinal = reader["TotalFinal"] != DBNull.Value ? (decimal)reader["TotalFinal"] : 0,
            };
        }

        private ReporteGeneral MapToValueGeneralA(SqlDataReader reader)
        {
            return new ReporteGeneral
            {
                NumeroUEG = reader["NumeroUEG"] != DBNull.Value ? (int)reader["NumeroUEG"] : 0,
                Descripcion = reader["Descripcion"] != DBNull.Value ? reader["Descripcion"].ToString() : "",
                Capitulo2000 = reader["Capitulo2000"] != DBNull.Value ? (int)reader["Capitulo2000"] : 0,
                Capitulo3000 = reader["Capitulo3000"] != DBNull.Value ? (int)reader["Capitulo3000"] : 0,
                Capitulo5000 = reader["Capitulo5000"] != DBNull.Value ? (int)reader["Capitulo5000"] : 0,
                TotalProyectos = reader["TotalProyectos"] != DBNull.Value ? (int)reader["TotalProyectos"] : 0,
                TotalRecursos2000 = reader["TotalRecursos2000"] != DBNull.Value ? (decimal)reader["TotalRecursos2000"] : 0,
                TotalRecursos3000 = reader["TotalRecursos3000"] != DBNull.Value ? (decimal)reader["TotalRecursos3000"] : 0,
                TotalRecursos5000 = reader["TotalRecursos5000"] != DBNull.Value ? (decimal)reader["TotalRecursos5000"] : 0,
                TotalFinal = reader["TotalFinal"] != DBNull.Value ? (decimal)reader["TotalFinal"] : 0,
            };
        }

        private ReporteUEG MapToValue(SqlDataReader reader,int id)
        {
            return new ReporteUEG 
            { 
                Id = id,
                UEGId = reader["UEGId"] != DBNull.Value ? (int)reader["UEGId"] : 0,
                Capitulo = reader["Capitulo"] != DBNull.Value ? (int)reader["Capitulo"] : 0,
                Presupuesto = reader["Presupuesto"] != DBNull.Value ? reader["Presupuesto"].ToString() : "",
                Proyecto = reader["Proyecto"] != DBNull.Value ? reader["Proyecto"].ToString() : "",
                Partidas = reader["Partidas"] != DBNull.Value ? reader["Partidas"].ToString() : "",
                FechaEstimada  = reader["FechaEstimada"] != DBNull.Value ? reader["FechaEstimada"].ToString() : "",
                Importe = reader["Importe"] != DBNull.Value ? (decimal)reader["Importe"] : 0,
                Importes = reader["Importes"] != DBNull.Value ? reader["Importes"].ToString() : "",
            };
        }
        
        private ReporteUEG MapToValueRGUEG(SqlDataReader reader)
        {
            return new ReporteUEG 
            { 
                Id = reader["Id"] != DBNull.Value ? (int)reader["Id"] : 0,
                UEGId = reader["UEGId"] != DBNull.Value ? (int)reader["UEGId"] : 0,
                Capitulo = reader["Capitulo"] != DBNull.Value ? (int)reader["Capitulo"] : 0,
                Presupuesto = reader["Presupuesto"] != DBNull.Value ? reader["Presupuesto"].ToString() : "",
                Proyecto = reader["Proyecto"] != DBNull.Value ? reader["Proyecto"].ToString() : "",
                Partidas = reader["Partidas"] != DBNull.Value ? reader["Partidas"].ToString() : "",
                FechaEstimada  = reader["FechaEstimada"] != DBNull.Value ? reader["FechaEstimada"].ToString() : "",
                Importe = reader["Importe"] != DBNull.Value ? (decimal)reader["Importe"] : 0,
                Importes = reader["Importes"] != DBNull.Value ? reader["Importes"].ToString() : "",
            };
        }

        private ReportePunto MapToValueRPunto(SqlDataReader reader)
        {
            return new ReportePunto
            { 
                Id = reader["Id"] != DBNull.Value ? (int)reader["Id"] : 0,
                NumeroUEG = reader["NumeroUEG"] != DBNull.Value ? (int)reader["NumeroUEG"] : 0,
                Capitulo = reader["Capitulo"] != DBNull.Value ? (int)reader["Capitulo"] : 0,
                NoProyecto = buscaProyecto(reader["Proyecto"].ToString()+"|" + (int)reader["NumeroUEG"]+"|"+ reader["Tipo"].ToString()) ? 0 : 1,
                ColumnaTramite = reader["Tramite"] != DBNull.Value ? (Convert.ToDecimal(reader["Tramite"]) > 0 ? 1:0):0,
                ClaveProyecto = reader["ClaveProyecto"] != DBNull.Value ? (int)reader["ClaveProyecto"] : 0,
                Descripcion = reader["Descripcion"] != DBNull.Value ? reader["Descripcion"].ToString() : "",
                Tipo = reader["Tipo"] != DBNull.Value ? reader["Tipo"].ToString() : "",
                Partidas = reader["Partidas"] != DBNull.Value ? reader["Partidas"].ToString() : "",
                Proyecto = reader["Proyecto"] != DBNull.Value ? reader["Proyecto"].ToString() : "",
                Trimestre = reader["Trimestre"] != DBNull.Value ? reader["Trimestre"].ToString() : "",
                Importe = reader["Importe"] != DBNull.Value ? (decimal)reader["Importe"] : 0,
                SuficienciaDGPPT = reader["SuficienciaDGPPT"] != DBNull.Value ? (decimal)reader["SuficienciaDGPPT"] : 0,
                SuficienciaDGRM = reader["SuficienciaDGRM"] != DBNull.Value ? (decimal)reader["SuficienciaDGRM"] : 0,
                RecursosUtilizar = reader["RecursosUtilizar"] != DBNull.Value ? (decimal)reader["RecursosUtilizar"] : 0,
                FechaEstimada  = reader["FechaEstimada"] != DBNull.Value ? reader["FechaEstimada"].ToString() : "",
                FechaAdjudicacion = reader["FechaAdjudicacion"] != DBNull.Value ? reader["FechaAdjudicacion"].ToString() : "",
                ImporteTotal = reader["ImporteTotal"] != DBNull.Value ? (decimal)reader["ImporteTotal"] : 0,
                MontoPropio = reader["MontoPropio"] != DBNull.Value ? (decimal)reader["MontoPropio"] : 0,
                MontoAdjudicado = reader["MontoAdjudicado"] != DBNull.Value ? (decimal)reader["MontoAdjudicado"] : 0,
                Tramite = reader["Tramite"] != DBNull.Value ? (decimal)reader["Tramite"] : 0,
                TramiteCovid = reader["TramiteCovid"] != DBNull.Value ? (decimal)reader["TramiteCovid"] : 0,
                RecursosDisponibles = reader["RecursosDisponibles"] != DBNull.Value ? (decimal)reader["RecursosDisponibles"] : 0,
                DTesoreria = reader["DTesoreria"] != DBNull.Value ? (decimal)reader["DTesoreria"] : 0,
                TesoreriaDGRM = reader["TesoreriaDGRM"] != DBNull.Value ? (decimal)reader["TesoreriaDGRM"] : 0,
                OficiosTesoreria = reader["OficiosTesoreria"] != DBNull.Value ? reader["OficiosTesoreria"].ToString() : "",
                FechasTesoreria  = reader["FechasTesoreria"] != DBNull.Value ? reader["FechasTesoreria"].ToString() : "",
                DTCovid = reader["DTCovid"] != DBNull.Value ? (decimal)reader["DTCovid"] : 0,
                OficiosTCovid = reader["OficiosTCovid"] != DBNull.Value ? reader["OficiosTCovid"].ToString() : "",
                FechasTCovid = reader["FechasTCovid"] != DBNull.Value ? reader["FechasTCovid"].ToString() : "",
                DDeductiva = reader["DDeductiva"] != DBNull.Value ? (decimal)reader["DDeductiva"] : 0,
                OficiosDeductivas = reader["OficiosDeductivas"] != DBNull.Value ? reader["OficiosDeductivas"].ToString() : "",
                FechasDeductivas = reader["FechasDeductivas"] != DBNull.Value ? reader["FechasDeductivas"].ToString() : "",
                Prestador = reader["Prestador"] != DBNull.Value ? reader["Prestador"].ToString() : "",
                Contrato = reader["Contrato"] != DBNull.Value ? reader["Contrato"].ToString() : "",
                TipoAdjudicacion = reader["TipoAdjudicacion"] != DBNull.Value ? reader["TipoAdjudicacion"].ToString() : "",
                Observaciones = reader["Observaciones"] != DBNull.Value ? reader["Observaciones"].ToString() : "",
                Apartado = reader["Apartado"] != DBNull.Value ? reader["Apartado"].ToString() : "",
            };
        }

        private ReportePuntoAhorros MapToValueAPunto(SqlDataReader reader)
        {
            return new ReportePuntoAhorros
            { 
                Id = reader["Id"] != DBNull.Value ? (int)reader["Id"] : 0,
                NumeroUEG = reader["NumeroUEG"] != DBNull.Value ? (int)reader["NumeroUEG"] : 0,
                Descripcion = reader["Descripcion"] != DBNull.Value ? reader["Descripcion"].ToString() : "",
                ClaveProyecto = reader["ClaveProyecto"] != DBNull.Value ? (int)reader["ClaveProyecto"] : 0,
                Evento = reader["Evento"] != DBNull.Value ? (bool) reader["Evento"] : false,
                Partidas = reader["Partidas"] != DBNull.Value ? reader["Partidas"].ToString() : "",
                Trimestre = reader["Trimestre"] != DBNull.Value ? (int)reader["Trimestre"] : 0,
                Proyecto = reader["Proyecto"] != DBNull.Value ? reader["Proyecto"].ToString() : "",
                Importe = reader["Importe"] != DBNull.Value ? (decimal)reader["Importe"] : 0,
                FechaAdjudicacion = reader["FechaAdjudicacion"] != DBNull.Value ? reader["FechaAdjudicacion"].ToString() : "",
                NombreOficioSolicitud = reader["NombreOficioSolicitud"] != DBNull.Value ? reader["NombreOficioSolicitud"].ToString() : "",
                RecursosSolicitados = reader["RecursosSolicitados"] != DBNull.Value ? (decimal)reader["RecursosSolicitados"] : 0,
                FechasSolicitud = reader["FechasSolicitud"] != DBNull.Value ? reader["FechasSolicitud"].ToString() : "",
                AreaFuncional = reader["AreaFuncional"] != DBNull.Value ? reader["AreaFuncional"].ToString() : "",
                OficiosOtorgados = reader["OficiosOtorgados"] != DBNull.Value ? reader["OficiosOtorgados"].ToString() : "",
                NombreOficioOtorgado = reader["NombreOficioOtorgado"] != DBNull.Value ? reader["NombreOficioOtorgado"].ToString() : "",
                RecursosOtorgados = reader["RecursosOtorgados"] != DBNull.Value ? (decimal)reader["RecursosOtorgados"] : 0,
                FechasOtorgados = reader["FechasOtorgados"] != DBNull.Value ? reader["FechasOtorgados"].ToString() : "",
                Monto = reader["Monto"] != DBNull.Value ? (decimal)reader["Monto"] : 0,
                Nombre = reader["Nombre"] != DBNull.Value ? reader["Nombre"].ToString() : "",
                Contrato = reader["Contrato"] != DBNull.Value ? reader["Contrato"].ToString() : "",
                Variacion = reader["Variacion"] != DBNull.Value ? (decimal)reader["Variacion"] : 0,
                DTesoreria = reader["DTesoreria"] != DBNull.Value ? (decimal)reader["DTesoreria"] : 0,
                NombreOficioDGPPT = reader["NombreOficioDGPPT"] != DBNull.Value ? reader["NombreOficioDGPPT"].ToString() : "",
                DDeductiva = reader["DDeductiva"] != DBNull.Value ? (decimal)reader["DDeductiva"] : 0,
                OficiosDeductivas = reader["OficiosDeductivas"] != DBNull.Value ? reader["OficiosDeductivas"].ToString() : "",
                Observaciones = reader["Observaciones"] != DBNull.Value ? reader["Observaciones"].ToString() : "",
            };
        }

        private ReporteAnticipadas MapToValueAnticipadas(SqlDataReader reader,int id)
        {
            return new ReporteAnticipadas
            {
                Capitulo = reader["Capitulo"] != DBNull.Value ? (int)reader["Capitulo"] : 0,
                ClaveProyecto = id,
                Partidas = reader["Partidas"] != DBNull.Value ? reader["Partidas"].ToString() : "",
                Proyecto = reader["Proyecto"] != DBNull.Value ? reader["Proyecto"].ToString() : "",
                TipoProyecto = reader["TipoProyecto"] != DBNull.Value ? reader["TipoProyecto"].ToString() : "",
                FechaEstimada = reader["FechaEstimada"] != DBNull.Value ? reader["FechaEstimada"].ToString() : "",
                Importe = reader["Importe"] != DBNull.Value ? (decimal)reader["Importe"] : 0,
            };
        }

        private ReportePunto MapToValueReporteTP(SqlDataReader reader)
        {
            return new ReportePunto
            {
                NumeroUEG = reader["NumeroUEG"] != DBNull.Value ? (int)reader["NumeroUEG"] : 0,
                Descripcion = reader["Descripcion"] != DBNull.Value ? reader["Descripcion"].ToString() : "",
                TipoId = reader["TipoId"] != DBNull.Value ? (int)reader["TipoId"] : 0,
                Nombre = reader["Nombre"] != DBNull.Value ? reader["Nombre"].ToString() : "",
                Importe = reader["Importe"] != DBNull.Value ? (decimal)reader["Importe"] : 0,
            };
        }  
        
        private ReporteCalendario MapToValueReporteCalendario(SqlDataReader reader)
        {
            return new ReporteCalendario
            {
                Capitulo = reader["Capitulo"] != DBNull.Value ? reader["Capitulo"].ToString() : "",
                Primer = reader["Primer"] != DBNull.Value ? (decimal)reader["Primer"] : 0,
                Segundo = reader["Segundo"] != DBNull.Value ? (decimal)reader["Segundo"] : 0,
                Tercero = reader["Tercero"] != DBNull.Value ? (decimal)reader["Tercero"] : 0,
                Cuarto = reader["Cuarto"] != DBNull.Value ? (decimal)reader["Cuarto"] : 0,
            };
        }   
        
        private ReporteCalendarioUEG MapToValueRCUEG(SqlDataReader reader)
        {
            return new ReporteCalendarioUEG
            {
                Unidad = reader["Unidad"] != DBNull.Value ? reader["Unidad"].ToString() : "",
                Partida = reader["Partida"] != DBNull.Value ? reader["Partida"].ToString() : "",
                NumeroUEG = reader["NumeroUEG"] != DBNull.Value ? (int)reader["NumeroUEG"] : 0,
                Enero = reader["Enero"] != DBNull.Value ? (decimal)reader["Enero"] : 0,
                Febrero = reader["Febrero"] != DBNull.Value ? (decimal)reader["Febrero"] : 0,
                Marzo = reader["Marzo"] != DBNull.Value ? (decimal)reader["Marzo"] : 0,
                Abril = reader["Abril"] != DBNull.Value ? (decimal)reader["Abril"] : 0,
                Mayo = reader["Mayo"] != DBNull.Value ? (decimal)reader["Mayo"] : 0,
                Junio = reader["Junio"] != DBNull.Value ? (decimal)reader["Junio"] : 0,
                Julio = reader["Julio"] != DBNull.Value ? (decimal)reader["Julio"] : 0,
                Agosto = reader["Agosto"] != DBNull.Value ? (decimal)reader["Agosto"] : 0,
                Septiembre = reader["Septiembre"] != DBNull.Value ? (decimal)reader["Septiembre"] : 0,
                Octubre = reader["Octubre"] != DBNull.Value ? (decimal)reader["Octubre"] : 0,
                Noviembre = reader["Noviembre"] != DBNull.Value ? (decimal)reader["Noviembre"] : 0,
                Diciembre = reader["Diciembre"] != DBNull.Value ? (decimal)reader["Diciembre"] : 0,
            };
        }   

        private bool buscaProyecto(string proyecto)
        {
            for(var i = 0; i < noProyecto.Count; i++)
            {
                if (noProyecto[i].Equals(proyecto))
                {
                    return true;
                }
            }
            noProyecto.Add(proyecto);
            return false;
        }
    }
}
