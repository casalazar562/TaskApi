using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskApi.Models
{
    public class Task
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        public string Tittle { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
        
        public EstadoTarea Status { get; set; } = EstadoTarea.Pendiente;
        
        public DateTime Created_at { get; set; }
    }
    
    public enum EstadoTarea
    {
        Pendiente,
        Completado
    }

    



}