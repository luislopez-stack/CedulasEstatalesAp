using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CedulasEstatalesApi.Cs
{
    public class procesos
    {
        private string constring = "Data Source=10.20.1.8\\PBA_MAPCE;Initial Catalog=MAPCEV2;Persist Security Info=True;User ID=cjimenez;Password=#j1m3n3z-";
        private CedulaEstatalEntities db = new CedulaEstatalEntities();

        public Models.Usuario tomarUsuario()
        {
            Models.Usuario usuario = new Models.Usuario();
            usuario.maxb = 5;
            string filtro = "";
            //VERIFICA CORREO
            try { filtro = HttpContext.Current.Request.Headers.GetValues("correo").First(); } catch (Exception){ filtro = ""; }
            //VERIFICA USUARIO
            string veousu = "0";
            try { veousu = HttpContext.Current.Request.Headers.GetValues("id_usuario").First(); } catch(Exception) { veousu = "0";}
            int id_usuario = int.Parse(veousu);
            //VERIFICA ROL
            veousu = "0";
            try { veousu = HttpContext.Current.Request.Headers.GetValues("perfil").First(); if (veousu == null || veousu.Length == 0) { veousu = "0"; } }
            catch (Exception) { veousu = "0"; }
            int perfil = int.Parse(veousu);

            if (filtro.Length > 0 && id_usuario > 0 && perfil > 0)
            {
                usuario.id_usuario = id_usuario;
                usuario.id_rol = perfil;
                usuario.correo = filtro;
                // Verificar la viabilidad de utilizar Cookies
                string alcance = "0";
                int alkance = 0;
                // Verifica si el Headers contiene alcance
                try { alcance = HttpContext.Current.Request.Headers.GetValues("alcance").First(); } catch (Exception) { alcance = "0"; }
                alkance = int.Parse(alcance);
                usuario.ban = validarUsuarioRol(filtro, perfil, id_usuario, alkance, usuario);
            }
            else
            {
                usuario.id_usuario = 0;
                usuario.id_rol = 0;
                usuario.correo = "";
                usuario.ban = 9;
                usuario.alcances = "";
            }
            return usuario;
        }

        public int validarUsuarioRol(string correoe, int rol, int id, int alcance, Models.Usuario usuario)
        {
            int ban = 0;
            using (SqlConnection conect = new SqlConnection(constring))
            {
                conect.Open();
                // Valida Usuario en BD

                var us = LeeTabla("select a.ID_ROL, a.ID_ALCANCE, b.ID_USUARIO, c.ID_PERSONA, c.CORREO_E, d.DESCRIPCION "
                    + " from CE_USUARIO_ROL as a, CE_CAT_USUARIO as b, PL_PERSONAL as c, CE_CAT_ALCANCE as d "
                    + " where c.CORREO_E = '" + correoe + "' and b.ID_PERSONA = c.ID_PERSONA "
                    + " and a.ID_USUARIO = b.ID_USUARIO and b.ID_USUARIO = " + id
                    + " and a.ID_ROL = " + rol + " and a.ID_ALCANCE = d.ID_ALCANCE and d.ID_ALCANCE = " + alcance, conect);

                if (us == null || us.Rows.Count == 0)
                { ban = 9; usuario.alcances = ""; }
                else
                {
                    usuario.alcances = us.Rows[0].ItemArray[5].ToString();
                    //COINCIDE ROL CON ID_USUARIO
                    ban = verificaRol(rol);
                }
            }
            return ban;
        }

        // Verifica si es Firmante, Escuela o Registros y Controles
        public int verificaRol(int rol)
        {
            int ban = 0;
            switch (rol)
            {
                case 120:
                    //Administrador
                    ban = 5;
                    break;
                case 16:
                    //Otro
                    //ban = 2;
                    //break;
                default:
                    // Rol No Definido por tanto No Válido
                    ban = 9;
                    break;
            }
            return ban;
        }

        public string numeroCedula(long idCedula)
        {
            string idced = "";
            string idCedStrg = idCedula.ToString();
            int leng = idCedStrg.Length;
            switch (leng)
            {
                case 1:
                    idced = "00000" + idCedStrg;
                    break;
                case 2:
                    idced = "0000" + idCedStrg;
                    break;
                case 3:
                    idced = "000" + idCedStrg;
                    break;
                case 4:
                    idced = "00" + idCedStrg;
                    break;
                case 5:
                    idced = "0" + idCedStrg;
                    break;
                case 6:
                    idced = idCedula.ToString();
                    break;
            }
            return idced;
        }
        
        public string tipoCedulaEstatal(long idCedula) {

            string numCedula = "";
            numCedula = "A" + "-" + numeroCedula(idCedula);
            return numCedula;
        }

        public string tipoCedulaFederal(long idCedula)
        {

            string numCedula = "";
            var cedula = db.DOC_CEDULA.Where(t => t.ID_CEDULA == idCedula).FirstOrDefault();
            string tpc = cedula.TIPO_CEDULA;
            string cf = cedula.CEDULA_FEDERAL;
            switch (tpc)
            {
                case "A":
                    numCedula = cf;
                    break;
                case "B":
                    numCedula = "B" + "-" + cf;
                    break;
                default:
                    break;
            }
            return numCedula;
        }

        public string fechanull(DateTime? fecha) {

            string fechaFormart;
            if (fecha != null) {
                fechaFormart = fecha.Value.ToString("dd \\de MMMM \\de yyyy");
                return fechaFormart;
            }

            return fecha.ToString(); ;
        }

        //Lee Tabla Estandar con Sql
        public DataTable LeeTabla(string query, SqlConnection conect)
        {
            DataTable table = new DataTable();
            SqlCommand cmd = new SqlCommand(query, conect);
            SqlDataReader dataread;
            dataread = cmd.ExecuteReader();
            table.Load(dataread);
            dataread.Close();
            dataread.Dispose();
            return table;
        }
    }
}