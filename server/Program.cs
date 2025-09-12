// server/Program.cs
using Server.Setup;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApiBasics(builder.Configuration)   // 控制器、ProblemDetails、CORS、HttpLogging、Swagger
    .AddTodoInfrastructure()               // 你的仓储等基础设施
    .AddCorrelationId();                   // 关联ID中间件

var app = builder.Build();

app.UseAppPipeline();                      // HTTPS/HSTS、异常处理、关联ID、HttpLogging、CORS、Swagger、路由/健康检查

app.Run();

public partial class Program { }
