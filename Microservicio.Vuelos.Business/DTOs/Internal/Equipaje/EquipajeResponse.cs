using System;
using System.Collections.Generic;
using System.Text;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Equipaje
{
    public class EquipajeResponse
    {
        public int IdEquipaje { get; set; }

        public int IdBoleto { get; set; }

        // MANO / BODEGA
        public string Tipo { get; set; } = string.Empty;

        public decimal PesoKg { get; set; }

        public string? DescripcionEquipaje { get; set; }

        public decimal PrecioExtra { get; set; }

        public string? DimensionesCm { get; set; }

        // Generado por la BD — solo lectura
        public string NumeroEtiqueta { get; set; } = string.Empty;

        // REGISTRADO / EMBARCADO / etc
        public string EstadoEquipaje { get; set; } = string.Empty;
    }
}