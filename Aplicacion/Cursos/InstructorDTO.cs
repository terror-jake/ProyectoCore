using System;

namespace Aplicacion.Cursos
{
    public class InstructorDTO
    {
        public Guid InstructorId { get; set; }
        public string NombreInstructor { get; set; }
        public string Apellidos { get; set; }
        public string Grado { get; set; }
        public byte[] FotoPerfil { get; set; }
        public DateTime? FechaCreacion { get; set; }

    }
}