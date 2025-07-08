using Novusnet.Dominio.Modelo.Abstracciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novusnet.Infraestructura.AccesoDatos.Repositorio
{
    public class ServicioRepositorioImpl : RepositorioImpl<SServicio>, IServicioRepositorio
    {
        private readonly NovusnetPROContext _novusnetPROContext;

        public ServicioRepositorioImpl(NovusnetPROContext dBContext) : base(dBContext)
        {
            _novusnetPROContext = dBContext;
        }

        public Task<List<SServicio>> ListarServiciosActivos()
        {
            throw new NotImplementedException();
        }
    }
}
