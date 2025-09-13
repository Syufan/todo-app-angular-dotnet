using FluentAssertions;
using Server.Infrastructure;
using Xunit;

public class InMemoryTodoRepositoryTests
{
    [Fact]
    public void List_InitiallyEmpty_ReturnsEmpty()
    {
        var repo = new InMemoryTodoRepository();
        var list = repo.List();
        list.Should().BeEmpty();
    }

    [Fact]
    public void Add_AssignsIncrementingIds_AndTrims()
    {
        var repo = new InMemoryTodoRepository();
        var a = repo.Add("  A  ");
        var b = repo.Add("B");

        a.Id.Should().Be(1);
        a.Title.Should().Be("A");
        b.Id.Should().Be(2);
    }

    [Fact]
    public void Delete_WhenExists_ReturnsTrueAndRemoves()
    {
        var repo = new InMemoryTodoRepository();
        var item = repo.Add("X");

        var ok = repo.Delete(item.Id);
        ok.Should().BeTrue();

        var list = repo.List();
        list.Should().BeEmpty();
    }

    [Fact]
    public void Delete_WhenMissing_ReturnsFalse()
    {
        var repo = new InMemoryTodoRepository();
        (repo.Delete(42)).Should().BeFalse();
    }

    [Fact]
    public void List_ReturnsSortedById()
    {
        var repo = new InMemoryTodoRepository();
        var _ = repo.Add("B");
        var __ = repo.Add("A");

        var list = repo.List();
        list.Select(x => x.Id).Should().BeInAscendingOrder();
    }

    [Fact]
    public async Task Add_Parallel_GeneratesUniqueIncreasingIds()
    {
        var repo = new InMemoryTodoRepository();

        var tasks = Enumerable.Range(0, 200)
            .Select(i => Task.Run(() => repo.Add($"T{i}")));

        var items = await Task.WhenAll(tasks);

        items.Select(x => x.Id).Should().OnlyHaveUniqueItems();
        items.Select(x => x.Id).Min().Should().BeGreaterOrEqualTo(1);
    }
}