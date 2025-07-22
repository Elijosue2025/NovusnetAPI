using Microsoft.EntityFrameworkCore;
using Novusnet.Aplicacion.ServicioImpl;
using Novusnet.Infraestructura.AccesoDatos;
using Novusnet.Aplicacion.Servicio;


internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        /*
  * 1 lee el archivo deconfiguracion  de la db
  */
        var coneccionDB = builder.Configuration.GetConnectionString("ConexionDBNovusnet");
        // coneccion dbcontext  almacenada
        builder.Services.AddDbContext<NovusnetPROContext>(options => options.UseSqlServer(coneccionDB), ServiceLifetime.Scoped);

        builder.Services.AddScoped<IEmpleadoServicio, EmpleadoServicioImpl>();
        builder.Services.AddScoped<IClienteServicio, ClienteServicoImpl>();

        var app = builder.Build();

       



        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}