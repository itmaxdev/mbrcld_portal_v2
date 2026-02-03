using Mbrcld.Application.Features.Users.Queries.GetUserProfileCompletion.SectionCheckers;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Enrollments.Queries
{
    public sealed class GetEnrollmentStatusByIdQuery : IRequest<GetEnrollmentStatusByIdViewModel>
    {
        #region Query
        public Guid Id { get; }

        public GetEnrollmentStatusByIdQuery(Guid id)
        {
            Id = id;
        }
        #endregion

        #region Query handler
        private sealed class QueryHandler : IRequestHandler<GetEnrollmentStatusByIdQuery, GetEnrollmentStatusByIdViewModel>
        {
            private readonly IEnrollmentRepository refRepository;
            private readonly IUserRepository userRepository;
            private readonly IProgramQuestionRepository programQuestionRepository;
            private readonly IProgramAnswerRepository programAnswerRepository;
            private readonly IEnumerable<IUserProfileSectionCompletionChecker> userProfileSectionCompletionCheckers;

            public QueryHandler(IEnrollmentRepository refRepository,
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

            public async Task<GetEnrollmentStatusByIdViewModel> Handle(GetEnrollmentStatusByIdQuery request, CancellationToken cancellationToken)
            {
                GetEnrollmentStatusByIdViewModel statusModel = new GetEnrollmentStatusByIdViewModel();
                var enrollment = await refRepository.GetByIdAsync(request.Id, cancellationToken);
                if (enrollment.HasNoValue)
                {
                    throw new Exception();
                }

                var user = (User)await this.userRepository.GetByIdAsync(enrollment.Value.ContactId, cancellationToken);

                if (user.Achievements.Count > 0)
                {
                    statusModel.IsAchievementStepCompleted = true;
                }
                if (user.References.Count > 0 && user.References.Count(x => x.IsCompleted) > 0)
                {
                    statusModel.IsReferenceStepCompleted = true;
                }

                var questions = await this.programQuestionRepository.ListByProgramIdAsync(
                    enrollment.Value.ProgramId, cancellationToken);

                var answers = await this.programAnswerRepository.ListByEnrollmentIdAsync(enrollment.Value.Id, cancellationToken);

                if (questions.Any(q => answers.All(a => a.QuestionId != q.Id || string.IsNullOrEmpty(a.AnswerText))))
                {
                    statusModel.IsQuestionStepCompleted = false;
                }
                else
                {
                    statusModel.IsQuestionStepCompleted = true;
                }

                if (enrollment.Value.PymetricsStatus != null &&
                    enrollment.Value.PymetricsStatus >= 4)
                {
                    statusModel.IsSmartAssessmentStepCompleted = true;
                }

                if (statusModel.IsAchievementStepCompleted && statusModel.IsReferenceStepCompleted && statusModel.IsQuestionStepCompleted && statusModel.IsSmartAssessmentStepCompleted)
                {
                    statusModel.IsAcknowledgmentStepCompleted = true;
                }

                return statusModel;
            }
        }
        #endregion
    }
}
