using System;
using System.Collections.Generic;
using System.Text;

namespace LifeManager.Models
{
    internal class TodoTask
    {
        public int Priority { get; set; } = 1;

        public DateTime? DueDate { get; set; }
    }
}
