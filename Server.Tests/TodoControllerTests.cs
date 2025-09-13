using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Server.Application;
using Server.Contracts;
using Server.Controllers;
using Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using Xunit;

public class TodoControllerTests
{
    private static ITodoRepository Repo() => new InMemoryTodoRepository();

    [Fact]
    public void Get_ReturnsOkWithList()
    {
        var repo = Repo();
        var sut  = new TodoController(repo);

        var act =  sut.GetTodos();

        var ok  = act.Result as OkObjectResult;
        ok.Should().NotBeNull();
        ok.StatusCode.Should().Be(200); 
        var list = ok.Value as IEnumerable<TodoItemDto>;
        list.Should().BeEmpty();
    }

    [Fact]
    public void Get_ReturnsListWithItems()
    {
        var repo = Repo();
        var created = repo.Add("Buy eggs");
        var sut = new TodoController(repo);

        var act = sut.GetTodos();
        var ok = act.Result as OkObjectResult;

        ok.Should().NotBeNull();
        ok!.StatusCode.Should().Be(200);
        var list = ok.Value as IEnumerable<TodoItemDto>;
        list.Should().ContainSingle(x => x.Id == created.Id && x.Title == "Buy eggs");
    }

    private static void ValidateModel(object model, ControllerBase controller)
    {
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(model, context, results, validateAllProperties: true);

        foreach (var vr in results)
            controller.ModelState.AddModelError(vr.MemberNames.FirstOrDefault() ?? string.Empty, vr.ErrorMessage!);
    }

    [Fact]
    public void AddTodo_WithEmptyTitle_ReturnsBadRequest()
    {
        var sut = new TodoController(Repo());
        var req = new CreateTodoRequest { Title = "   " };

        var result = sut.AddTodo(req);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(400, badRequest.StatusCode);
    }

    [Fact]
    public void AddTodo_WithMissingTitle_ShouldFailModelValidation()
    {
        var sut = new TodoController(Repo());
        var req = new CreateTodoRequest { Title = null };

        ValidateModel(req, sut);

        var result = sut.AddTodo(req);
        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        badRequest.StatusCode.Should().Be(400);
    }

    [Fact]
    public void Post_ValidTitle_ReturnsCreated()
    {
        var sut = new TodoController(Repo());
        var req = new CreateTodoRequest { Title = "Buy milk" };

        var act = sut.AddTodo(req);
        var created = act.Result as CreatedAtActionResult;

        created.Should().NotBeNull();
        created!.StatusCode.Should().Be(201);
        created.Value.Should().BeOfType<TodoItemDto>();
    }

    [Fact]
    public void Delete_Existing_ReturnsNoContent()
    {
        var repo = Repo();
        var sut  = new TodoController(repo);

        var created = repo.Add("X");
        var act = sut.DeleteTodo(created.Id);

        var result = act as NoContentResult;
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(204);
    }

    [Fact]
    public void Delete_Missing_ReturnsNotFound()
    {
        var sut = new TodoController(Repo());

        var act = sut.DeleteTodo(999);

        var notFound = act as NotFoundResult;
        notFound.Should().NotBeNull();
        notFound!.StatusCode.Should().Be(404);
    }
}