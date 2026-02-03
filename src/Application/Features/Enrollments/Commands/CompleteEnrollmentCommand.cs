using Mbrcld.Application.Features.Users.Queries.GetUserProfileCompletion.SectionCheckers;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Enrollments.Commands
{
    public sealed class CompleteEnrollmentCommand : IRequest<Result>
    {
        #region Command
        public Guid Id { get; }

        public CompleteEnrollmentCommand(Guid id)
        {
            this.Id = id;
        }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<CompleteEnrollmentCommand, Result>
        {
            private readonly IEnrollmentRepository refRepository;
            private readonly IUserRepository userRepository;
            private readonly IProgramQuestionRepository programQuestionRepository;
            private readonly IProgramAnswerRepository programAnswerRepository;
            private readonly IEnumerable<IUserProfileSectionCompletionChecker> userProfileSectionCompletionCheckers;

            public CommandHandler(
                IEnrollmentRepository refRepository,
                IUserRepository userRepository,
                IProgramQuestionRepository programQuestionRepository,
                IProgramAnswerRepository programAnswerRepository,
                IEnumerable<IUserProfileSectionCompletionChecker> userProfileSectionCompletionCheckers)
            {
                this.refRepository = refRepository;
                this.userRepository = userRepository;
                this.programQuestionRepository = programQuestionRepository;
                this.programAnswerRepository = programAnswerRepository;
                this.userProfileSectionCompletionCheckers = userProfileSectionCompletionCheckers;
            }

            public async Task<Result> Handle(CompleteEnrollmentCommand request, CancellationToken cancellationToken)
            {
                var enrollment = await refRepository.GetByIdAsync(request.Id, cancellationToken);
                if (enrollment.HasNoValue)
                {
                    return Result.Failure("Enrollment does not exist");
                }

                var canEnroll = true;

                if (enrollment.Value.PymetricsStatus == null ||
                    enrollment.Value.PymetricsStatus < 4)
                {
                    canEnroll = false;
                }

                if (canEnroll)
                {
                    var user = (User)await this.userRepository.GetByIdAsync(enrollment.Value.ContactId, cancellationToken);

                    if (user.Achievements.Count == 0 ||
                        user.References.Count == 0 )//|| user.References.All(x => !x.IsCompleted))
                    {
                        canEnroll = false;
                    }

                    if (canEnroll)
                    {
                        foreach (var checker in this.userProfileSectionCompletionCheckers)
                        {
                            canEnroll &= await checker.IsSectionCompleteAsync(user);
                        }
                    }
                }

                if (canEnroll)
                {
                    var questions = await this.programQuestionRepository.ListByProgramIdAsync(
                        enrollment.Value.ProgramId, cancellationToken);

                    var answers = await this.programAnswerRepository.ListByEnrollmentIdAsync(enrollment.Value.Id, cancellationToken);

                    if (questions.Any(q => answers.All(a => a.QuestionId != q.Id)))
                    {
                        canEnroll = false;
                    }
                }

                if (!canEnroll)
                {
                    return Result.Failure("MissingInformation");
                }

                var enroValue = enrollment.Value;
                enroValue.Stage = 1; ////Enrolled
                await refRepository.UpdateAsync(enroValue).ConfigureAwait(false);
                return Result.Success();
            }
        }
        #endregion
    }
}
