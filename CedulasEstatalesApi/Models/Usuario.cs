using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Titulosf.Models
{
    public class Usuario
    {
        public int id_usuario { get; set; }
        public int id_rol { get; set; }
        public string correo { get; set; }
        public int ban { get; set; }
        public string alcances { get; set; }
        public int maxb { get; set; }
        public string cvct { get; set; }
    }
}