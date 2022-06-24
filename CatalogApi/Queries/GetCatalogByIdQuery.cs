namespace CatalogApi.Queries
{
    public class GetCatalogByIdQuery : IRequest<CatalogItem?>
    {
        public int Id { get; set; }

        public class GetCatalogByIdQueryHandler : IRequestHandler<GetCatalogByIdQuery, CatalogItem?>
        {
            private readonly CatalogContext _context;

            public GetCatalogByIdQueryHandler(CatalogContext context)
            { 
                _context = context;
            }

            public async Task<CatalogItem?> Handle(GetCatalogByIdQuery query, CancellationToken cancellationToken)
            {
                var catalog = await _context.CatalogItems
                    .Include(i => i.CatalogType)
                    .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken: cancellationToken);
                if (catalog == null) return null;
                return catalog;
            }
        }
    }
}
