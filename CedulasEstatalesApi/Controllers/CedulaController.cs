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
    public class CedulaController : ApiController
    {
        private CedulaEstatalEntities db = new CedulaEstatalEntities();
        private Cs.procesos rolUsuario = new Cs.procesos();

        // GET: api/Cedula
        public List<Models.camposXml> GetDOC_CEDULA()
        {

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
                var dOC_CEDULA = db.DOC_CEDULA;
                List<Models.camposXml> listaQr = (from DOC_CEDULA item in dOC_CEDULA.AsEnumerable()
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
                                                      CEDULAFEDERAL = item.CEDULA_FEDERAL,
                                                      CEDULAESTATAL = item.TIPO_CEDULA + "-" + item.ID_CEDULA,
                                                      //XML = db.XML.Where(x => x.ID_CEDULA == item.ID_CEDULA).FirstOrDefault().XML1,
                                                      SELLO = item.SELLO,
                                                      HASH = item.HASH_QR,
                                                  }).ToList();
                return listaQr;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "El Correo Electrónico // o Id de usuario // o Perfil Es incorrecto... ");
                return null;
            }


        }

        // GET: api/Cedula/HASH
        [ResponseType(typeof(Models.camposXml))]
        public Models.camposXml GetDOC_CEDULA(string id)
        {
            //DOC_CEDULA item = db.DOC_CEDULA.Find(id);
            var item = db.DOC_CEDULA.Where(h => h.HASH_QR == id).FirstOrDefault(); ;
            Models.camposXml registro = new Models.camposXml();
            if (item == null)
            {
                registro.NOMBRE = "La cedula no se encuentra almacenada";
            }
            else
            {
                registro.NOMBRE = item.NOMBRES;
                registro.PRIMERAPELLIDO = item.PRIMER_APELLIDO;
                registro.SEGUNDOAPELLIDO = item.SEGUNDO_APELLIDO;
                registro.CURP = item.CURP;
                registro.CVECARRERA = item.ID_CARRERA.ToString();
                registro.NOMBRECARRERA = item.DESC_CARRERA;
                registro.NOMBREINSTITUCION = item.INSTITUCION;
                registro.CEDULAESTATAL = item.TIPO_CEDULA + "-" + item.ID_CEDULA;
                registro.CEDULAFEDERAL = item.CEDULA_FEDERAL;
                registro.SELLO = item.SELLO;
            }
            return registro;
        }

        // PUT: api/Cedula/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDOC_CEDULA(long id, DOC_CEDULA dOC_CEDULA)
        {
            /*if (!ModelState.IsValid)
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
            }*/

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Cedula
        [ResponseType(typeof(DOC_CEDULA))]
        public IHttpActionResult PostCedula(Models.camposXml camposXml)
        {
            long idCedula;
            string fechaCarga;
            byte idFirmante;
            string tipoCedula;
            string cedulaF;
            Cs.sellado sellado = new Cs.sellado();

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
                if (camposXml.CEDULAFEDERAL == null || camposXml.CEDULAFEDERAL == "") { cedulaF = "0000000"; } else { cedulaF = camposXml.CEDULAFEDERAL; }


                //////CREAR Y CARGAR CEDULA
                DOC_CEDULA dOC_CEDULA = (new DOC_CEDULA()
                {
                    CURP = camposXml.CURP,
                    NOMBRES = camposXml.NOMBRE,
                    PRIMER_APELLIDO = camposXml.PRIMERAPELLIDO,
                    SEGUNDO_APELLIDO = camposXml.SEGUNDOAPELLIDO,
                    INSTITUCION = camposXml.NOMBREINSTITUCION,
                    ID_CARRERA = Int32.Parse(camposXml.CVECARRERA),
                    DESC_CARRERA = camposXml.NOMBRECARRERA,

                    TIPO_CEDULA = "A",
                    ID_ESTATUS = 1,
                    FECHA_CARGA = DateTime.Now,
                    FECHA_SELLO = DateTime.Now,
                    CEDULA_FEDERAL = cedulaF,
                    SELLO = "",
                    HASH_QR = "",
                    ID_FIRMANTE = 1,
                });

                db.DOC_CEDULA.Add(dOC_CEDULA);
                try
                {
                    db.SaveChanges();
                    var urlId = CreatedAtRoute("DefaultApi", new { id = dOC_CEDULA.ID_CEDULA }, dOC_CEDULA);
                    idCedula = urlId.Content.ID_CEDULA;
                    var urlFechaS = CreatedAtRoute("DefaultApi", new { id = dOC_CEDULA.FECHA_SELLO }, dOC_CEDULA);
                    fechaCarga = (urlFechaS.Content.FECHA_SELLO).ToString();
                    var urlFirmante = CreatedAtRoute("DefaultApi", new { id = dOC_CEDULA.ID_FIRMANTE }, dOC_CEDULA);
                    idFirmante = (byte)urlFirmante.Content.ID_FIRMANTE;
                    var urlTipoCedula = CreatedAtRoute("DefaultApi", new { id = dOC_CEDULA.TIPO_CEDULA }, dOC_CEDULA);
                    tipoCedula = urlFirmante.Content.TIPO_CEDULA;
                }
                catch (Exception ex)
                {
                    return BadRequest("ERROR: Al grabar cedula " + ex.ToString());
                }

                //////PROSESO DE SELLADO
                string idCedE = tipoCedula + "-" + ((idCedula).ToString());
                string cadenaOriginal = sellado.cadenaOriginal(camposXml, idCedE, cedulaF, fechaCarga, idFirmante);
                string sello = sellado.crearSello(cadenaOriginal, idFirmante);
                string selloSubstring = sello.Substring(0, 5);
                if (selloSubstring == "Error")
                {
                    return BadRequest(sello);
                }
                /////CREACION DE HASH
                string hashQr = sellado.stringSha265(idCedE);
                /////COMPLETAN LOS CAMPOS FALTANTES DE DOC_CEDULA
                var cedula = db.DOC_CEDULA.Where(c => c.ID_CEDULA == idCedula).FirstOrDefault();
                cedula.SELLO = sello;
                cedula.HASH_QR = hashQr;
                db.Entry(cedula).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return BadRequest("ERROR: Al actualizar sello " + ex.ToString());
                }

                /////ALMACENA XML
                XML xML = (new XML()
                {
                    ID_CEDULA = idCedula,
                    XML1 = camposXml.XML,
                    
                });
                db.XML.Add(xML);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex) { BadRequest("ERROR: Al grabar xml " + ex.ToString()); }

                //////ALMACENAMOS EN BITACORA
                BITACORA bITACORA = (new BITACORA()
                {
                    ID_CEDULA = idCedula,
                    FECHA = DateTime.Now,
                    TIPO_MOVIMIENTO = 1,
                    ID_USUARIO = id_usuario,
                });
                db.BITACORA.Add(bITACORA);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex) { BadRequest("ERROR: Al grabar bitacora " + ex.ToString()); }

                //return CreatedAtRoute("DefaultApi", new { id = dOC_CEDULA.ID_CEDULA }, dOC_CEDULA);
                return Ok("Cedula: " +idCedE+ " creada");
            }
            else
            {
                return BadRequest("El Correo Electrónico // o Id de usuario // o Perfil Es incorrecto... ");
            }
        }

        // DELETE: api/Cedula/5
        [ResponseType(typeof(DOC_CEDULA))]
        public IHttpActionResult DeleteDOC_CEDULA(long id)
        {
            DOC_CEDULA dOC_CEDULA = db.DOC_CEDULA.Find(id);
            /*if (dOC_CEDULA == null)
            {
                return NotFound();
            }

            db.DOC_CEDULA.Remove(dOC_CEDULA);
            db.SaveChanges();*/

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