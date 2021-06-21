using System;

namespace Persistencia.DapperConexion.Instructor
{
    public class InstructorModel
    {
        public Guid InstructorId { get; set; }
        public string NombreInstructor { get; set; }
        public string Apellidos { get; set; }
        public string Titulo { get; set; }
        
    }
}