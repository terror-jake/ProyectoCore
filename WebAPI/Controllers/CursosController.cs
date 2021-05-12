using System.Collections.Generic;
using System.Threading.Tasks;
using Aplicacion.Cursos;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // http://localhost:5000/api/Cursos
    public class CursosController : MiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<CursoDTO>>> Get()
        {
            return await Mediator.Send(new Consulta.ListaCursos());
        }

        [HttpGet("{id}")] // http://localhost:5000/api/Cursos/Guid
        public async Task<ActionResult<CursoDTO>> Detalle(Guid id)
        {
            return await Mediator.Send(new ConsultaId.CursoUnico{Id = id}  );
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data)
        {
            return await Mediator.Send(data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Editar(Guid id, Editar.Ejecuta data)
        {
            data.CursoId = id;
            return await Mediator.Send(data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Eliminar(Guid id)
        {
            return await Mediator.Send(new Eliminar.Ejecuta{Id = id});
        }

    }   
}