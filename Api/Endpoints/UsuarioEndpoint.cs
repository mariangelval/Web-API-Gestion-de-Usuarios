using Microsoft.AspNetCore.Mvc;
namespace Api.Endpoints;
using Models;

//CRUD
public static class UsuarioEndpoints
{
    public static RouteGroupBuilder MapUsuarioEndpoints(this RouteGroupBuilder app)
    {
        //-----------------------------------ENDPOINTS USUARIOS-----------------------------------

        //Hacer lista de usuarios (Test)
        /*List<Usuario> usuarios = [
            new Usuario{Idusuario = 1, Nombre = "Mariangel", Email = "mariangel.valerioet12d1@gmail.com", Username = "mariangelmar", Contrasenia = "1234", Habilitado = true, Fechacreacion = DateTime.Now},
            new Usuario{Idusuario = 2, Nombre = "Iliana", Email = "iliduarte@gmail.com", Username = "iliduarte", Contrasenia = "floricienta", Habilitado = false, Fechacreacion = DateTime.Now},
            new Usuario{Idusuario = 3, Nombre = "Taylor", Email = "taylorswift@gmail.com", Username = "tayswift", Contrasenia = "tay1989", Habilitado = false, Fechacreacion = DateTime.Now}
        ];*/

        //Crear un nuevo usuario
        app.MapPost("/usuario", ([FromBody] Usuario usuario, EscuelaContext context) =>
        {
            // Validar si alguno de los campos del usuario es vacío o null
            if (string.IsNullOrWhiteSpace(usuario.Nombre) ||
                string.IsNullOrWhiteSpace(usuario.Email) ||
                string.IsNullOrWhiteSpace(usuario.Username) ||
                string.IsNullOrWhiteSpace(usuario.Contrasenia))
            {
                return Results.BadRequest();
            }
            usuario.Fechacreacion = DateTime.Now;
            usuario.Habilitado = true;
            context.Usuarios.Add(usuario);
            return Results.Created($"/usuario/{usuario.Idusuario}", usuario);
        })
.WithTags("Usuario");

        // Leer usuarios

        // 1. Leer todos los usuarios
        app.MapGet("/usuarios", (EscuelaContext context) =>
        {
            return Results.Ok(context.Usuarios);
        })
            .WithTags("Usuario");

        // 2. Leer usuario por ID
        app.MapGet("/usuarios/{IdUsuario}", (int IdUsuario, EscuelaContext context) =>
        {
            var usuariobyId = context.Usuarios.FirstOrDefault(usuario => usuario.Idusuario == IdUsuario);
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

        app.MapPut("/usuarios/{IdUsuario}", (int IdUsuario, [FromBody] Usuario usuario, EscuelaContext context) =>
        {
            var usuarioAActualizar = context.Usuarios.FirstOrDefault(usuario => usuario.Idusuario == IdUsuario);

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
            usuarioAActualizar.Contrasenia = usuario.Contrasenia;

            // Devolver 204 No Content si la actualización es exitosa
            return Results.NoContent(); // 204 No Content
        })
        .WithTags("Usuario");


        // Borrar usuarios 

        app.MapDelete("/usuario", ([FromQuery] int IdUsuario, EscuelaContext context) =>
        {
            var usuarioAEliminar = context.Usuarios.FirstOrDefault(usuario => usuario.Idusuario == IdUsuario);
            if (usuarioAEliminar != null)
            {
                context.Usuarios.Remove(usuarioAEliminar);
                return Results.NoContent(); // Código 204
            }
            else
            {
                return Results.NotFound(); // Código 404
            }
        })
        .WithTags("Usuario");

        //Cruzados
        app.MapPost("/usuario/{IdUsuario}/rol/{IdRol}", (int IdUsuario, int IdRol, EscuelaContext context) =>
{
    var usuario = context.Usuarios.FirstOrDefault(usuario => usuario.Idusuario == IdUsuario);
    var rol = context.Rols.FirstOrDefault(rol => rol.Idrol == IdRol);

    if (usuario != null && rol != null)
    {
        // Agregar el rol al usuario
        context.Usuariorols.Add(new Usuariorol { Idrol = IdRol, Idusuario = IdUsuario });
        return Results.Ok();
    }

    return Results.NotFound();
})
    .WithTags("Usuario");


    app.MapDelete("/usuario/{IdUsuario}/rol/{IdRol}", (int IdUsuario, int IdRol, EscuelaContext context) =>
{
    var usuariorol = context.Usuariorols.FirstOrDefault(usuariorol => usuariorol.Idrol == IdRol && usuariorol.Idusuario == IdUsuario);

    if (usuariorol != null && usuariorol != null)
    {
        // Eliminar el rol del usuario
        context.Usuariorols.Remove(usuariorol);
        return Results.Ok();
    }

    return Results.NotFound();
})
    .WithTags("Usuario");


        return app;
    }
}
