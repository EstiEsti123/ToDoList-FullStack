// var builder = WebApplication.CreateBuilder(args);
// var app = builder.Build();

// app.MapGet("/", () => "Hello World!");

// app.Run();

// using Microsoft.EntityFrameworkCore;
// using TodoApi;
// // ודאי שאת מוסיפה using לשם הפרויקט שלך כדי שיזהה את ה-DbContext
// // למשל: using TodoApi.Models; 

// var builder = WebApplication.CreateBuilder(args);

// // --- הוספת החיבור ל-DB ---
// var connectionString = builder.Configuration.GetConnectionString("todolist");

// builder.Services.AddDbContext<ToDoDbContext>(options =>
//     options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
// );
// // -------------------------

// var app = builder.Build();

// app.MapGet("/", () => "Hello World!");
// app.MapGet("/tasks", async (ToDoDbContext db) => 
//     await db.Items.ToListAsync());


// app.MapPost("/tasks", async (ToDoDbContext db, Item newItem) =>
// {
//     db.Items.Add(newItem);   
//     await db.SaveChangesAsync();
//     return Results.Created($"/tasks/{newItem.Id}", newItem);
// });


// app.MapPut("/tasks/{id}", async (ToDoDbContext db, int id, Item inputItem) =>
// {
//     var item = await db.Items.FindAsync(id);

//     if (item is null) return Results.NotFound();

//     item.Name = inputItem.Name;
//     item.IsComplete = inputItem.IsComplete;

//     await db.SaveChangesAsync();

//     return Results.NoContent();
// });

// app.MapDelete("/tasks/{id}", async (ToDoDbContext db, int id) =>
// {
//     if (await db.Items.FindAsync(id) is Item item)
//     {
//         db.Items.Remove(item);
//         await db.SaveChangesAsync();
//         return Results.Ok(item);
//     }

//     return Results.NotFound();
// });
// app.Run();
/////............😁עד אן הקוד הטוב בלי CORS/////
using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);

// --- הוספת החיבור ל-DB ---
var connectionString = builder.Configuration.GetConnectionString("todolist");

builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

// 1. הגדרת פוליסת ה-CORS (חייב לבוא לפני ה-Build)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
// -------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// 2. הפעלת ה-CORS (חייב לבוא מיד אחרי ה-Build ולפני ה-Endpoints)
app.UseCors("AllowAll");

app.MapGet("/", () => "Hello World!");

app.MapGet("/tasks", async (ToDoDbContext db) => 
    await db.Items.ToListAsync());

app.MapPost("/tasks", async (ToDoDbContext db, Item newItem) =>
{
    db.Items.Add(newItem);   
    await db.SaveChangesAsync();
    return Results.Created($"/tasks/{newItem.Id}", newItem);
});

app.MapPut("/tasks/{id}", async (ToDoDbContext db, int id, Item inputItem) =>
{
    var item = await db.Items.FindAsync(id);
    if (item is null) return Results.NotFound();

    item.Name = inputItem.Name;
    item.IsComplete = inputItem.IsComplete;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/tasks/{id}", async (ToDoDbContext db, int id) =>
{
    if (await db.Items.FindAsync(id) is Item item)
    {
        db.Items.Remove(item);
        await db.SaveChangesAsync();
        return Results.Ok(item);
    }
    return Results.NotFound();
});

app.Run();