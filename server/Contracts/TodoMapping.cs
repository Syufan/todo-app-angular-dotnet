namespace Server.Contracts;
using Server.Domain;

public static class TodoMapping
{
    public static TodoItemDto ToDto(this TodoItem item)
        => new(item.Id, item.Title);
}
