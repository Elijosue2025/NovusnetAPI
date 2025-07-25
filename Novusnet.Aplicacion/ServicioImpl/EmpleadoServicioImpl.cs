﻿using Novusnet.Aplicacion.Servicio;
using Novusnet.Dominio.Modelo.Abstracciones;
using Novusnet.Infraestructura.AccesoDatos;
using Novusnet.Infraestructura.AccesoDatos.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novusnet.Aplicacion.ServicioImpl
{
    public class 
        EmpleadoServicioImpl : IEmpleadoServicio
    {
        private IEmpleadoRepositorio _empleadoRepositorio;
        private readonly NovusnetPROContext _dBContext;

        public EmpleadoServicioImpl(NovusnetPROContext dBContext)
        {
            _dBContext = dBContext;
            _empleadoRepositorio = new EmpleadoRepositorioImpl(_dBContext);


        }

        public async Task EmpleadoAddAsync(Empleado entidad)
        {

            await _empleadoRepositorio.EmpleadoAddAsync(entidad);
        }

       

        public async Task EmpleadoDeleteAsync(int entidad)
        {
            await _empleadoRepositorio.EmpleadoDeleteAsync(entidad);
        }

        public async Task<IEnumerable<Empleado>> EmpleadoGetAllAsync()
        {
            var empleados = await _empleadoRepositorio.EmpleadoGetAllAsync();
            return empleados;
        }

        public Task<Empleado> EmpleadoGetByIdAsync(int id)
        {
            return _empleadoRepositorio.EmpleadoGetByIdAsync(id);
        }

        public Task<List<Empleado>> ListarEmpleadoRoll()
        {
            return _empleadoRepositorio.ListarEmpleadoRoll();
        }

        public async Task  EmpleadoUpdateAsync(Empleado entidad)
        {
            await _empleadoRepositorio.EmpleadoUpdateAsync(entidad);
        }

    }
}
