using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Models.Dtos;

namespace Todo.Models.Models
{
    public class TodoTask
    {
        #region Constructor
        public TodoTask(int Id, string? Title, DateTime? DueBy = null, bool IsComplete = false)
        {
            this.Id = Id;
            this.Title = Title;
            this.DueBy = DueBy;
            this.IsComplete = IsComplete;
            this.UserId = 0;
        }
        #endregion

        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Title { get; set; }
        public DateTime? DueBy { get; set; } = null;
        public bool IsComplete { get; set; } = false;

        #region Implicit operator convert
        public static implicit operator TodoTask?(TodoDto? todoDto)
        {
            if (todoDto is null)
                return null;

            return new TodoTask(todoDto.Id, todoDto.Title, todoDto.DueBy, todoDto.IsComplete);
        }
        #endregion
    }
}
