using Microsoft.EntityFrameworkCore;
using TaskApi.Models;

namespace TaskApi.Data
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options)
        {
        }

        public DbSet<TaskApi.Models.Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Datos de ejemplo
            modelBuilder.Entity<TaskApi.Models.Task>().HasData(
                new TaskApi.Models.Task { 
                    Id = -1, 
                    Tittle = "Aprender .NET", 
                    Description = "Estudiar conceptos básicos de .NET", 
                    Status = EstadoTarea.Pendiente, 
                    Created_at = DateTime.Now 
                },
                new TaskApi.Models.Task { 
                    Id = -2, 
                    Tittle = "Crear API RESTful", 
                    Description = "Desarrollar una API para gestionar tareas", 
                    Status = EstadoTarea.Completado, 
                    Created_at = DateTime.Now 
                }
            );
        }
    }
}