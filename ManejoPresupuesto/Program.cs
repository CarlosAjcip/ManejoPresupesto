using ManejoPresupuesto.Interface;
using ManejoPresupuesto.Servicios;
using ManejoPresupuesto.Validaciones;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//se agg los servicios que se utilizaran en los controladores
builder.Services.AddTransient<IRepositorioTiposCuentas, RepositorioTiposCuentas>();
builder.Services.AddTransient<IServicioUsuario, ServicioUsuario>();
builder.Services.AddTransient<IRepositorioCuentas, RepositorioCuentas>();
builder.Services.AddTransient<IRepositorioCategorias, RepositorioCategorias>();
builder.Services.AddTransient<IRepositorioTransacciones, RepositorioTransacciones>();
//configurar autoMapper
builder.Services.AddAutoMapper(typeof(Program));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
