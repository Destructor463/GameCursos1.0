using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameCursos.DTO
{
    public class TodoDTO
    {
        public int userId { get; set; }
        public int id { get; set; }
        public string? title { get; set; }
        public bool completed { get; set; }
    }
}