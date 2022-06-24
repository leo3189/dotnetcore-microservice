namespace CatalogApi.Commands
{
    public class UpdateCatalogGroupByIdCommand : IRequest<int>
    {
        public CatalogGroup Group { get; set; }

        public class UpdateCatalogGroupCommandHandler : IRequestHandler<UpdateCatalogGroupByIdCommand, int>
        { 
            private readonly CatalogContext _context;

            public UpdateCatalogGroupCommandHandler(CatalogContext context)
            { 
                _context = context;
            }

            public async Task<int> Handle(UpdateCatalogGroupByIdCommand command, CancellationToken cancellationToken)
            {
                var group = await _context.CatalogGroups
                    .AsNoTracking()
                    .SingleOrDefaultAsync(s => s.Id.Equals(command.Group.Id), cancellationToken);
                if (group == null) return default;

                _context.CatalogGroups.Update(command.Group);
                await _context.SaveChangesAsync(cancellationToken);
                return command.Group.Id;
            }
        }
    }
}
