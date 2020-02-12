using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CedulasEstatalesApi.Models
{
    public class camposXml
    {
        public string NOMBRE { get; set; }
        public string PRIMERAPELLIDO { get; set; }
        public string SEGUNDOAPELLIDO { get; set; }
        public string CURP { get; set; }
        public string NOMBREINSTITUCION { get; set; }
        public string CVECARRERA { get; set; }
        public string NOMBRECARRERA { get; set; }
       
        public string CEDULAESTATAL { get; set; }
        public string CEDULAFEDERAL { get; set; }
        public byte? ESTATUS { get; set; }
        public string SELLO { get; set; }
        public string HASH { get; set; }
        public string XML { get; set; }
    }
}