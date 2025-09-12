using Microsoft.AspNetCore.Mvc;
using Server.Application;
using Server.Contracts;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class TodoController : ControllerBase
{
    // Dependency Injection
    private readonly ITodoRepository _repo;

    public TodoController(ITodoRepository repo) => _repo = repo;

    // List All Todos
    [HttpGet]
    public ActionResult<IEnumerable<TodoItemDto>> GetTodos()
    {
        var items = _repo.List();
        return Ok(items.Select(x => x.ToDto()));
    }

    // Create Todo
    [HttpPost]
    public ActionResult<TodoItemDto> AddTodo([FromBody] CreateTodoRequest request)
    {
        var created = _repo.Add(request.Title!);
        var dto = created.ToDto();

        return CreatedAtAction(nameof(GetTodos), new { }, dto);
    }

    // Remove Todo
    [HttpDelete("{id:int}")]
    public IActionResult DeleteTodo(int id)
    {
        var ok = _repo.Delete(id);
        return ok ? NoContent() : NotFound();
    }
}
