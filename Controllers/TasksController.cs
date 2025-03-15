using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApi.Data;
using TaskApi.Models;

namespace TaskApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TaskDbContext _context;

        public TasksController(TaskDbContext context)
        {
            _context = context;
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskApi.Models.Task>>> GetTasks([FromQuery] string? filtro = null)
{
    IQueryable<TaskApi.Models.Task> query = _context.Tasks;
    
    if (!string.IsNullOrEmpty(filtro))
    {
        switch (filtro.ToLower())
        {
            case "pendientes":
                query = query.Where(t => t.Status == EstadoTarea.Pendiente);
                break;
            case "completadas":
                query = query.Where(t => t.Status == EstadoTarea.Completado);
                break;
        }
    }
    
    return await query.ToListAsync();
}

        // GET: api/Tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskApi.Models.Task>> GetTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return task;
        }

        // POST: api/Tasks
        [HttpPost]
        public async Task<ActionResult<TaskApi.Models.Task>> CreateTask(TaskApi.Models.Task task)
        {
            task.Created_at = DateTime.Now;
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        // PUT: api/Tasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskApi.Models.Task task)
        {
            if (id != task.Id)
            {
                return BadRequest();
            }

            _context.Entry(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // PUT: api/Tasks/5/estado
        [HttpPut("{id}/estado")]
        public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] EstadoTarea nuevoEstado)
        {
            var task = await _context.Tasks.FindAsync(id);
            
            if (task == null)
            {
                return NotFound();
            }

            task.Status = nuevoEstado;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
