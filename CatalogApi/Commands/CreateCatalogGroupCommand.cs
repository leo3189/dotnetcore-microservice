namespace CatalogApi.Commands
{
    public class CreateCatalogGroupCommand : IRequest<CatalogGroup>
    {
        public string Name { get; set; }

        public int CatalogTypeId { get; set; }

        public class CreateCatalogGroupHandler : IRequestHandler<CreateCatalogGroupCommand, CatalogGroup>
        { 
            private readonly CatalogContext _context;

            public CreateCatalogGroupHandler(CatalogContext context)
            { 
                _context = context;
            }

            public async Task<CatalogGroup> Handle(CreateCatalogGroupCommand command, CancellationToken cancellationToken)
            {
                var group = new CatalogGroup { Name = command.Name, CatalogTypeId = command.CatalogTypeId };
                _context.CatalogGroups.Add(group);
                await _context.SaveChangesAsync(cancellationToken);
                return group;
            }
        }
    }
}
