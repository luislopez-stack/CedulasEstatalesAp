using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CedulasEstatalesApi.Controllers
{
    public class ValuesController : ApiController
    {
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
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
