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
    public class RepositorioIntegracion: IRepositorioIntegracion
    {
        private readonly string _connectionString;

        public RepositorioIntegracion(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DatabaseConnection"); ;
        }

        public async Task<int> NuevaIntegracionProyecto(List<Integracion> integracion)
        {
            int id = -1;
            try
            {
                foreach (var ip in integracion)
                {
                    using (SqlConnection sql = new SqlConnection(_connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_insertaIntegracion", sql))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.BigInt)).Direction = ParameterDirection.Output;
                            cmd.Parameters.Add(new SqlParameter("@ueg", ip.UEGId));
                            cmd.Parameters.Add(new SqlParameter("@usuario", ip.UsuarioId));
                            if (ip.Tipo != null && ip.Tipo.Equals("Ahorros"))
                                cmd.Parameters.Add(new SqlParameter("@tipo", ip.Tipo));
                            cmd.Parameters.Add(new SqlParameter("@proyecto", ip.ProyectoId));
                            cmd.Parameters.Add(new SqlParameter("@ejercicio", ip.Ejercicio));
                            cmd.Parameters.Add(new SqlParameter("@estatus", ip.Estatus));

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
                return -1;
            }
        }
        public async Task<int> ActualizarIntegracionProyecto(Integracion integracion)
        {
            int id = -1;
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_actualizaIntegracion", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@id", integracion.Id));
                        cmd.Parameters.Add(new SqlParameter("@usuario", integracion.UsuarioId));
                        cmd.Parameters.Add(new SqlParameter("@evento", integracion.Evento));
                        if (integracion.Capitulo != 0)
                            cmd.Parameters.Add(new SqlParameter("@capitulo", integracion.Capitulo));
                        if (integracion.TipoId != 0)
                            cmd.Parameters.Add(new SqlParameter("@tipoId", integracion.TipoId));
                        if (integracion.Anio != 0)
                            cmd.Parameters.Add(new SqlParameter("@anio", integracion.Anio));
                        if (integracion.Mes != null && !integracion.Mes.Equals(""))
                            cmd.Parameters.Add(new SqlParameter("@mes", integracion.Mes));
                        if (integracion.Tipo != null && !integracion.Tipo.Equals(""))
                            cmd.Parameters.Add(new SqlParameter("@tipo", integracion.Tipo));
                        if (integracion.Importe != 0)
                            cmd.Parameters.Add(new SqlParameter("@importe", integracion.Importe));
                        if (!integracion.Estatus.Equals(""))
                            cmd.Parameters.Add(new SqlParameter("@estatus", integracion.Estatus));
                        if (integracion.TipoProyecto != null && !integracion.TipoProyecto.Equals(""))
                            cmd.Parameters.Add(new SqlParameter("@tipoProyecto", integracion.TipoProyecto));


                        await sql.OpenAsync();
                        id = await cmd.ExecuteNonQueryAsync();

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
        public async Task<int> AutorizarRechazarIntegracionProyecto(Integracion integracion)
        {
            int id = 0;
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_autorizarRechazarIntegracion", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@id", integracion.Id));
                        cmd.Parameters.Add(new SqlParameter("@estatus", integracion.Estatus));

                        await sql.OpenAsync();
                        id = await cmd.ExecuteNonQueryAsync();

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
        public async Task<int> ActualizarOrdenProyectos(List<Integracion> ordenamiento)
        {
            int id = -1;
            try
            {
                foreach (var integracion in ordenamiento)
                {
                    using (SqlConnection sql = new SqlConnection(_connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_actualizaOrdenProyectos", sql))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@id", integracion.Id));
                            cmd.Parameters.Add(new SqlParameter("@clave", integracion.ClaveProyecto));

                            await sql.OpenAsync();
                            int i = await cmd.ExecuteNonQueryAsync();
                            if (i > 0)
                            {
                                id = Convert.ToInt32(cmd.Parameters["@id"].Value);
                            }
                        }
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
    }
}
