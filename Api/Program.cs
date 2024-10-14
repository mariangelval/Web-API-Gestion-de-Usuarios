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

//-----------------------------------ENDPOINTS USUARIOS-----------------------------------

//Hacer lista de usuarios (Test)
List<Usuario> usuarios = [
    new Usuario{IdUsuario = 1, Nombre = "Mariangel", Email = "mariangel.valerioet12d1@gmail.com", Username = "mariangelmar", Contrasena = "1234", Habilitado = true, FechaCreacion = DateTime.Now},
    new Usuario{IdUsuario = 2, Nombre = "Iliana", Email = "iliduarte@gmail.com", Username = "iliduarte", Contrasena = "floricienta", Habilitado = false, FechaCreacion = DateTime.Now},
    new Usuario{IdUsuario = 3, Nombre = "Taylor", Email = "taylorswift@gmail.com", Username = "tayswift", Contrasena = "tay1989", Habilitado = false, FechaCreacion = DateTime.Now}
];

//CRUD

//Crear un nuevo usuario
app.MapPost("/usuario", ([FromBody] Usuario usuario) =>
{
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
})
.WithTags("Usuario");

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

app.MapPut("/usuarios/{IdUsuario}", (int IdUsuario, [FromBody] Usuario usuario) =>
{
    var usuarioAActualizar = usuarios.FirstOrDefault(usuario => usuario.IdUsuario == IdUsuario);

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
.WithTags("Usuario");


// Borrar usuarios 

app.MapDelete("/usuario", ([FromQuery] int IdUsuario) =>
{
    var usuarioAEliminar = usuarios.FirstOrDefault(usuario => usuario.IdUsuario == IdUsuario);
    if (usuarioAEliminar != null)
    {
        usuarios.Remove(usuarioAEliminar);
        return Results.NoContent(); // Código 204
    }
    else
    {
        return Results.NotFound(); // Código 404
    }
})
.WithTags("Usuario");

//-----------------------------------ENDPOINTS ROLES-----------------------------------

//Hacer lista de roles (Test)
List<Rol> roles = [
    new Rol{IdRol = 1, Nombre = "Preceptor", Habilitado = true, FechaCreacion= DateTime.Now},
    new Rol{IdRol = 2, Nombre = "Rector", Habilitado = true, FechaCreacion= DateTime.Now},
    new Rol{IdRol = 3, Nombre = "Profesor", Habilitado = true, FechaCreacion= DateTime.Now},
    new Rol{IdRol = 4, Nombre = "Alumno", Habilitado = true, FechaCreacion= DateTime.Now}
];

// 1. Crear un nuevo Rol
app.MapPost("/rol", ([FromBody] Rol rol) =>
{
    // Validar si alguno de los campos del usuario es vacío o null
    if (string.IsNullOrWhiteSpace(rol.Nombre))
    {
        return Results.BadRequest();
    }
    rol.FechaCreacion = DateTime.Now;
    rol.Habilitado = true;
    roles.Add(rol);
    return Results.Created($"/usuario/{rol.IdRol}", rol);

})
.WithTags("Rol");

// 2. a. Ver todos los datos de todos los roles
app.MapGet("/roles", () =>
{
    return Results.Ok(roles);
})
    .WithTags("Rol");

// b.  Mostrar rol por ID
app.MapGet("/roles/{IdRol}", (int IdRol) =>
{
    var rolbyId = roles.FirstOrDefault(rol => rol.IdRol == IdRol);
    if (rolbyId != null)
    {
        return Results.Ok(rolbyId); //Codigo 200
    }
    else
    {
        return Results.NotFound(); //Codigo 404
    }
})
    .WithTags("Rol");


// 3. Modificar Rol excepto el nombre
app.MapPut("/rol", ([FromQuery] int IdRol, [FromBody] Rol usuario) =>
{
    var rolAActualizar = usuarios.FirstOrDefault(alumno => usuario.IdRol == IdRol);

    // Verificar si el rol existe
    if (rolAActualizar == null)
    {
        return Results.NotFound(); // 404 Not Found
    }
    // Verificar si se está intentando modificar el nombre
    if (usuario.Nombre != null)
    {
        return Results.BadRequest(); // 400 Bad Request
    }
    // Devolver 204 No Content si la actualización es exitosa
    return Results.NoContent(); // 204 No Content
})
.WithTags("Rol");

// 4. Borrar por ID

app.MapDelete("/rol", ([FromQuery] int IdRol) =>
{
    var rolAEliminar = roles.FirstOrDefault(rol => rol.IdRol == IdRol);
    if (rolAEliminar != null)
    {
        roles.Remove(rolAEliminar);
        return Results.NoContent(); // Código 204
    }
    else
    {
        return Results.NotFound(); // Código 404
    }
})
.WithTags("Rol");

/* Usuario-Rol Rol-Usuario
app.MapPost("/rol/{IdRol}/usuario/{IdUsuario}", (int IdRol, int IdUsuario) =>
{
    var rol = roles.FirstOrDefault(rol => rol.IdRol == IdRol);
    var usuario = usuarios.FirstOrDefault(usuario => usuario.IdUsuario == IdUsuario);

    if (usuario != null && rol != null)
    {
        //alumno.Cursos.Add(curso);
        rol.usuarios.Add(usuario);
        return Results.Ok();
    }

    return Results.NotFound();
})
    .WithTags("Rol");

app.MapDelete("/rol/{IdRol}/usuario/{IdUsuario}", (int IdRol, int IdUsuario) =>
{
    var rol = roles.FirstOrDefault(rol => rol.IdRol == IdRol);
    var usuario = usuarios.FirstOrDefault(usuario => usuario.IdUsuario == IdUsuario);

    if (usuario != null && rol != null)
    {
        // Eliminar el usuario del rol
        rol.usuarios.Remove(usuario);
        return Results.Ok();
    }

    return Results.NotFound();
})
    .WithTags("Rol");

app.MapPost("/usuario/{IdUsuario}/rol/{IdRol}", (int IdUsuario, int IdRol) =>
{
    var usuario = usuarios.FirstOrDefault(usuario => usuario.IdUsuario == IdUsuario);
    var rol = roles.FirstOrDefault(rol => rol.IdRol == IdRol);

    if (usuario != null && rol != null)
    {
        // Agregar el rol al usuario
        usuario.Roles.Add(rol);
        return Results.Ok();
    }

    return Results.NotFound();
})
    .WithTags("Usuario");

app.MapDelete("/usuario/{IdUsuario}/rol/{IdRol}", (int IdUsuario, int IdRol) =>
{
    var usuario = usuarios.FirstOrDefault(usuario => usuario.IdUsuario == IdUsuario);
    var rol = roles.FirstOrDefault(rol => rol.IdRol == IdRol);

    if (usuario != null && rol != null)
    {
        // Eliminar el rol del usuario
        usuario.Roles.Remove(rol);
        return Results.Ok();
    }

    return Results.NotFound();
})
    .WithTags("Usuario");*/

app.Run();
