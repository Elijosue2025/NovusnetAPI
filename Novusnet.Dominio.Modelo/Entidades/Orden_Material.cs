﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Novusnet.Infraestructura.AccesoDatos;

public partial class Orden_Material
{
    public int pk_orden_material { get; set; }

    public string orma_codigo { get; set; }

    public int? orma_cantidad { get; set; }

    public string orma_estado { get; set; }

    public string orma_observaciones { get; set; }

    public DateTime? orma_fecha_uso { get; set; }

    public int fk_material { get; set; }

    public int fk_orden_trabajo { get; set; }

    public virtual Material fk_materialNavigation { get; set; }

    public virtual Orden_Trabajo fk_orden_trabajoNavigation { get; set; }
}