using Microsoft.AspNetCore.Mvc;
using Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//------------------------------------------------ ENDPOINTS ---------------------------------------------

//Hacer lista de usuarios (Test)
List<Usuario> usuarios = [
    new Usuario{IdUsuario = 1, Nombre = "Mariangel", Email = "mariangel.valerioet12d1@gmail.com", Username = "mariangelmar", Contrasena = "1234", Habilitado = true, FechaCreacion = DateTime.Now},
    new Usuario{IdUsuario = 2, Nombre = "Iliana", Email = "iliduarte@gmail.com", Username = "iliduarte", Contrasena = "floricienta", Habilitado = false, FechaCreacion = DateTime.Now},
    new Usuario{IdUsuario = 3, Nombre = "Taylor", Email = "taylorswift@gmail.com", Username = "tayswift", Contrasena = "tay1989", Habilitado = false, FechaCreacion = DateTime.Now}
];

//CRUD

//Crear un nuevo usuario
app.MapPost("/usuario", ([FromBody] Usuario usuario) => {
    // Validar si alguno de los campos del usuario es vacío o null
    if (string.IsNullOrWhiteSpace(usuario.Nombre) ||
        string.IsNullOrWhiteSpace(usuario.Email) ||
        string.IsNullOrWhiteSpace(usuario.Username) ||
        string.IsNullOrWhiteSpace(usuario.Contrasena))
    {
        return Results.BadRequest();
    }
    usuario.FechaCreacion = DateTime.Now;
    usuario.Habilitado = true;
    usuarios.Add(usuario);
    return Results.Created($"/usuario/{usuario.IdUsuario}", usuario);
});

// 

app.Run();
