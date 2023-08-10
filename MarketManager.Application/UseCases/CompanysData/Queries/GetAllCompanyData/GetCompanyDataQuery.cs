using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.UseCases.CompanysData.Responce;
using MediatR;

namespace MarketManager.Application.UseCases.CompanysData.Queries.GetAllCompanyData
{
    public class GetCompanyDataQuery : IRequest<CompanyDataResponce>
    {
    }
    public class GetAllCompanyDataQueryHandler : IRequestHandler<GetCompanyDataQuery, CompanyDataResponce>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public GetAllCompanyDataQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CompanyDataResponce> Handle(GetCompanyDataQuery request, CancellationToken cancellationToken)
        {
            var compData =  _context.CompanyDatas;
            if (compData == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.CompanyData));
            }

            var compRes = _mapper.Map<CompanyDataResponce>(compData);
            return compRes;
        }
    }
}
