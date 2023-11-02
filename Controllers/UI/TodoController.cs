using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GameCursos.DTO;
using GameCursos.Integrations;

namespace GameCursos.Controllers.UI
{
    public class TodoController : Controller
    {
        private readonly ILogger<TodoController> _logger;
        private readonly JsonplaceholderAPIIntegration _jsonplaceholder;

        public TodoController(ILogger<TodoController> logger,JsonplaceholderAPIIntegration jsonplaceholder)
        {
            _logger = logger;
            _jsonplaceholder = jsonplaceholder;
        }

        public async Task<IActionResult> Index()
        {

            List<TodoDTO> todos =await _jsonplaceholder.GetAll();

            //List<TodoDTO> filtro = todos.Where(todo => todo.userId > 6).ToList();

            //List<TodoDTO> filtro = todos.Where(todo => todo.title.Contains("Tarea")).ToList();

            List<TodoDTO> filtro = todos
            .Where(todo => todo.title.Contains("provident") && todo.id > 121)
            .OrderBy(todo => todo.title)
            .ThenByDescending(todo => todo.completed)
            .ToList();

            return View(todos);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}