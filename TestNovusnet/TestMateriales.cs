using Microsoft.EntityFrameworkCore;
using Novusnet.Aplicacion.Servicio;
using Novusnet.Aplicacion.ServicioImpl;
using Novusnet.Infraestructura.AccesoDatos;

namespace TestNovusnet;

public class TestMateriales
{

    private NovusnetPROContext _novusnetPROContext;
    private IMaterialesServicio _iMaterialServicio;

    [SetUp]
    public void Setup()

    {
        var options = new DbContextOptionsBuilder<NovusnetPROContext>()
           .UseSqlServer("Data Source=EJCF;Initial Catalog=NovusnetPRO;Integrated Security=True;TrustServerCertificate=True")
             .Options;
        _novusnetPROContext = new NovusnetPROContext(options);
        _iMaterialServicio = new MaterialesServicioImpl(_novusnetPROContext);
    }

    [Test]
    public async Task TestInsertarCliente()
    {

        /*
         Primer Incer en la tabla Materiales

         */
        var Material = new Material
        {
            ma_codigo = "000000003",
            ma_descripcion = "XXX",
            ma_duracion = "3 AÑOS",
            ma_nombre = "0000000XX000",
            ma_precio_unitario = 1,
            ma_stock_actual = 1,
            ma_stock_minimo = 1

        };

        await _iMaterialServicio.MaterialAddAsync(Material);

        Assert.Pass("Cliente insertado correctamente.");
        /*
         Listar stock por materiales
         
         */

        var Resultado = await _iMaterialServicio.ListarMaterialStock();
        foreach (var item in Resultado)
        {
            Console.WriteLine(item);

        }
    }


    [TearDown]
    public void DespuesTest()
    {
        _novusnetPROContext.Dispose();
    }
}
