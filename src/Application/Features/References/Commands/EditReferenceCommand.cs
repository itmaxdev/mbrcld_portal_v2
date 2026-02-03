using Mbrcld.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.References.Commands
{
    public sealed class EditReferenceCommand : IRequest
    {
        #region Command
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string FullName_AR { get; set; }
        public string JobTitle { get; set; }
        public string JobTitle_AR { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationName_AR { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        #endregion

        #region Command handler
        public sealed class CommandHandler : AsyncRequestHandler<EditReferenceCommand>
        {
            private readonly IReferenceRepository refRepository;

            public CommandHandler(IReferenceRepository refRepository)
            {
                this.refRepository = refRepository;
            }

            protected override async Task Handle(EditReferenceCommand request, CancellationToken cancellationToken)
            {
                var reference = await refRepository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
                if (reference.HasNoValue)
                {
                    throw new Exception();
                }

                var refValue = reference.Value;
                refValue.FullName = request.FullName;
                refValue.FullName_Ar = request.FullName_AR;
                refValue.JobTitle = request.JobTitle;
                refValue.JobTitle_Ar = request.JobTitle_AR;
                refValue.OrganizationName = request.OrganizationName;
                refValue.OrganizationName_Ar = request.OrganizationName_AR;
                refValue.Email = request.Email;
                refValue.Mobile = request.Mobile;

                await refRepository.UpdateAsync(refValue).ConfigureAwait(false);
            }
        }
        #endregion
    }
}
