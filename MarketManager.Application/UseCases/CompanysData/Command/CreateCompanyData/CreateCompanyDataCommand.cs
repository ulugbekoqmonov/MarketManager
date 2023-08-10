
using MarketManager.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MarketManager.Application.UseCases.CompanysData.Command.CreateCompanyData
{
    public class CreateCompanyDataCommand : IRequest<Guid>
    {
        public string CompanyName { get; set; }
        public IFormFile LogoImg { get; set; }
        public string Phone { get; set; }
        public string Location { get; set; }
        public DateOnly Data { get; set; }
    }
    public class CreateCompanyDataCommandHandler : IRequestHandler<CreateCompanyDataCommand, Guid>
    {
        private readonly ISaveImg _saveLogo;
        private readonly IApplicationDbContext _context;

        public CreateCompanyDataCommandHandler(ISaveImg saveLogo, IApplicationDbContext context)
        {
            _saveLogo = saveLogo;
            _context = context;
        }

        public async Task<Guid> Handle(CreateCompanyDataCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.CompanyData compData = new Domain.Entities.CompanyData
            {
                Id = Guid.NewGuid(),
                CompanyName = request.CompanyName,
                Logo = _saveLogo.SaveImage(request.LogoImg),
                Phone = request.Phone,
                Location = request.Location,
                Data = request.Data
            };
            var entity = await _context.CompanyDatas.AddAsync(compData);
            await _context.SaveChangesAsync();
            return compData.Id;

        }
    }
}
