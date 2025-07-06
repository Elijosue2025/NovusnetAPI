using Novusnet.Dominio.Modelo.Abstracciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novusnet.Infraestructura.AccesoDatos.Repositorio
{
    public class OrdenMaterialRepositorioImpl : RepositorioImpl<Orden_Material>, IOrdenMaterialRepositorio
    {
        public OrdenMaterialRepositorioImpl(NovusnetPROContext dBcontext) : base(dBcontext)
        {
        }
    }
}
