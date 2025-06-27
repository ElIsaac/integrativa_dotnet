using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

// 🔧 Agrega configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend", policy =>
    {
        policy.WithOrigins("http://localhost") // o pon el puerto del frontend, ej. http://localhost:5173
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddSqlServer<DatabaseContext>(
    builder.Configuration.GetConnectionString("DefaultConnection")!);

var app = builder.Build();

// 🔧 Usa la política CORS ANTES de definir los endpoints
app.UseCors("PermitirFrontend");

app.MapPost("/categorias", async (List<Category> categorias, DatabaseContext db) =>
{
    db.Categories.AddRange(categorias);
    await db.SaveChangesAsync();
    return Results.Created("/categorias", categorias);
});

app.MapGet("/categorias", async (DatabaseContext db) =>
    await db.Categories.ToListAsync());

app.MapPost("/productos", async (List<Product> productos, DatabaseContext db) =>
{
    db.Products.AddRange(productos);
    await db.SaveChangesAsync();
    return Results.Created("/productos", productos);
});

app.MapGet("/productos", async (DatabaseContext db) =>
    await db.Products.ToListAsync());

app.MapPost("/ventas", async (Sale venta, DatabaseContext db) =>
{
    var producto = await db.Products.FindAsync(venta.ProductID);

    if (producto == null)
    {
        return Results.NotFound($"Producto con ID {venta.ProductID} no encontrado.");
    }

    if (producto.Stock < venta.Quantity)
    {
        return Results.BadRequest($"No hay suficiente stock para el producto '{producto.ProductName}'. Stock actual: {producto.Stock}");
    }

    producto.Stock -= venta.Quantity;

    db.Sales.Add(venta);

    await db.SaveChangesAsync();

    return Results.Created($"/ventas/{venta.SaleID}", venta);
});

app.MapGet("/ventas", async (DatabaseContext db) =>
    await db.Sales.ToListAsync());

app.Run();
