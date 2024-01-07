using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Models.Models;

namespace Todo.Data.Mappers.TodoSqlServerMappers
{
    internal class TodoTaskMap : IEntityTypeConfiguration<TodoTask>
    {
        public void Configure(EntityTypeBuilder<TodoTask> builder)
        {
            builder.HasKey(e => e.Id);
            builder.ToTable("TodoTask");
            builder.Property(e => e.UserId).HasColumnName("UserId");
            builder.Property(e => e.DueBy).HasColumnType("date");
            builder.Property(e => e.IsComplete).HasColumnName("IsComplete");
            builder.Property(e => e.Title).HasColumnName("Title");
        }
    }
}
