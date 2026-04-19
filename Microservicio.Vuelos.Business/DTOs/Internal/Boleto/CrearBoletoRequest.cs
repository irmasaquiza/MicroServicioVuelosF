using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Boleto
{
    public class CrearBoletoRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int IdReserva { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int IdVuelo { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int IdAsiento { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int IdFactura { get; set; }

        [Required]
        public string Clase { get; set; } = "ECONOMICA";

        [Required]
        [Range(0, double.MaxValue)]
        public decimal PrecioVueloBase { get; set; }

        [Range(0, double.MaxValue)]
        public decimal PrecioAsientoExtra { get; set; } = 0;

        [Range(0, double.MaxValue)]
        public decimal ImpuestosBoleto { get; set; } = 0;

        [Range(0, double.MaxValue)]
        public decimal CargoEquipaje { get; set; } = 0;

        [Required]
        [Range(0, double.MaxValue)]
        public decimal PrecioFinal { get; set; }
    }
}