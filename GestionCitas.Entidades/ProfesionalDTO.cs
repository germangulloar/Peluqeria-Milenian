using System;
using System.Collections.Generic;

namespace GestionCitas.Entidades
{
    public class ProfesionalDTO
    {
        public Int32 Id { get; set; }
        public String Nombres { get; set; }
        public String Apellidos { get; set; }
        public String Dni { get; set; }
        public String Direccion { get; set; }
        public String Correo { get; set; }
        public String Telefono { get; set; }
        public String Sexo { get; set; }
        public String NumeroColegiatura { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public Boolean Activo { get; set; }
        public List<ProfesionalTipoServicioDTO> ListaTipoServicios { get; set; }//esta es la lista de servicios que tiene el profesional
    }
}
