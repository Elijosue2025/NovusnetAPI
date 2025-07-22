using Novusnet.Aplicacion.DTO.DTOS;
using Novusnet.Infraestructura.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novusnet.Dominio.Modelo.Abstracciones
{
    
    public interface IEmpleadoRepositorio : IRepositorio<Empleado>
    {
        Task EmpleadoAddAsync(Empleado entidad);
        Task EmpleadoDeleteAsync(int entidad);
        Task<IEnumerable<Empleado>> EmpleadoGetAllAsync();
        Task<Empleado> EmpleadoGetByIdAsync(int id);
        Task EmpleadoUpdateAsync(Empleado entidad);
        Task<List<Empleado>> ListarEmpleadoRoll();
        //Task<List<Empleado>> EmpleadoGetAllAsync();

    }
}
