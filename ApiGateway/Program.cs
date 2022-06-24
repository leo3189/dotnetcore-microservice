using ApiGateway.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "Api Gateway",
            Version = "v1",
        });

    //c.CustomSchemaIds(x => x.FullName);

    //c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    //{
    //    Name = "Authorization",
    //    Type = SecuritySchemeType.ApiKey,
    //    Scheme = "Bearer",
    //    BearerFormat = "JWT",
    //    In = ParameterLocation.Header,
    //    Description = "JWT Authorization header using the Bearer scheme."
    //});

    //c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    //{
    //    {
    //        new OpenApiSecurityScheme
    //        {
    //            Reference = new OpenApiReference
    //            {
    //                Type = ReferenceType.SecurityScheme,
    //                Id = "Bearer"
    //            },
    //            Scheme = "oauth2",
    //            Name = "Bearer",
    //            In = ParameterLocation.Header
    //        },
    //        new List<string>()
    //    }
    //});
});

builder.Host.ConfigureAppConfiguration(options =>
{ 
    options.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
});

builder.Services.AddOcelot();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer("IdentityApiKey", options =>
    {
        options.Authority = "https://localhost:7008";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseOcelot();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

#region Catalog Item API
const string Catalog_Item_Tag = "Catalog Item";

app.MapPost("/catalog-item", (CreateCatalogItemDTO itemDTO) => { }).WithTags(Catalog_Item_Tag);

app.MapGet("/catalog-item", () => { }).Produces<IEnumerable<CatalogItem>>().WithTags(Catalog_Item_Tag);

app.MapGet("/catalog-items-by-type/{typeId}", (int typeId) => { }).Produces<IEnumerable<CatalogItem>>().WithTags(Catalog_Item_Tag);

app.MapGet("/catalog-item/{id}", (int id) => { }).Produces<CatalogItem>().WithTags(Catalog_Item_Tag);

app.MapGet("/catalog-items-by-group/{groupId}", (int groupId) => { })
    .Produces<IEnumerable<CatalogItem>>()
    .WithTags(Catalog_Item_Tag);

app.MapPut("/catalog-item", (UpdateCatalogItemDTO itemDTO) => { })
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status204NoContent)
    .WithTags(Catalog_Item_Tag);

app.MapDelete("/catalog-item/{id}", (int id) => { }).WithTags(Catalog_Item_Tag);
#endregion

#region Catalog Type API
const string Catalog_Type_Tag = "Catalog Type";

app.MapPost("/catalog-type", (CreateCatalogTypeDTO itemDTO) => { }).WithTags(Catalog_Type_Tag);

app.MapGet("/catalog-type", () => { }).Produces<IEnumerable<CatalogType>>().WithTags(Catalog_Type_Tag);

app.MapPut("/catalog-type", (UpdateCatalogTypeDTO itemDTO) => { }).WithTags(Catalog_Type_Tag);

app.MapDelete("/catalog-type/{id}", (int id) => { }).WithTags(Catalog_Type_Tag);
#endregion

#region Catalog Group API
const string Catalog_Group_Tag = "Catalog Group";

app.MapPost("/catalog-group", (CreateCatalogGroupDTO dto) => { }).WithTags(Catalog_Group_Tag);

app.MapGet("/catalog-group", () => { }).Produces<IEnumerable<CatalogGroup>>().WithTags(Catalog_Group_Tag);

app.MapGet("/catalog-group-by-type/{typeId}", (int typeId) => { }).Produces<IEnumerable<CatalogGroup>>().WithTags(Catalog_Group_Tag);

app.MapPut("/catalog-group", (UpdateCatalogGroupDTO dto) => { }).WithTags(Catalog_Group_Tag);

app.MapDelete("/catalog-group/{id}", (int id) => { }).WithTags(Catalog_Group_Tag);
#endregion

#region Basket API
const string Basket_Tag = "Basket";

app.MapPost("/basket", [Authorize] (CustomerBasketDTO basketDTO) => { }).WithTags(Basket_Tag);

app.MapGet("/basket/{customerId}", [Authorize] (string customerId) => { })
    .Produces<CustomerBasketDTO>()
    .Produces(StatusCodes.Status204NoContent)
    .WithTags(Basket_Tag);

app.MapPost("/checkout", [Authorize] (BasketCheckoutDTO checkoutDTO, [FromHeader(Name = "x-requestid")] string requestId) => { }).WithTags(Basket_Tag);
#endregion

app.Run();