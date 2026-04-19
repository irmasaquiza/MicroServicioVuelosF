using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Equipaje
{
    public class CrearEquipajeRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int IdBoleto { get; set; }

        // MANO / BODEGA
        [Required]
        public string Tipo { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal PesoKg { get; set; }

        public string? DescripcionEquipaje { get; set; }

        [Range(0, double.MaxValue)]
        public decimal PrecioExtra { get; set; } = 0;

        public string? DimensionesCm { get; set; }
    }
}