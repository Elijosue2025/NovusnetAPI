using Microsoft.EntityFrameworkCore;
using Novusnet.Dominio.Modelo.Abstracciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novusnet.Infraestructura.AccesoDatos.Repositorio
{
    public class OrdenMaterialRepositorioImpl : RepositorioImpl<Orden_Material>, IOrdenMaterialRepositorio

    {
        private readonly NovusnetPROContext _novusnetPROContext;

        public OrdenMaterialRepositorioImpl(NovusnetPROContext dBcontext) : base(dBcontext)
        {
            _novusnetPROContext = dBcontext;

        }
        public Task<List<Material>> ListarOrdenMaterialActivos()
        {
            try
            {
                var Resultado = from tmMaterial in _novusnetPROContext.Material
                                where tmMaterial.ma_stock_actual > 1
                                select tmMaterial;
                return Resultado.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error Listar Clientes Activos:" + ex.Message);

            }
        }

    }
}
