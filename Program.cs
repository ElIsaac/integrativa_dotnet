using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSqlServer<DatabaseContext>(
    builder.Configuration.GetConnectionString("DefaultConnection")!);

var app = builder.Build();



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
    db.Sales.Add(venta);
    await db.SaveChangesAsync();
    return Results.Created($"/ventas/{venta.SaleID}", venta);
});

app.MapGet("/ventas", async (DatabaseContext db) =>
    await db.Sales.ToListAsync());

app.Run();