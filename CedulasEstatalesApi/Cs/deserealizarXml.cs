using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;

namespace CedulasEstatalesApi.Cs
{
    public class deserealizarXml
    {

        public Models.camposXml deserealizar(byte[] archivo)
        {
            string stgArchivo;
            Models.camposXml camposXml = new Models.camposXml();

            ////CONVERTIR ARCHIVO EN STRING
            var ms = new MemoryStream(archivo);
            using (var read = new StreamReader(ms))
            {
                stgArchivo = read.ReadToEnd();
            }

            ////RECORER XML 
            byte[] encodedStrin = Encoding.UTF8.GetBytes(stgArchivo.ToString());
            MemoryStream me = new MemoryStream(encodedStrin);
            me.Flush();
            me.Position = 0;
            
            XElement element = XElement.Load(me);

            foreach (var node in element.Descendants().Where(r => r.Attribute("correoElectronico") != null))
            {
                camposXml.NOMBRE = node.Attribute("nombre").Value.ToString();
                camposXml.PRIMERAPELLIDO = node.Attribute("primerApellido").Value.ToString();
                camposXml.SEGUNDOAPELLIDO = node.Attribute("segundoApellido").Value.ToString();
                camposXml.CURP = node.Attribute("curp").Value.ToString();

            }

            foreach (var node in element.Descendants().Where(r => r.Attribute("cveInstitucion") != null))
            {
                camposXml.NOMBREINSTITUCION = node.Attribute("nombreInstitucion").Value.ToString();
            }

            foreach (var node in element.Descendants().Where(r => r.Attribute("cveCarrera") != null))
            {
                camposXml.CVECARRERA = node.Attribute("cveCarrera").Value.ToString();
                camposXml.NOMBRECARRERA = node.Attribute("nombreCarrera").Value.ToString();
            }

            if (camposXml.NOMBRE == null || camposXml.NOMBREINSTITUCION == null || camposXml.CVECARRERA == null)
            {
                camposXml.NOMBRE = "El archivo cargado es incopatible con el formato de titulo digital";
            }
            camposXml.XML = stgArchivo;
            return camposXml;

        }
    }

}