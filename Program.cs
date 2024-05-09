var builder = WebApplication.CreateBuilder();

builder.Services.AddDistributedMemoryCache();// добавляем IDistributedMemoryCache
builder.Services.AddSession();  // добавляем сервисы сессии


var app = builder.Build();

app.UseSession();
app.Use(async (context, next) =>
{
    context.Items["froggy"] = "Your FrogName = ";
    await next.Invoke();
    context.Items.Remove("froggy");
});

app.Map("/cat/{MEOW}", async (string MEOW, HttpContext context) => {
    context.Response.Cookies.Append("MEOW", MEOW);
    if (context.Request.Cookies.ContainsKey("MEOW"))
    {
        string? name = context.Request.Cookies["MEOW"];
        await context.Response.WriteAsync($"The cat {name}!");
    }
    else
    {
        context.Response.Cookies.Delete("MEOW");
    }
});

app.Map("/frog/{froggy}", async (string froggy, HttpContext context) =>
{
    await context.Response.WriteAsync($"{context.Items["froggy"]}" + froggy);
});

app.Map("/weather/{weather}", async (string weather, HttpContext context) =>
{
    if (context.Session.Keys.Contains("weather"))
        await context.Response.WriteAsync($"The weather {context.Session.GetString("weather")}!");
    else
    {
        context.Session.SetString("weather", weather);
        await context.Response.WriteAsync("Hello user!");
    }
});
/*
app.Run(async (context) =>
{ 
});*/
 

app.Run();
public delegate Task RequestDelegateWithFroggy(string froggy2, HttpContext context);
public class Dog()
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Age { get; set; }
}