using Microsoft.EntityFrameworkCore;
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
        private readonly NovusnetPROContext _novusnetPROContext;

        public OrdenTrabajoRepositorioImpl(NovusnetPROContext dBcontext) : base(dBcontext)
        {
            _novusnetPROContext = dBcontext;
        }
        public Task<List<Orden_Material>> ListarMaterialStock()
        {
            try
            {
                var Resultado = from tmOrdenMaterial in _novusnetPROContext.Orden_Material
                                where tmOrdenMaterial.orma_estado == "Usado"
                                select tmOrdenMaterial;
                return Resultado.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error Listar Clientes Activos:" + ex.Message);

            }
        }
    }
}
