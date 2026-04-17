using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace Microservicio.Vuelos.DataManagement.Models
{
    public class RolDataModel
    {
        public int IdRol { get; set; }

        public Guid RolGuid { get; set; }

        public string NombreRol { get; set; }
        public string DescripcionRol { get; set; }

        public string EstadoRol { get; set; }

        public bool Activo { get; set; }
    }
}