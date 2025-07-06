using Novusnet.Dominio.Modelo.Abstracciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novusnet.Infraestructura.AccesoDatos.Repositorio
{
    public class OrdenTrabajoRepositorioImpl : RepositorioImpl<Orden_Trabajo>, IOrdenTrabajoRepositorio
    {
        public OrdenTrabajoRepositorioImpl(NovusnetPROContext dBcontext) : base(dBcontext)
        {
        }
    }
}
