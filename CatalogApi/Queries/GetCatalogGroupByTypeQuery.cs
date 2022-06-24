namespace CatalogApi.Queries
{
    public class GetCatalogGroupByTypeQuery : IRequest<IEnumerable<CatalogGroup>>
    {
        public int Id { get; set; }

        public class GetCatalogGroupByTypeQueryHandler : IRequestHandler<GetCatalogGroupByTypeQuery, IEnumerable<CatalogGroup>>
        { 
            private readonly CatalogContext _catalogContext;

            public GetCatalogGroupByTypeQueryHandler(CatalogContext catalogContext)
            { 
                _catalogContext = catalogContext;
            }

            public async Task<IEnumerable<CatalogGroup>> Handle(GetCatalogGroupByTypeQuery request, CancellationToken cancellationToken)
            { 
                var groups = await _catalogContext.CatalogGroups
                    .Where(g => g.CatalogTypeId == request.Id)
                    .ToListAsync(cancellationToken);
                return groups.AsReadOnly();
            }
        }
    }
}
