using Microsoft.EntityFrameworkCore;
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
        public async Task<List<SServicio>> ListarServiciosConMateriales()
        {
            try
            {
                var resultado = from tmSservicio in _novusnetPROContext.SServicio
                                where tmSservicio.ser_requiere_material == 1
                                select tmSservicio;

                return await resultado.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar servicios que requieren materiales: " + ex.Message, ex);
            }
        }



    }
}