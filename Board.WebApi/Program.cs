using Board.WebApi.ExceptionHandlers;
using Board.WebApi.Extensions;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.AddServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Uploads")),
    RequestPath = "/Uploads"
});

app.UseExceptionHandler(new ExceptionHandlerOptions
{
    ExceptionHandler = app.Services.GetRequiredService<ExceptionHandler>().Invoke
});

app.UseRouting();
app.MapControllers();
app.Run();
