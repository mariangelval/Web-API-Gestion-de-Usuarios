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

// Leer usuarios

// 1. Leer todos los usuarios
app.MapGet("/usuarios", () =>
{
    return Results.Ok(usuarios);
})
    .WithTags("Usuario");

// 2. Leer usuario por ID
app.MapGet("/usuarios/{IdUsuario}", (int IdUsuario) =>
{
    var usuariobyId = usuarios.FirstOrDefault(usuario => usuario.IdUsuario == IdUsuario);
    if (usuariobyId != null)
    {
        return Results.Ok(usuariobyId); //Codigo 200
    }
    else
    {
        return Results.NotFound(); //Codigo 404
    }
})
    .WithTags("Usuario");


// Actualizar usuarios

app.MapPut("/usuario", ([FromQuery] int IdUsuario, [FromBody] Usuario usuario) =>
{
    var usuarioAActualizar = usuarios.FirstOrDefault(alumno => usuario.IdUsuario == IdUsuario);
    
    // Verificar si el usuario existe
    if (usuarioAActualizar == null)
    {
        return Results.NotFound(); // 404 Not Found
    }
    
    // Verificar si se está intentando modificar el nombre
    if (usuario.Nombre != null)
    {
        return Results.BadRequest(); // 400 Bad Request
    }
    
    // Modificar las propiedades del usuario (excepto el nombre)
    usuarioAActualizar.Email = usuario.Email; 
    usuarioAActualizar.Username = usuario.Username;
    usuarioAActualizar.Contrasena = usuario.Contrasena;
    
    // Devolver 204 No Content si la actualización es exitosa
    return Results.NoContent(); // 204 No Content
})
.WithTags("Alumno");



app.Run();
