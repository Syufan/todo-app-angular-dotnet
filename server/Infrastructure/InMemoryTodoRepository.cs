using System.Collections.Concurrent;
using Server.Application;
using Server.Domain;

namespace Server.Infrastructure;

public sealed class InMemoryTodoRepository : ITodoRepository
{
    // Thread-Safe Storage
    private readonly ConcurrentDictionary<int, TodoItem> _items = new();
    private int _lastId = 0;

    // Read Operation
    public IReadOnlyList<TodoItem> List() =>
        _items.Values.OrderBy(x => x.Id).ToList();

    // Create Operation
    public TodoItem Add(string title)
    {
        var id = Interlocked.Increment(ref _lastId);
        var item = new TodoItem(id, title.Trim());
        _items[id] = item;
        return item;
    }

    // Delete Operation
    public bool Delete(int id) =>
        _items.TryRemove(id, out _);
}
