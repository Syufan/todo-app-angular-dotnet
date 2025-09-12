using Server.Domain;
namespace Server.Application;
public interface ITodoRepository
{   // Read Operation - Get all todos
    IReadOnlyList<TodoItem> List();

    // Write Operation - Create new todo
    TodoItem Add(string title);

    // Write Operation - Delete by ID, returns success/failure
    bool Delete(int id);
}
