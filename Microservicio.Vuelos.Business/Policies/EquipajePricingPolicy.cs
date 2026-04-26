using System;

namespace Microservicio.Vuelos.Business.Policies
{
    public static class EquipajePricingPolicy
    {
        public static decimal CalcularPrecio(string tipo, decimal pesoKg)
        {
            tipo = tipo?.ToUpper() ?? "";

            // 🧳 Equipaje de mano
            if (tipo == "MANO")
            {
                if (pesoKg <= 10)
                    return 0;
            }

            // 🧳 Equipaje de bodega
            if (tipo == "BODEGA")
            {
                if (pesoKg <= 23)
                    return 25;

                // exceso de peso
                var exceso = pesoKg - 23;
                return 25 + (exceso * 3);
            }

            // 🔥 default
            return 0;
        }
    }
}
