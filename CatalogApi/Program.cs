var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Mysql configure
builder.Services.AddDbContext<CatalogContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("CatalogDB");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// Add MediatR
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

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

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

#region Catalog Item API
const string Catalog_Item_Tag = "Catalog Item";

app.MapPost("/api/catalog-item", async (CreateCatalogItemCommand item, IMediator mediator) => 
{
    return Results.Ok(await mediator.Send(item));
}).WithTags(Catalog_Item_Tag);

app.MapGet("/api/catalog-item", async ([FromServices] IMediator mediator) =>
{
    return Results.Ok(await mediator.Send(new GetAllCatalogsQuery()));
}).WithTags(Catalog_Item_Tag);

app.MapGet("/api/catalog-item/{id}", async (int id, [FromServices] IMediator mediator) =>
{
    var command = await mediator.Send(new GetCatalogByIdQuery { Id = id });
    if (command == null) return Results.NotFound();
    return Results.Ok(command);
}).WithTags(Catalog_Item_Tag);

app.MapGet("/api/catalog-items-by-type/{typeId}", async (int typeId, [FromServices] IMediator mediator) =>
{
    var command = await mediator.Send(new GetCatalogItemByTypeIdQuery { Id = typeId });
    return Results.Ok(command);
}).WithTags(Catalog_Item_Tag);

app.MapGet("/api/catalog-items-by-group/{groupId}", async (int groupId, [FromServices] IMediator mediator) =>
{
    var command = await mediator.Send(new GetCatalogItemByGroupIdQuery { GroupId = groupId });
    return Results.Ok(command);
}).WithTags(Catalog_Item_Tag);

app.MapPut("/api/catalog-item", async (CatalogItem item, [FromServices] IMediator mediator) =>
{
    var command = await mediator.Send(new UpdateCatalogByIdCommand { Item = item });
    if (command == 0) return Results.NotFound();
    return Results.NoContent();
}).WithTags(Catalog_Item_Tag);

app.MapDelete("/api/catalog-item/{id}", async (int id, [FromServices] IMediator mediator) =>
{
    var command = await mediator.Send(new DeleteCatalogByIdCommand { Id = id });
    if (command == 0) return Results.NotFound();
    return Results.NoContent();
}).WithTags(Catalog_Item_Tag);
#endregion

#region Catalog Type API
const string Catalog_Type_Tag = "Catalog Type";

app.MapPost("/api/catalog-type", async (CreateCatalogTypeCommand command, [FromServices] IMediator mediator) =>
{
    return Results.Ok(await mediator.Send(command));
}).WithTags(Catalog_Type_Tag);

app.MapGet("/api/catalog-type", async ([FromServices] IMediator mediator) =>
{
    return Results.Ok(await mediator.Send(new GetAllCatalogTypesQuery()));
}).WithTags(Catalog_Type_Tag);

app.MapPut("/api/catalog-type", async (CatalogType type, [FromServices] IMediator mediator) =>
{
    var commond = await mediator.Send(new UpdateCatalogTypeCommand { Type = type });
    if (commond == 0) return Results.NotFound();
    return Results.NoContent();
}).WithTags(Catalog_Type_Tag);

app.MapDelete("/api/catalog-type/{id}", async (int id, [FromServices] IMediator mediator) =>
{
    var commod = await mediator.Send(new DeleteCatalogTypeByIdCommand { Id = id });
    if (commod == 0) return Results.NotFound();
    return Results.NoContent();
}).WithTags(Catalog_Type_Tag);
#endregion

#region Catalog Group API
const string Catalog_Group_Tag = "Catalog Group";

app.MapPost("/api/catalog-group", async (CreateCatalogGroupCommand command, [FromServices] IMediator mediator) =>
{
    return Results.Ok(await mediator.Send(command));
}).WithTags(Catalog_Group_Tag);

app.MapGet("/api/catalog-group", async ([FromServices] IMediator mediator) =>
{
    return Results.Ok(await mediator.Send(new GetAllCatalogGroupsQuery()));
}).WithTags(Catalog_Group_Tag);

app.MapGet("/api/catalog-group-by-type/{typeId}", async (int typeId, [FromServices] IMediator mediator) =>
{
    return Results.Ok(await mediator.Send(new GetCatalogGroupByTypeQuery { Id = typeId }));
}).WithTags(Catalog_Group_Tag);

app.MapPut("/api/catalog-group", async (CatalogGroup group, [FromServices] IMediator mediator) =>
{
    var command = await mediator.Send(new UpdateCatalogGroupByIdCommand { Group = group });
    if (command == 0) return Results.NotFound();
    return Results.NoContent();
}).WithTags(Catalog_Group_Tag);

app.MapDelete("/api/catalog-group/{id}", async (int id, [FromServices] IMediator mediator) =>
{
    var command = await mediator.Send(new DeleteCatalogGroupByIdCommand { Id = id });
    if (command == 0) return Results.NotFound();
    return Results.NoContent();
}).WithTags(Catalog_Group_Tag);
#endregion

app.Run();