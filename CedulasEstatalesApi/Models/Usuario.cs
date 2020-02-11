using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CedulasEstatalesApi.Models
{
    public class Usuario
    {
        public int id_usuario { get; set; }
        public int id_rol { get; set; }
        public string correo { get; set; }
        public int ban { get; set; }
        public string alcances { get; set; }
        public int maxb { get; set; }
    }
}