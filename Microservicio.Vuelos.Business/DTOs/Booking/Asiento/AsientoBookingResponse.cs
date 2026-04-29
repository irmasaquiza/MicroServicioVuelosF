namespace Microservicio.Vuelos.Business.DTOs.Booking.Asiento
{
    public class AsientoBookingResponse
    {
        public int IdAsiento { get; set; }

        public int IdVuelo { get; set; }

        public string NumeroAsiento { get; set; }

        public string Clase { get; set; }

        public bool Disponible { get; set; }

        public decimal PrecioExtra { get; set; }

        public string Posicion { get; set; }
    }
}