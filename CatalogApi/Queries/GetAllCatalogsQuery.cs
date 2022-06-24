namespace CatalogApi.Queries
{
    public class GetAllCatalogsQuery : IRequest<IEnumerable<CatalogItem>>
    {
        public class GetAllCatalogsQueryHandler : IRequestHandler<GetAllCatalogsQuery, IEnumerable<CatalogItem>>
        { 
            private readonly CatalogContext _context;

            public GetAllCatalogsQueryHandler(CatalogContext context)
            { 
                _context = context; 
            }

            public async Task<IEnumerable<CatalogItem>> Handle(GetAllCatalogsQuery request, CancellationToken cancellationToken)
            {
                var catalogList = await _context.CatalogItems
                    .Include(i => i.CatalogType)
                    .OrderBy(o => o.CatalogTypeId)
                    .ToListAsync(cancellationToken);

                if (!catalogList.Any())
                {
                    return Array.Empty<CatalogItem>();
                }
                return catalogList.AsReadOnly();
            }

        }
    }
}
