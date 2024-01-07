using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Models.Models;

namespace Todo.Models.Dtos
{
    public class TodoDto
    {
        #region Constructor
        public TodoDto(int Id, string? Title, DateTime? DueBy = null, bool IsComplete = false)
        {
            this.Id = Id;
            this.Title = Title;
            this.DueBy = DueBy;
            this.IsComplete = IsComplete;
        }
        #endregion

        public int Id { get; set; }
        public string? Title { get; set; }
        public DateTime? DueBy { get; set; } = null;
        public bool IsComplete { get; set; } = false;

        #region Implicit Operator Convert
        public static implicit operator TodoDto?(TodoTask todoTask)
        {
            if (todoTask is null)
                return null;

            return new TodoDto(todoTask.Id, todoTask.Title, todoTask.DueBy, todoTask.IsComplete);
        }
        #endregion
    }
}
