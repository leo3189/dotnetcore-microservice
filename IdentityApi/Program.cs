using IdentityApi.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("IdentityDB");

// Mysql configure
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    //var connectionString = builder.Configuration.GetConnectionString("IdentityDB");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// Add MediatR
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

// Add .net identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddIdentityServer()
    .AddInMemoryClients(Config.Clients)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryIdentityResources(Config.IdentityResources)
    .AddAspNetIdentity<ApplicationUser>()
    .AddDeveloperSigningCredential();

//builder.Services.AddIdentityServer()
//    .AddDeveloperSigningCredential()
//    .AddAspNetIdentity<ApplicationUser>()
//    .AddConfigurationStore(options =>
//    {
//        options.ConfigureDbContext = b => b.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), options =>
//        {
//            options.MigrationsAssembly("IdentityApi");
//        });
//    })
//    .AddOperationalStore(options =>
//    {
//        options.ConfigureDbContext = b => b.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), options =>
//        {
//            options.MigrationsAssembly("IdentityApi");
//        });
//    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//    db.Database.Migrate();
//}

//DatabaseInitializer.PopulateIdentityServer(app);

//app.UseAuthorization();

app.UseIdentityServer();

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.MapPost("/signup", async (SignupDTO signupDTO, IMediator mediator) =>
{ 
    return Results.Ok(await mediator.Send(new SignupCommand { SignupDTO = signupDTO }));
}).Produces<IdentityResult>();

app.MapPost("/signin", async (SigninDTO signinDTO, IMediator mediator) =>
{
    return Results.Ok(await mediator.Send(new SigninCommand { SigninDTO = signinDTO }));
}).Produces<IdentityResult>();

app.Run();