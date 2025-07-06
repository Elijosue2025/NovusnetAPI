using Novusnet.Dominio.Modelo.Abstracciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novusnet.Infraestructura.AccesoDatos.Repositorio
{
    
        public class EmpleadoRepositorioImpl : RepositorioImpl<Empleado>, IEmpleadoRepositorio
        {
            public EmpleadoRepositorioImpl(NovusnetPROContext dBContext) : base(dBContext)
            {

            }
        }
    
}
