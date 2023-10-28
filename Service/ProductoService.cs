using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using GameCursos.Models;
using GameCursos.Data;

namespace GameCursos.Service
{
    public class ProductoService
    {
        private readonly ILogger<ProductoService> _logger;
        private readonly ApplicationDbContext _context;

        public ProductoService(ILogger<ProductoService> logger,
            ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

       public async Task<Producto> CreateOrUpdate(Producto p)
        {
            // Regla de Negocio 1
            if (p.Precio < 1)
            {
                throw new SystemException("No se puede ingresar datos con precio menor a 1 sol");
            }

            // Verificar si se está creando o actualizando un producto
            if (p.Id == 0) // Suponiendo que 0 representa un nuevo producto
            {
                // Se está creando un nuevo producto
                _context.Add(p);
            }
            else
            {
                // Se está actualizando un producto existente
                _context.Update(p);
            }

            await _context.SaveChangesAsync();
            return p;
        }

        public async Task<List<Producto>?> GetAll(){
            if(_context.DataProductos == null )
                return null;
            return await _context.DataProductos.ToListAsync();
        }

        public async Task<Producto?> Get(int? id){
            if (id == null || _context.DataProductos == null)
            {
                return null;
            }

            var producto = await _context.DataProductos.FindAsync(id);
            if (producto == null)
            {
                return null;
            }
            return producto;
        }

        public async Task<Producto?> FirstOrDefault(int? id){
            var producto = await _context.DataProductos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return null;
            }
            return producto;
        }

        public async Task Delete(int? id)
{
    var producto = await _context.DataProductos.FindAsync(id);
    if (producto != null)
    {
        // Eliminar registros relacionados en la tabla "t_carrito"
        var carritosRelacionados = _context.DataCarrito.Where(c => c.Producto.Id == id);
        _context.DataCarrito.RemoveRange(carritosRelacionados);
        _context.DataProductos.Remove(producto);
    }
    await _context.SaveChangesAsync();
}

        public bool ProductoExists(int id)
        {
            return (_context.DataProductos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}