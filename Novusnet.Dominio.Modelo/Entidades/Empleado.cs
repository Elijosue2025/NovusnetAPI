﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Novusnet.Infraestructura.AccesoDatos;

public partial class Empleado
{
    public int pk_Empleado { get; set; }

    public string emp_roll { get; set; }

    public string emp_nombre { get; set; }

    public string emp_apellido { get; set; }

    public string emp_cedula { get; set; }

    public string emp_direccion { get; set; }

    public string emp_telefono { get; set; }

    public string emp_email { get; set; }

    public DateTime? emp_fecha_registro { get; set; }

    public int? emp_activo { get; set; }

    public virtual ICollection<Logging> Logging { get; set; } = new List<Logging>();

    public virtual ICollection<Orden_Trabajo> Orden_Trabajo { get; set; } = new List<Orden_Trabajo>();
}