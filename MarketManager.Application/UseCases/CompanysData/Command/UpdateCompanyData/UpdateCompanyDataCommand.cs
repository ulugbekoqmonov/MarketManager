using MarketManager.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MarketManager.Application.UseCases.CompanyData.Command.UpdateCompanyData
{
    public class UpdateCompanyDataCommand : IRequest
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string Logo { get; set; }
        public IFormFile? LogoImg { get; set; }
        public string Phone { get; set; }
        public string Location { get; set; }
        public DateOnly Data { get; set; }
    }

    public class UpdateCompanyDataCommandHandler : IRequestHandler<UpdateCompanyDataCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ISaveImg _saveImg;
        public UpdateCompanyDataCommandHandler(IApplicationDbContext context, ISaveImg saveImg)
        {
            _context = context;
            _saveImg = saveImg;
        }

        public async Task Handle(UpdateCompanyDataCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.CompanyData compData = await _context.CompanyDatas.FindAsync(request.Id);
            if (compData == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.CompanyData), request.Id);
            }

            string imgSource = request.Logo;
            if (request.LogoImg != null)
            {
                imgSource = _saveImg.SaveImage(request.LogoImg);
            }

            compData.Id= request.Id;
            compData.CompanyName= request.CompanyName;
            compData.Logo = imgSource;
            compData.Phone= request.Phone;
            compData.Location = request.Location;
            compData.Data = request.Data;

            _context.CompanyDatas.Update(compData);
            await _context.SaveChangesAsync(cancellationToken);
            
        }
    }
}
