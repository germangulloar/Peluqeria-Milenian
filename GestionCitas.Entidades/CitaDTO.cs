using System;

namespace GestionCitas.Entidades
{
    public class CitaDTO
    {
        public Int32 Id { get; set; }
        public Int32 ProfesionalId { get; set; }
        public String Profesional { get; set; }
        public Int32 ClienteId { get; set; }
        public String ClienteNombres { get; set; }
        public String ClienteApellidos { get; set; }
        public String ClienteDni { get; set; }
        public String Estado { get; set; }
        public String Observaciones { get; set; }
        public DateTime FechaAtencion { get; set; }
        public DateTime FechaAtencionInicio { get; set; }
        public DateTime FechaAtencionFin { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public Boolean Activo { get; set; }
    }
}
