/*
using System;
using System.Collections.Generic;

namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class TipoMetodoPagoEntity
    {
        public int IdTipoMetodo { get; set; }

        public string NombreTipo { get; set; } = null!;     // ✔ obligatorio
        public string? Descripcion { get; set; }            // ✔ nullable

        public string Estado { get; set; } = null!;         // ✔ ACTIVO / INACTIVO
        public bool EsEliminado { get; set; }

        // 🔗 Relaciones
        public virtual ICollection<MetodoPagoEntity> MetodosPago { get; set; } = new List<MetodoPagoEntity>();
    }
}*/