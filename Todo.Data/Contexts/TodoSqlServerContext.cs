using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Data.Mappers.TodoSqlServerMappers;
using Todo.Models.Models;

namespace Todo.Data.Contexts
{
    public class TodoSqlServerContext : DbContext
    {
        #region Constructor
        public TodoSqlServerContext(DbContextOptions<TodoSqlServerContext> options) : base(options)
        {
        }
        #endregion

        #region DbSets
        public DbSet<User> User { get; set; }
        public DbSet<TodoTask> TodoTask { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");
            #region Map Context Models
            modelBuilder.ApplyConfiguration(new TodoTaskMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            #endregion

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableDetailedErrors()
                .EnableServiceProviderCaching()
                .EnableSensitiveDataLogging();

            base.OnConfiguring(optionsBuilder);
        }
    }
}
