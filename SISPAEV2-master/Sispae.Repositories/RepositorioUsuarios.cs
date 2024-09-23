using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Sispae.Entities.MLogin;
using Sispae.Entities.MUsuarios;
using Sispae.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Sispae.Repositories
{
    public class RepositorioUsuarios: IRepositorioUsuarios
    {
        private readonly string _connectionString;
        private readonly IRepositorioAreas vRepositorioAreas;

        public RepositorioUsuarios(IConfiguration configuration, IRepositorioAreas iRepositorioAreas)
        {
            _connectionString = configuration.GetConnectionString("DatabaseConnection");
            this.vRepositorioAreas = iRepositorioAreas ?? throw new ArgumentNullException(nameof(iRepositorioAreas));
        }

        //Método para Obtener los Usuarios
        public async Task<List<Usuarios>> getUsuarios()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_getUsuarios", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var response = new List<Usuarios>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToValueUsuarios(reader));
                        }
                    }

                    return response;
                }
            }
        }

        //Método para Obtener un usuario especifico
        public async Task<Usuarios> getUserById(int id)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_getUserById", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    Usuarios response = null;
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response = MapToValueUsuarios(reader);
                        }
                    }
                    return response;
                }
            }
        }

        //Inserta usuario en el sistema
        public async Task<int> insertaUsuario(string datosUsuario, string password)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(datosUsuario);

            XmlNodeList list_dg = doc.SelectNodes("//DatosGenerales");
            XmlNodeList list_areas = doc.SelectNodes("//Áreas");

            string jsonDatosGen = JsonConvert.SerializeXmlNode(list_dg[0], Newtonsoft.Json.Formatting.None, true);
            string jsonAreas = JsonConvert.SerializeXmlNode(list_areas[0], Newtonsoft.Json.Formatting.None, true);

            Usuarios user = JsonConvert.DeserializeObject<Usuarios>(jsonDatosGen);
            Areas area = JsonConvert.DeserializeObject<Areas>(jsonAreas);

            //Definimos la variable con el ID del area que se registrara...
            int areaID = await vRepositorioAreas.insertaArea(area);

            int id = 0;
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_insertaUsuario", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.BigInt)).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(new SqlParameter("@areaId", areaID));
                        cmd.Parameters.Add(new SqlParameter("@expediente", user.exp));
                        cmd.Parameters.Add(new SqlParameter("@usuario", user.cve_usuario));
                        cmd.Parameters.Add(new SqlParameter("@password", password));
                        cmd.Parameters.Add(new SqlParameter("@nombre", user.nombre_emp));
                        cmd.Parameters.Add(new SqlParameter("@apellidoPat", user.paterno_emp));
                        cmd.Parameters.Add(new SqlParameter("@apellidoMat", user.materno_emp));
                        cmd.Parameters.Add(new SqlParameter("@rfc", user.rfc_emp));
                        cmd.Parameters.Add(new SqlParameter("@curp", user.curp_emp));
                        cmd.Parameters.Add(new SqlParameter("@cvePuesto", user.cve_puesto));
                        cmd.Parameters.Add(new SqlParameter("@puesto", user.nom_pue));
                        if (!String.IsNullOrEmpty(user.correo_electronico))
                        {
                            cmd.Parameters.Add(new SqlParameter("@email", user.correo_electronico));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@email", "N/A"));
                        }
                       cmd.Parameters.Add(new SqlParameter("@nomCat", user.nom_cat));
                       cmd.Parameters.Add(new SqlParameter("@intentos", user.intentos));
                       cmd.Parameters.Add(new SqlParameter("@vigenciaPass", user.fch_vig_pswd));

                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        id = Convert.ToInt32(cmd.Parameters["@id"].Value);

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

        //Obtenemos los inmuebles por usuaio
        public async Task<List<UnidadesUsuarios>> getUnidadesUsuario(int user)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getUnidadesUsuario", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@user", user));
                        var response = new List<UnidadesUsuarios>();
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
            catch(Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }

        public async Task<int> insertaUEGByUser(List<UnidadesUsuarios> uegUsuario)
        {
            try
            {

                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    foreach (var uegUsr in uegUsuario)
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_insertaUnidadByUser", sql))
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@usuario", uegUsr.UsuarioId));
                            cmd.Parameters.Add(new SqlParameter("@unidad", uegUsr.UnidadId));
                            await sql.OpenAsync();
                            await cmd.ExecuteNonQueryAsync();
                            await sql.CloseAsync();
                        }
                    }
                    return 1;
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public async Task<int> EliminaUnidadByUser(int unidad, int user)
        {
            try
            {

                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_eliminaUnidadByUser", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@unidad", unidad));
                        cmd.Parameters.Add(new SqlParameter("@usuario", user));
                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                        await sql.CloseAsync();
                        return 1;
                    }

                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public async Task<int> asignaPerfil(List<PerfilesUsuario> perfilesUsuarios)
        {
            try
            {

                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    foreach (var pu in perfilesUsuarios)
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_insertaPerfilByUser", sql))
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@usuario", pu.UsuarioId));
                            cmd.Parameters.Add(new SqlParameter("@perfil", pu.PerfilId));
                            await sql.OpenAsync();
                            await cmd.ExecuteNonQueryAsync();
                            await sql.CloseAsync();
                        }
                    }
                    return 1;
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }
        public async Task<int> actualizaCorreoElectronico(Usuarios usuarios)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_actualizaEmail", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@id", usuarios.Id));
                        cmd.Parameters.Add(new SqlParameter("@email", usuarios.correo_electronico));
                        await sql.OpenAsync();
                        int i = await cmd.ExecuteNonQueryAsync();

                        return i == 1 ? i : -1;
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return -1;
            }
        }

        private UnidadesUsuarios MapToValue(SqlDataReader reader)
        {
            return new UnidadesUsuarios()
            {
                Id = reader["Id"] != DBNull.Value ? (int) reader["Id"]:0,
                Clave = reader["NumeroUEG"] != DBNull.Value ? (int)reader["NumeroUEG"] :0,
                Nombre = reader["Nombre"] != DBNull.Value ? reader["Nombre"].ToString():"",
                Descripcion = reader["Nombre"] != DBNull.Value ? reader["Descripcion"].ToString():"",
            };
        }
        private Usuarios MapToValueUsuarios(SqlDataReader reader)
        {
            return new Usuarios()
            {
                Id = (int)reader["ID"],
                AreaId = (int)reader["AreaId"],
                exp = (int)reader["Expediente"],
                cve_usuario = reader["Usuario"].ToString(),
                nom_cat = reader["NomCat"].ToString(),
                nombre_emp = reader["NombreEmp"].ToString(),
                paterno_emp = reader["PaternoEmp"].ToString(),
                materno_emp = reader["MaternoEmp"].ToString(),
                cve_puesto = reader["ClavePuesto"].ToString(),
                nom_pue = reader["Puesto"].ToString(),
                rfc_emp = reader["RFC"].ToString(),
                curp_emp = reader["CURP"].ToString(),
                correo_electronico = reader["CorreoElectronico"].ToString()
            };
        }

    }
}
