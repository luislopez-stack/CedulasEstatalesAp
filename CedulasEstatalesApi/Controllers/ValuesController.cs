using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CedulasEstatalesApi.Controllers
{
    public class ValuesController : ApiController   
    {
        private CedulaEstatalEntities db = new CedulaEstatalEntities();

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public IHttpActionResult Post(Models.cargaTituloXml value)
        {
            //POST MOMENTANEO
            //MANDAR A LLAMAR FUNCION DESEARIZAR XML
            Cs.deserealizarXml deserealizarXml = new Cs.deserealizarXml();
            //Models.cargaTituloXml cargaTituloXml = new Models.cargaTituloXml();
            byte[] archivo = value.ARCHIVO_XML;
            // nombre = value.NOMBRE;
            var camposXml = deserealizarXml.deserealizar(archivo);

            return Ok(camposXml);
        }

        // PUT api/values/5
        public IHttpActionResult Put(int id, Models.cargaTituloXml archivoKey)
        {
             ///////AGREGAR .KEY A DB
            /*var firmanteKey = db.CAT_FIRMANTE.Where(f => f.ID_FIRMANTE == id).FirstOrDefault();
            firmanteKey.LLAVE = archivoKey.ARCHIVO_XML;
            db.Entry(firmanteKey).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest("ERROR: Al actualizar sello " + ex.ToString());
            }*/
            return Ok();
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
