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
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Role).HasColumnName("Role");
            builder.Property(e => e.Username).HasColumnName("Username");
            builder.Property(e => e.Password).HasColumnName("Password");
        }
    }
}
