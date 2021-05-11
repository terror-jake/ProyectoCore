using System;
using Dominio;

namespace Aplicacion.Cursos
{
    public class CursoInstructorDTO
    {
        public Guid InstructorId { get; set; }
        public Curso Curso { get; set; }
    }
}