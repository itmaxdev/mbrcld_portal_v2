using Mbrcld.Application.Exceptions;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.ProgramAnswers.Commands
{
    public sealed class AddEditProgramAnswerCommand : IRequest<Result>
    {
        #region Command
        public Guid EnrollmentId { get; }
        public IReadOnlyDictionary<Guid, string> Answers { get; }

        public AddEditProgramAnswerCommand(Guid enrollmentId, IReadOnlyDictionary<Guid, string> answers)
        {
            this.EnrollmentId = enrollmentId;
            this.Answers = answers;
        }
        #endregion

        #region Command handler
        public sealed class CommandHandler : IRequestHandler<AddEditProgramAnswerCommand, Result>
        {
            private readonly IProgramAnswerRepository programAnswerRepository;
            private readonly IEnrollmentRepository enrollmentRepository;

            public CommandHandler(IProgramAnswerRepository programAnswerRepository, IEnrollmentRepository enrollmentRepository)
            {
                this.programAnswerRepository = programAnswerRepository;
                this.enrollmentRepository = enrollmentRepository;
            }

            public async Task<Result> Handle(AddEditProgramAnswerCommand request, CancellationToken cancellationToken)
            {
                var enrollment = await this.enrollmentRepository.GetByIdAsync(request.EnrollmentId, cancellationToken);
                if (enrollment.HasNoValue)
                {
                    return Result.Failure($"Enrollment with id {request.EnrollmentId} not found");
                }

                var existingAnswers = await this.programAnswerRepository.ListByEnrollmentIdAsync(request.EnrollmentId);

                foreach (var (questionId, answerText) in request.Answers)
                {
                    var existingAnswer = existingAnswers.FirstOrDefault(x => x.QuestionId == questionId);

                    if (existingAnswer == null)
                    {
                        var answer = ProgramAnswer.Create(
                            questionId,
                            request.EnrollmentId,
                            answerText
                            );

                        await this.programAnswerRepository.CreateAsync(answer, cancellationToken);
                    }
                    else if (!string.Equals(answerText, existingAnswer.AnswerText))
                    {
                        existingAnswer.AnswerText = answerText;
                        await this.programAnswerRepository.UpdateAsync(existingAnswer, cancellationToken);
                    }
                }

                return Result.Success();
            }
        }
        #endregion
    }
}
