using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Models.Models;

namespace Todo.Models.Dtos.Requests
{
    public class TodoRequestDto
    {
        #region Constructor
        public TodoRequestDto(string? Title)
        {
            this.Title = Title;
        }
        #endregion

        public string? Title { get; set; }

        #region Implicit Operator Convert
        public static implicit operator TodoDto?(TodoRequestDto? todoTask)
        {
            if (todoTask is null)
                return null;

            return new TodoDto(0, todoTask.Title);
        }
        #endregion
    }
}
