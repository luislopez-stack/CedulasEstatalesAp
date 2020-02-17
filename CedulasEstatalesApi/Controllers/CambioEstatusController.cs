using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using CedulasEstatalesApi;

namespace CedulasEstatalesApi.Controllers
{
    public class CambioEstatusController : ApiController
    {
        private CedulaEstatalEntities db = new CedulaEstatalEntities();
        private Cs.procesos rolUsuario = new Cs.procesos();

        // GET: api/CambioEstatus
        public IQueryable<CAT_ESTATUS> GetCAT_ESTATUS()
        {
            return db.CAT_ESTATUS;
        }

        // GET: api/CambioEstatus/5
        [ResponseType(typeof(CAT_ESTATUS))]
        public IHttpActionResult GetCAT_ESTATUS(byte id)
        {
            CAT_ESTATUS cAT_ESTATUS = db.CAT_ESTATUS.Find(id);
            /*if (cAT_ESTATUS == null)
            {
                return NotFound();
            }*/

            return Ok(cAT_ESTATUS);
        }

        // PUT: api/CambioEstatus/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCAT_ESTATUS(byte id, CAT_ESTATUS cAT_ESTATUS)
        {
            /*if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cAT_ESTATUS.ID_ESTATUS)
            {
                return BadRequest();
            }

            db.Entry(cAT_ESTATUS).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CAT_ESTATUSExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }*/

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/CambioEstatus
        [ResponseType(typeof(CAT_ESTATUS))]
        public IHttpActionResult PostEstatus(Models.cambioEstatusModel cAT_ESTATUS)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //VALIDAR USUARIO
            var usuario = rolUsuario.tomarUsuario();
            int ban = usuario.ban;
            string filtro = usuario.correo;
            int id_usuario = usuario.id_usuario;
            int perfil = usuario.id_rol;
            string alcances = usuario.alcances;
            int maxb = usuario.maxb;

            if (ban <= maxb)
            {
                long idCedula = Int64.Parse(cAT_ESTATUS.ID_CEDULA.Substring(2));
                var cedula = db.DOC_CEDULA.Where(e => e.ID_CEDULA == idCedula).FirstOrDefault();
                if (cedula == null)
                {
                    return BadRequest("ERROR: La cedula: " + cAT_ESTATUS.ID_CEDULA + " no existe.");
                }
                byte estEnviar = 2;
                cedula.ID_ESTATUS = estEnviar;
                db.Entry(cedula).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return BadRequest("ERROR: Al actualizar estatus " + ex.ToString());
                }

                return Ok(cedula.TIPO_CEDULA + "-" + cedula.ID_CEDULA);
            }

            return BadRequest("El Correo Electrónico // o Id de usuario // o Perfil Es incorrecto... ");
        }

        // DELETE: api/CambioEstatus/5
        [ResponseType(typeof(CAT_ESTATUS))]
        public IHttpActionResult DeleteCAT_ESTATUS(byte id)
        {
            CAT_ESTATUS cAT_ESTATUS = db.CAT_ESTATUS.Find(id);
            /*if (cAT_ESTATUS == null)
            {
                return NotFound();
            }

            db.CAT_ESTATUS.Remove(cAT_ESTATUS);
            db.SaveChanges();
            */
            return Ok(cAT_ESTATUS);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CAT_ESTATUSExists(byte id)
        {
            return db.CAT_ESTATUS.Count(e => e.ID_ESTATUS == id) > 0;
        }
    }
}