using System;

namespace GestionCitas.Entidades
{
    public class HorarioDTO
    {
        public Int32 Id { get; set; }
        public Int32 ProfesionalId { get; set; }
        public DateTime FechaAtencion { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public Boolean Activo { get; set; }
        public object ProfesnionalId { get; set; }
    }
}
