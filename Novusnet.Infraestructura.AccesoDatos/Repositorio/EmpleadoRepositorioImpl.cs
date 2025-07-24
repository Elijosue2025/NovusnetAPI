using Microsoft.EntityFrameworkCore;
using Novusnet.Aplicacion.DTO.DTOS;
using Novusnet.Dominio.Modelo.Abstracciones;

namespace Novusnet.Infraestructura.AccesoDatos.Repositorio
{
    public class EmpleadoRepositorioImpl : RepositorioImpl<Empleado>, IEmpleadoRepositorio
    {
        private readonly NovusnetPROContext _novusnetPROContext;
        // private readonly Empleado _empleado;
      //  private IEmpleadoRepositorio _empleadoRepositorio;

        public  EmpleadoRepositorioImpl(NovusnetPROContext dBContext) : base(dBContext)
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

        // 2. Buscar empleados por roll específico
        public async Task<List<EmpleadoDTO>> BuscarEmpleadosPorRoll(string roll)
        {
            try
            {
                var resultados = await (from e in _novusnetPROContext.Empleado
                                        where e.emp_roll.Contains(roll) && e.emp_activo == 1
                                        orderby e.emp_nombre
                                        select new EmpleadoDTO
                                        {
                                            IdEmpleado = e.pk_Empleado,
                                            NombreCompleto = e.emp_nombre + " " + e.emp_apellido,
                                            Roll = e.emp_roll,
                                            FechaRegistro = e.emp_fecha_registro,
                                            Direccion = e.emp_direccion
                                        }).ToListAsync();
                return resultados;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // 3. Obtener empleados registrados en un rango de fechas
        public async Task<List<EmpleadoDTO>> EmpleadosRegistradosEnRango(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var resultados = await (from e in _novusnetPROContext.Empleado
                                        where e.emp_fecha_registro >= fechaInicio &&
                                              e.emp_fecha_registro <= fechaFin
                                        group e by e.emp_roll into grupo
                                        select new EmpleadoDTO
                                        {
                                            Roll = grupo.Key,
                                            CantidadEmpleados = grupo.Count(),
                                            EmpleadosDelRoll = grupo.Select(emp => emp.emp_nombre + " " + emp.emp_apellido).ToList()
                                        }).ToListAsync();
                return resultados;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<EmpleadoDTO>> ListarEmpleadosActivos()
        {
            try
            {
                var resultados = await (from e in _novusnetPROContext.Empleado
                                        where e.emp_activo == 1
                                        orderby e.emp_nombre
                                        select new EmpleadoDTO
                                        {
                                            IdEmpleado = e.pk_Empleado,
                                            NombreCompleto = e.emp_nombre + " " + e.emp_apellido,
                                            Roll = e.emp_roll,
                                            FechaRegistro = e.emp_fecha_registro,
                                            Direccion = e.emp_direccion
                                        }).ToListAsync();
                return resultados;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar empleados activos", ex);
            }
        }

        public async Task<EmpleadoDTO> ObtenerEmpleadoPorCedula(string cedula)
        {
            try
            {
                var resultado = await (from e in _novusnetPROContext.Empleado
                                       where e.emp_cedula == cedula
                                       select new EmpleadoDTO
                                       {
                                           IdEmpleado = e.pk_Empleado,
                                           NombreCompleto = e.emp_nombre + " " + e.emp_apellido,
                                           Roll = e.emp_roll,
                                           FechaRegistro = e.emp_fecha_registro,
                                           Direccion = e.emp_direccion,
                                           Cedula = e.emp_cedula
                                       }).FirstOrDefaultAsync();
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener empleado por cédula", ex);
            }
        }

        public async Task<List<EmpleadoDTO>> EmpleadosPorEstado(bool activo)
        {
            try
            {
                int estadoInt = activo ? 1 : 0;
                var resultados = await (from e in _novusnetPROContext.Empleado
                                        where e.emp_activo == estadoInt
                                        orderby e.emp_nombre
                                        select new EmpleadoDTO
                                        {
                                            IdEmpleado = e.pk_Empleado,
                                            NombreCompleto = e.emp_nombre + " " + e.emp_apellido,
                                            Roll = e.emp_roll,
                                            FechaRegistro = e.emp_fecha_registro,
                                            Direccion = e.emp_direccion
                                        }).ToListAsync();
                return resultados;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener empleados por estado", ex);
            }
        }

        public async Task<int> ContarEmpleadosPorRoll(string roll)
        {
            try
            {
                var count = await (from e in _novusnetPROContext.Empleado
                                   where e.emp_roll == roll
                                   select e).CountAsync();
                return count;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al contar empleados por roll", ex);
            }
        }

        public async Task<bool> ExisteEmpleadoPorCedula(string cedula)
        {
            try
            {
                var existe = await (from e in _novusnetPROContext.Empleado
                                    where e.emp_cedula == cedula
                                    select e).AnyAsync();
                return existe;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar existencia de empleado por cédula", ex);
            }
        }

        public async Task EmpleadoAddAsync(Empleado entidad)
        {
            await _novusnetPROContext.Empleado.AddAsync(entidad);
            await _novusnetPROContext.SaveChangesAsync();
        }

      
        public new async Task<List<Empleado>> EmpleadoGetAllAsync(){
   
            try
    
            {
                return await _novusnetPROContext.Empleado.ToListAsync();
            }
    
            catch (Exception ex)
    
            {
        
                throw new Exception("Error al listar empleados", ex);
    
            }
}

        public async Task<Empleado> EmpleadoGetByIdAsync(int pk_Empleado)
        {
            try
            {
                return await _novusnetPROContext.Empleado.FindAsync(pk_Empleado);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener empleado con ID {pk_Empleado}", ex);
            }
        }

       

        public async Task<List<Empleado>> ListarEmpleadoRoll()
        {
            try
            {
                
                return await _novusnetPROContext.Empleado
                    .Where(e => e.emp_roll == "Tecnico")
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar empleados por rol", ex);
            }
        }

        public Task ObtenerPorIdAsync(int pk_Empleado)
        {
            throw new NotImplementedException();
        }

        public Task EmpleadoDeleteAsync(int entidad)
        {
            // return _empleadoRepositorio.EmpleadoDeleteAsync(entidad);
            throw new NotImplementedException();

        }

        public async Task EmpleadoUpdateAsync(Empleado entidad)
        {
            try
            {
                var empleadoExistente = await _novusnetPROContext.Empleado.FindAsync(entidad.pk_Empleado);

                if (empleadoExistente == null)
                    throw new Exception("Empleado no encontrado.");

                // Actualizar los campos
                empleadoExistente.emp_roll = entidad.emp_roll;
                empleadoExistente.emp_nombre = entidad.emp_nombre;
                empleadoExistente.emp_apellido = entidad.emp_apellido;
                empleadoExistente.emp_cedula = entidad.emp_cedula;
                empleadoExistente.emp_direccion = entidad.emp_direccion;
                empleadoExistente.emp_telefono = entidad.emp_telefono;
                empleadoExistente.emp_email = entidad.emp_email;
                empleadoExistente.emp_fecha_registro = entidad.emp_fecha_registro;
                empleadoExistente.emp_activo = entidad.emp_activo;

                await _novusnetPROContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar empleado", ex);
            }
        }



    }
}