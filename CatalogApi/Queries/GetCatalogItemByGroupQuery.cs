namespace CatalogApi.Queries
{
    public class GetCatalogItemByGroupIdQuery : IRequest<IEnumerable<CatalogItem>>
    {
        public int GroupId { get; set; }

        public class GetCatalogItemByGroupQueryHandler : IRequestHandler<GetCatalogItemByGroupIdQuery, IEnumerable<CatalogItem>>
        { 
            private readonly CatalogContext _context;

            public GetCatalogItemByGroupQueryHandler(CatalogContext context)
            { 
                _context = context;
            }

            public async Task<IEnumerable<CatalogItem>> Handle(GetCatalogItemByGroupIdQuery request, CancellationToken cancellationToken)
            {
                var catalogItems = await _context.CatalogItems
                    .Include(i => i.CatalogGroup)
                    .Where(w => w.CatalogGroupId == request.GroupId)
                    .ToListAsync(cancellationToken);

                return catalogItems.AsReadOnly();
            }
        }
    }
}
