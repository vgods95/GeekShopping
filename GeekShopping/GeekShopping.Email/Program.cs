using GeekShopping.Email.Model.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connection = "Server=localhost;DataBase=geek_shopping_email;Uid=root;Pwd=#Helpdesk2013;Port=3306";

builder.Services.AddDbContext<MySQLContext>(op => op
    .UseMySql(connection,
    new MySqlServerVersion(
        new Version(8, 0, 25)
    )));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
