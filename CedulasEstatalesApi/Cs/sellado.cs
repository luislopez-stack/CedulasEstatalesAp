using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using uuIdClass;

namespace CedulasEstatalesApi.Cs
{
    public class sellado
    {
        private CedulaEstatalEntities db = new CedulaEstatalEntities();

        public string cadenaOriginal(Models.camposXml camposXml, string idCedulaE, string idCedulaF, string fechaS, byte idFirmante)
        {
            var firmante = db.CAT_FIRMANTE.Find(idFirmante);
            var uuId = Uuid.NewSqlSequentialId();
            var cedulaE = Uuid.NewNamespaceUuid(uuId, idCedulaE);
            var cedulaF = Uuid.NewNamespaceUuid(uuId, idCedulaF);

            string cadena = "||1.0|";

            cadena += cedulaE + "|";
            cadena += cedulaF + "|";
            cadena += camposXml.CURP + "|";
            cadena += camposXml.NOMBRE + "|";
            cadena += camposXml.PRIMERAPELLIDO + "|";
            cadena += camposXml.SEGUNDOAPELLIDO + "|";
            cadena += fechaS + "|";
            cadena += firmante.CERTIFICADO + "|";    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
            cadena += firmante.NO_CERTIFICADO + "|"; //00001000000502102401
            cadena += "|";
            return cadena;
        }

        public string crearSello(string cadenaOriginal, byte idFirmante)
        {
            string sellodigital = "";
            var firmante = db.CAT_FIRMANTE.Find(idFirmante);
            //byte[] byteKey = firmante.KEY;
            try
            {
                //string archivoCertificado = "";
                //string key = @"C:\Users\Luis\Documents\MiTitulo\FIEL_AULJ951012PZ7_20190328160816\Claveprivada_FIEL_SOLR761006UN5_20191112_125540.key";
                string key = @"C:\CedulasEstatales\key\Claveprivada_FIEL_SOLR761006UN5_20191112_125540.key";
                
                string Password = firmante.PASSWORD;
                string strCadenaOriginal = cadenaOriginal;
                SecureString identidad = new SecureString();// Se requerira un objeto SecureString que represente el password de la clave privada, que se obtiene asi:
                identidad.Clear();
                //foreach (char c in lPassword.ToCharArray())
                foreach (char c in Password.ToCharArray())
                {
                    identidad.AppendChar(c);
                }

                //Byte[] llavePrivada = byteKey;
                Byte[] llavePrivada = System.IO.File.ReadAllBytes(key);
                RSACryptoServiceProvider rsa = JavaScience.opensslkey.DecodeEncryptedPrivateKeyInfo(llavePrivada, identidad);// Uso de la clase opensslkey
                SHA1CryptoServiceProvider hasher = new SHA1CryptoServiceProvider();
                Byte[] bytesFirmados = rsa.SignData(System.Text.Encoding.UTF8.GetBytes(strCadenaOriginal), hasher);
                sellodigital = Convert.ToBase64String(bytesFirmados);// Obtengo Sello

            }
            catch (Exception ex) { return sellodigital = "Error al sellar. La contraseña o el archivo .key parecen ser incorrectos. " + ex; }

            return sellodigital;
        }


        public string stringSha265(string noCedula)
        {
            HashAlgorithm hashAlgorithm = SHA256.Create();
            byte[] bytes = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(noCedula));

            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < bytes.Length; i++)
                sb.Append(bytes[i].ToString("x2"));

            return sb.ToString();
        }

    }
}