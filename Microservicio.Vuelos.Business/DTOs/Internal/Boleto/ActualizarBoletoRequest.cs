using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Boleto
{
    public class ActualizarBoletoRequest
    {
        public string? Clase { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? PrecioVueloBase { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? PrecioAsientoExtra { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? ImpuestosBoleto { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? CargoEquipaje { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? PrecioFinal { get; set; }

        public string? EstadoBoleto { get; set; }
    }
}