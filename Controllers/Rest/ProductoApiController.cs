using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using GameCursos.Data;
using GameCursos.Models;

namespace GameCursos.Controllers.Rest
{
    [ApiController]
    [Route("api/producto")]
    public class ProductoApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductoApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Producto>>> List()
        {
            var productos = await _context.DataProductos.ToListAsync();
            if(productos == null)
                return NotFound();
            return Ok(productos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int? id)
        {
            if (id == null || _context.DataProductos == null)
            {
                return NotFound();
            }

            var producto = await _context.DataProductos.FindAsync(id);
            if(producto == null)
                return NotFound();
            return Ok(producto);
        }
    }
}