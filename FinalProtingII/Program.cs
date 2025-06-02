var builder = WebApplication.CreateBuilder(args);

// Tambahkan ini untuk IHttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Session & MVC
builder.Services.AddSession();
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Login/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();
