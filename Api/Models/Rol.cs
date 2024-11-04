﻿using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Rol
{
    public string? Nombre { get; set; }

    public bool? Habilitado { get; set; }

    public DateTime? Fechacreacion { get; set; }

    public int Idrol { get; set; }

    public virtual ICollection<Usuariorol> Usuariorols { get; set; } = new List<Usuariorol>();
}
