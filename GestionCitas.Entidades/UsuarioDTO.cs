using System;

namespace GestionCitas.Entidades
{
    public class UsuarioDTO
    {
        public Int32 Id { get; set; }
        public String Nombre { get; set; }
        public String Clave { get; set; }
        public Int32 ProfesionalId { get; set; }
        public String ProfesionalNombres { get; set; }
        public String ProfesionalApellidos { get; set; }
        public Boolean Activo { get; set; }
    }
}
