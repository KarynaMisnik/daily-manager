using System;
using System.Collections.Generic;
using System.Text;

namespace LifeManager
{
    public class TodoTask
    {
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }

        public int CategoryId { get; set; }

        public int Id { get; set; }

        public int Priority { get; set; } = 1;
        public DateTime? DueDate { get; set; }
    }
}
