using MarketManager.Application.Common.Interfaces;
using MediatR;


namespace MarketManager.Application.UseCases.CompanysData.Command.DeleteCompanyData
{
    public class DeleteCompanyDataCommand : IRequest
    {
        public Guid Id { get; set; }
    }

    public class DeleteCompanyDataCommandHandler : IRequestHandler<DeleteCompanyDataCommand>
    {
       private readonly IApplicationDbContext _context;
        private readonly IDeleteImg _deleteImg;

        public DeleteCompanyDataCommandHandler(IApplicationDbContext context, IDeleteImg deleteImg)
        {
            _context = context;
            _deleteImg = deleteImg;
        }

        public async Task Handle(DeleteCompanyDataCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.CompanyData? compData = await _context.CompanyDatas.FindAsync(request.Id);

            if (compData == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.CompanyData), request.Id);
            }

            if (compData.Logo is not null)
            {
                _deleteImg.Delete_Img(compData.Logo);
            }

            _context.CompanyDatas.Remove(compData);
            await _context.SaveChangesAsync();
            
        }
    }
}
