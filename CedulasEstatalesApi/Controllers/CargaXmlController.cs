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
    public class CargaXmlController : ApiController
    {
        private CedulaEstatalEntities db = new CedulaEstatalEntities();
        Models.cargaTituloXml cargaTituloXml = new Models.cargaTituloXml();

        // GET: api/CargaXml
        /*public IQueryable<DOC_CEDULA> GetDOC_CEDULA()
        {
            return db.DOC_CEDULA;
        }*/

        // GET: api/CargaXml/5
        /*[ResponseType(typeof(DOC_CEDULA))]
        public IHttpActionResult GetDOC_CEDULA(long id)
        {
            DOC_CEDULA dOC_CEDULA = db.DOC_CEDULA.Find(id);
            if (dOC_CEDULA == null)
            {
                return NotFound();
            }

            return Ok(dOC_CEDULA);
        }*/

        // PUT: api/CargaXml/5
        /*[ResponseType(typeof(void))]
        public IHttpActionResult PutDOC_CEDULA(long id, DOC_CEDULA dOC_CEDULA)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dOC_CEDULA.ID_CEDULA)
            {
                return BadRequest();
            }

            db.Entry(dOC_CEDULA).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DOC_CEDULAExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }*/

        // POST: api/CargaXml
        public IHttpActionResult Post(Models.cargaTituloXml cargaXml)
        {
            //MANDAR A LLAMAR FUNCION DESEARIZAR XML
            Cs.deserealizarXml deserealizarXml = new Cs.deserealizarXml();
            var camposXml = deserealizarXml.deserealizar(cargaXml.ARCHIVO_XML);

            return Ok(camposXml);
        }

        // DELETE: api/CargaXml/5
        /*[ResponseType(typeof(DOC_CEDULA))]
        public IHttpActionResult DeleteDOC_CEDULA(long id)
        {
            DOC_CEDULA dOC_CEDULA = db.DOC_CEDULA.Find(id);
            if (dOC_CEDULA == null)
            {
                return NotFound();
            }

            db.DOC_CEDULA.Remove(dOC_CEDULA);
            db.SaveChanges();

            return Ok(dOC_CEDULA);
        }*/

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DOC_CEDULAExists(long id)
        {
            return db.DOC_CEDULA.Count(e => e.ID_CEDULA == id) > 0;
        }
    }
}