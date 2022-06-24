namespace CatalogApi.Queries
{
    public class GetCatalogItemByTypeIdQuery : IRequest<IEnumerable<CatalogItem>>
    {
        public int Id { get; set; }

        public class GetCatalogItemByTypeIdQueryHandler : IRequestHandler<GetCatalogItemByTypeIdQuery, IEnumerable<CatalogItem>>
        {
            private readonly CatalogContext _context;

            public GetCatalogItemByTypeIdQueryHandler(CatalogContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<CatalogItem>> Handle(GetCatalogItemByTypeIdQuery request, CancellationToken cancellationToken)
            {
                var catalogItems = await _context.CatalogItems
                    .Where(w => w.CatalogTypeId == request.Id)
                    .Include(i => i.CatalogType)
                    .ToListAsync(cancellationToken);

                if (request.Id == 0)
                {
                    // if pass catalog type id 0, return all catalog items
                    return await _context.CatalogItems
                        .Include(i => i.CatalogType)
                        .OrderBy(i => i.CatalogTypeId)
                        .ToListAsync(cancellationToken);
                }

                return catalogItems;
            }
        }
    }
}
