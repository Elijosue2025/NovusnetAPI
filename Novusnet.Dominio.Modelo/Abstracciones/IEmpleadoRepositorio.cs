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
        // Consultas específicas de empleados
        Task<List<Empleado>> ListaEmpleadosRoll();
        Task<List<EmpleadoDTO>> ListarEmpleadosActivos();
        Task<List<EmpleadoDTO>> BuscarEmpleadosPorRoll(string roll);
        Task<List<EmpleadoDTO>> EmpleadosRegistradosEnRango(DateTime fechaInicio, DateTime fechaFin);

        // Consultas adicionales útiles
        Task<EmpleadoDTO> ObtenerEmpleadoPorCedula(string cedula);
        Task<List<EmpleadoDTO>> EmpleadosPorEstado(bool activo);
        Task<int> ContarEmpleadosPorRoll(string roll);
        Task<bool> ExisteEmpleadoPorCedula(string cedula);
    }
}
