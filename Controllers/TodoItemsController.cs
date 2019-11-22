using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoItemsController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            String logSnippet = "[TodoItemsController][GetTodoItems][HttpGet] => ";
            Console.WriteLine("\n" + logSnippet + "(api/TodoItems)");

            return await _context.TodoItems.ToListAsync();
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            String logSnippet = "[TodoItemsController][GetTodoItem][HttpGet] => ";
            Console.WriteLine("\n" + logSnippet + $"(api/TodoItems/{id}");

            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem)
        {
            String logSnippet = "[TodoItemsController][PutTodoItem][HttpPut] => ";
            Console.WriteLine("\n" + logSnippet + $"(id)..................: {id}");
            Console.WriteLine(logSnippet + $"(todoItem.Id).........: {todoItem.Id}");
            Console.WriteLine(logSnippet + $"(todoItem.Name).......: {todoItem.Name}");
            Console.WriteLine(logSnippet + $"(todoItem.IsComplete).: {todoItem.IsComplete}");

            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
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

        // POST: api/TodoItems
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            String logSnippet = "[TodoItemsController][PostTodoItem][HttpPost] => ";
            Console.WriteLine("\n" + logSnippet + "(api/TodoItems)");
            Console.WriteLine(logSnippet + $"(todoItem.Id).........: {todoItem.Id}");
            Console.WriteLine(logSnippet + $"(todoItem.Name).......: {todoItem.Name}");
            Console.WriteLine(logSnippet + $"(todoItem.IsComplete).: {todoItem.IsComplete}");

            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoItem>> DeleteTodoItem(long id)
        {
            String logSnippet = "[TodoItemsController][DeleteTodoItem][HttpDelete] => ";
            Console.WriteLine("\n" + logSnippet + $"(api/TodoItems/{id}");

            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return todoItem;
        }

        private bool TodoItemExists(long id)
        {
            String logSnippet = "[TodoItemsController][TodoItemExists] => ";
            Console.WriteLine("\n" + logSnippet + $"(id): {id}");

            return _context.TodoItems.Any(e => e.Id == id);
        }
    }
}
