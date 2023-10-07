using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GameCursos.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GameCursos.Data;
using System.Dynamic;

namespace GameCursos1.Controllers
{
    
    public class CarritoController : Controller
    {
         private readonly ILogger<CarritoController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CarritoController(ILogger<CarritoController> logger,
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userIDSession = _userManager.GetUserName(User);

            //select proforma pr, producto p WHERE join ....And pr.user = usuario sesionado
            var items = from o in _context.DataCarrito select o;
            items = items.Include(p => p.Producto).
                Where(w => w.UserID.Equals(userIDSession) &&
                    w.Status.Equals("PENDIENTE")
                    );
            var itemsCarrito = items.ToList();
            //Fila1 1234, Shampo; Precio, Cantidad
            //Fila2 12345, Shampo3; Precio, Cantidad
            var total = itemsCarrito.Sum(c => c.Cantidad * c.Precio);
            //MEMORIA
            dynamic model = new ExpandoObject();
            model.montoTotal = total;
            model.elementosCarrito = itemsCarrito;
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemproforma = await _context.DataCarrito.FindAsync(id);
            if (itemproforma == null)
            {
                return NotFound();
            }
            return View(itemproforma);
        }

    

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemcarrito = await _context.DataCarrito.FindAsync(id);
            _context.DataCarrito.Remove(itemcarrito);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}