using Novusnet.Dominio.Modelo.Abstracciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novusnet.Infraestructura.AccesoDatos.Repositorio
{
    public class MarerialRepositorioImpl : RepositorioImpl<Material>,IMaterialRepositorio
    {
        public MarerialRepositorioImpl(NovusnetPROContext dBContext) : base(dBContext)
        {

        }
    }
}
