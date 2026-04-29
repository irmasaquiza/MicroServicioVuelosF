namespace Microservicio.Vuelos.Business.DTOs.Booking.Boleto
{
    public class CrearBoletoBookingRequest
    {
        public int IdReserva { get; set; }

        public int IdVuelo { get; set; }

        public int IdAsiento { get; set; }

        public int IdFactura { get; set; }

        public string Clase { get; set; }
    }
}