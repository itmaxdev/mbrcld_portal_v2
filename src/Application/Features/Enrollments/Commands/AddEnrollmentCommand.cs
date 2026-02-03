using FluentValidation;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Enrollments.Commands
{
    public sealed class AddEnrollmentCommand : IRequest<Result<Guid>>
    {
        #region Command
        public Guid Id { get; set; }
        public Guid ProgramId { get; set; }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<AddEnrollmentCommand, Result<Guid>>
        {
            private readonly IEnrollmentRepository enrollment;
            private readonly IProgramRepository programRepository;

            public CommandHandler(IEnrollmentRepository enrollment, IProgramRepository programRepository)
            {
                this.enrollment = enrollment;
                this.programRepository = programRepository;
            }

            public async Task<Result<Guid>> Handle(AddEnrollmentCommand request, CancellationToken cancellationToken)
            {
                var program = await programRepository.GetProgramByIdAsync(request.ProgramId);
                var enrollmentRecord = Enrollment.Create(request.Id, request.ProgramId, program.CohortId);
                await enrollment.CreateAsync(enrollmentRecord, cancellationToken).ConfigureAwait(false);
                return Result.Success(enrollmentRecord.Id);
            }
        }
        #endregion

        #region Command validator
        public sealed class CommandValidator : AbstractValidator<AddEnrollmentCommand>
        {
            public CommandValidator()
            {                
                RuleFor(x => x.ProgramId).NotNull().NotEmpty();
            }
        }
        #endregion
    }
}
