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
    public class CedulaFiltroController : ApiController
    {
        private CedulaEstatalEntities db = new CedulaEstatalEntities();

        // GET: api/CedulaFiltro
        public IQueryable<DOC_CEDULA> GetDOC_CEDULA()
        {
            return db.DOC_CEDULA;
        }

        // GET: api/CedulaFiltro/5
        [ResponseType(typeof(DOC_CEDULA))]
        public List<Models.camposXml> GetDOC_CEDULA(string id)
        {
            string curp="";
            int opc=0;
            var query = "";
            DateTime fechaini = new DateTime();
            DateTime fechafin = new DateTime();
            if (id.Length <= 18) { curp = (id.ToString()).Substring(0, 18); opc = 1; }
            else {
                // Rango de Fechas... 
                opc = 2;
                string fecha = (id.ToString()).Substring(10);

                var fecha1 = (id.ToString()).Replace('-','/').Substring(0, 10);
                fechaini = System.DateTime.Parse(fecha1);
                var fecha2 = (id.ToString()).Replace('-','/').Substring(10);
                fechafin = System.DateTime.Parse(fecha2);
            }



            switch (opc)
            {
                case 1:
                    query = "select * from DOC_CEDULA where CURP = '" + curp + "'";
                    break;
                case 2:
                    query = "select * from DOC_CEDULA where FECHA_SELLO >= '"+ fechaini.ToString("yyyy-dd-MM") + " 00:00:00" + "'" 
                        + " AND FECHA_SELLO <= '" + fechafin.ToString("yyyy-dd-MM") + " 23:00:00" + "'";
                    break;
            }

            var filtro = db.DOC_CEDULA.SqlQuery(query);
            List<Models.camposXml> list = (from DOC_CEDULA item in filtro.AsEnumerable()
                                           select new Models.camposXml
                                           {
                                               NOMBRE = item.NOMBRES,
                                               PRIMERAPELLIDO = item.PRIMER_APELLIDO,
                                               SEGUNDOAPELLIDO = item.SEGUNDO_APELLIDO,
                                               CURP = item.CURP,
                                               CVECARRERA = item.ID_CARRERA.ToString(),
                                               NOMBRECARRERA = item.DESC_CARRERA,
                                               NOMBREINSTITUCION = item.INSTITUCION,
                                               ESTATUS = item.ID_ESTATUS,
                                               CEDULAESTATAL = item.TIPO_CEDULA+"-"+(item.ID_CEDULA.ToString()),
                                               CEDULAFEDERAL = item.CEDULA_FEDERAL,
                                               SELLO = item.SELLO,
                                               HASH = item.HASH_QR,
                                           }).ToList();
            return list;

            /*if (dOC_CEDULA == null)
            {
                return NotFound();
            }

            return Ok(dOC_CEDULA);*/
        }

        // PUT: api/CedulaFiltro/5
        [ResponseType(typeof(void))]
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
        }

        // POST: api/CedulaFiltro
        [ResponseType(typeof(DOC_CEDULA))]
        public IHttpActionResult PostDOC_CEDULA(DOC_CEDULA dOC_CEDULA)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DOC_CEDULA.Add(dOC_CEDULA);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = dOC_CEDULA.ID_CEDULA }, dOC_CEDULA);
        }

        // DELETE: api/CedulaFiltro/5
        [ResponseType(typeof(DOC_CEDULA))]
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
        }

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