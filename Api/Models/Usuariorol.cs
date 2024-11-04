using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Usuariorol
{
    public int Idur { get; set; }

    public int Idusuario { get; set; }

    public int Idrol { get; set; }

    public virtual Rol IdrolNavigation { get; set; } = null!;

    public virtual Usuario IdusuarioNavigation { get; set; } = null!;
}
