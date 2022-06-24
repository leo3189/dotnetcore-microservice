namespace CatalogApi.Queries
{
    public class GetAllCatalogTypesQuery : IRequest<IEnumerable<CatalogType>>
    {
        public class GetAllCatalogTypesQueryHandler : IRequestHandler<GetAllCatalogTypesQuery, IEnumerable<CatalogType>>
        {
            private readonly CatalogContext _context;

            public GetAllCatalogTypesQueryHandler(CatalogContext context)
            { 
                _context = context;
            }

            public async Task<IEnumerable<CatalogType>> Handle(GetAllCatalogTypesQuery request, CancellationToken cancellationToken)
            {
                var types = await _context.CatalogTypes
                    .ToListAsync(cancellationToken);
                if (!types.Any()) return Enumerable.Empty<CatalogType>();
                return types.AsReadOnly();
            }
        }
    }
}
