namespace CatalogApi.Queries
{
    public class GetAllCatalogGroupsQuery : IRequest<IEnumerable<CatalogGroup>>
    {
        public class GetAllCatalogGroupsQueryHandler : IRequestHandler<GetAllCatalogGroupsQuery, IEnumerable<CatalogGroup>>
        {
            private readonly CatalogContext _context;

            public GetAllCatalogGroupsQueryHandler(CatalogContext context)
            { 
                _context = context; 
            }

            public async Task<IEnumerable<CatalogGroup>> Handle(GetAllCatalogGroupsQuery request, CancellationToken cancellationToken)
            { 
                var groups = await _context.CatalogGroups.ToListAsync(cancellationToken);
                if (!groups.Any()) return Enumerable.Empty<CatalogGroup>();
                return groups;
            }
        }
    }
}
