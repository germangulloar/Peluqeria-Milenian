using System;

namespace GestionCitas.Entidades
{
    public class ProfesionalTipoServicioDTO
    {
        public Int32 Id { get; set; }
        public Int32 TipoServicioId { get; set; }
        public String TipoServicio { get; set; }
        public String TipoServicioDescripcion { get; set; }
        public Boolean Activo { get; set; }
    }
}
