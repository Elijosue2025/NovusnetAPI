using Microsoft.EntityFrameworkCore;
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
        private readonly NovusnetPROContext _novusnetPROContext;

        public EmpleadoRepositorioImpl(NovusnetPROContext dBContext) : base(dBContext)
        {
            _novusnetPROContext = dBContext;
        }
        public async Task<List<Empleado>> ListaEmpleadosRoll()
        {
            try
            {
                var resultado = (from tmEmpleado in _novusnetPROContext.Empleado
                                 where tmEmpleado.emp_roll == "Tecnico"
                                 select tmEmpleado);

                return await resultado.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar empleados con rol técnico", ex);
            }
        }

    }


}
