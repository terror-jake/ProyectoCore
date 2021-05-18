using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Persistencia.DapperConexion.Instructor;
using Aplicacion.Instructores;

namespace WebAPI.Controllers
{
    public class InstructoresController : MiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<InstructorModel>>> ObtenerInstructores()
        {
            return await Mediator.Send(new Consulta.Lista());
        }
    }
}