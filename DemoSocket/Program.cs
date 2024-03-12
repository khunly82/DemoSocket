using DemoSocket.Hubs;
using DemoSocket.Infrastructure.Security;
using Microsoft.AspNetCore.Http.Connections;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSignalR();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<TokenManager>();
builder.Services.AddScoped<JwtSecurityTokenHandler>();
builder.Services.AddSingleton(
    builder.Configuration.GetSection("Jwt").Get<TokenManager.Config>() 
    ?? throw new Exception("Jwt config is missing")
);

builder.Services.AddCors(
    o => o.AddDefaultPolicy(b =>
        b.WithOrigins("http://localhost:4200")
        .AllowAnyHeader().AllowCredentials().AllowAnyMethod()
    )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/ws/chat", o =>
{
    o.Transports = HttpTransportType.WebSockets;
});

app.Run();
