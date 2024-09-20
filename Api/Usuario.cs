namespace Api;

public class Usuario
{
    public int IdUsuario { get; set; }
    public required string Nombre { get; set; }
    public required string Email {get; set;}
    public string Username {get; set;}
    public string Contrasena {get; set;}
    public bool Habilitado {get; set;}
    public DateTime FechaCreacion {get; set;}

    //Conexión entre las entidades 'Rol' y 'Usuario' (Tabla UsuarioRol)
    public List<Rol> Roles = new List<Rol>();
}