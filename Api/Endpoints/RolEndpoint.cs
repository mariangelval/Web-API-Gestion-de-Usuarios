using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;
using Models;
public static class RolEndpoints
{
    public static RouteGroupBuilder MapRolEndpoints(this RouteGroupBuilder app)
    {
        //-----------------------------------ENDPOINTS ROLES-----------------------------------

        //Hacer lista de roles (Test)
        /*List<Rol> roles = [
            new Rol{Idrol = 1, Nombre = "Preceptor", Habilitado = true, Fechacreacion= DateTime.Now},
            new Rol{Idrol = 2, Nombre = "Rector", Habilitado = true, Fechacreacion= DateTime.Now},
            new Rol{Idrol = 3, Nombre = "Profesor", Habilitado = true, Fechacreacion= DateTime.Now},
            new Rol{Idrol = 4, Nombre = "Alumno", Habilitado = true, Fechacreacion= DateTime.Now}
        ];*/
        // 1. Crear un nuevo Rol
        app.MapPost("/rol", ([FromBody] Rol rol, EscuelaContext context) =>
        {
            // Validar si alguno de los campos del usuario es vacío o null
            if (string.IsNullOrWhiteSpace(rol.Nombre))
            {
                return Results.BadRequest();
            }
            rol.Fechacreacion = DateTime.Now;
            rol.Habilitado = true;
            context.Rols.Add(rol);
            return Results.Created($"/usuario/{rol.Idrol}", rol);

        })
        .WithTags("Rol");

        // 2. a. Ver todos los datos de todos los roles
        app.MapGet("/roles", (EscuelaContext context) =>
        {
            return Results.Ok(context.Rols);
        })
            .WithTags("Rol");

        // b.  Mostrar rol por ID
        app.MapGet("/roles/{IdRol}", (int IdRol, EscuelaContext context) =>
        {
            var rolbyId = context.Rols.FirstOrDefault(rol => rol.Idrol == IdRol);
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
        app.MapPut("/rol", ([FromQuery] int IdRol, [FromBody] Rol usuario, EscuelaContext context) =>
        {
            var rolAActualizar = context.Rols.FirstOrDefault(alumno => usuario.Idrol == IdRol);

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

        app.MapDelete("/rol", ([FromQuery] int IdRol, EscuelaContext context) =>
        {
            var rolAEliminar = context.Rols.FirstOrDefault(rol => rol.Idrol == IdRol);
            if (rolAEliminar != null)
            {
                context.Rols.Remove(rolAEliminar);
                return Results.NoContent(); // Código 204
            }
            else
            {
                return Results.NotFound(); // Código 404
            }
        })
        .WithTags("Rol");

        // Cruzados
        app.MapPost("/rol/{IdRol}/usuario/{IdUsuario}", (int IdRol, int IdUsuario, EscuelaContext context) =>
{
    var rol = context.Rols.FirstOrDefault(rol => rol.Idrol == IdRol);
    var usuario = context.Usuarios.FirstOrDefault(usuario => usuario.Idusuario == IdUsuario);

    if (usuario != null && rol != null)
    {
        //alumno.Cursos.Add(curso);
        context.Usuariorols.Add(new Usuariorol { Idrol = IdRol, Idusuario = IdUsuario });
        return Results.Ok();
    }

    return Results.NotFound();
})
    .WithTags("Rol");

    app.MapDelete("/rol/{IdRol}/usuario/{IdUsuario}", (int IdRol, int IdUsuario, EscuelaContext context) =>
{
    var usuariorol = context.Usuariorols.FirstOrDefault(usuariorol => usuariorol.Idrol == IdRol && usuariorol.Idusuario == IdUsuario);
    if (usuariorol != null && usuariorol != null)
    {
        // Eliminar el usuario del rol
        context.Usuariorols.Remove(usuariorol);
        return Results.Ok();
    }

    return Results.NotFound();
})
    .WithTags("Rol");
    return app;
    }
}
