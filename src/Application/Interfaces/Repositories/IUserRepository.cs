using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task CreateAsync(User user, CancellationToken cancellationToken = default);
        Task<User> GetByEmiratesIdAsync(string emiratesId, bool isUpdate = false, Guid? id = null, CancellationToken cancellationToken = default);
        Task UpdateAsync(User user, CancellationToken cancellationToken = default);
        Task DeleteAsync(User user, CancellationToken cancellationToken = default);
        Task<Maybe<User>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Maybe<User>> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<Maybe<User>> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default);
        Task<IList<User>> ListDirectManagerApplicantsAsync(Guid userid, CancellationToken cancellationToken = default);
        Task<IList<User>> SearchAlumniAsync(string search, CancellationToken cancellationToken = default);
        Task<IList<User>> SearchAlumniCriteriaAsync(Guid? programId, Guid? sectorId, int? year, Guid userId, CancellationToken cancellationToken = default);
        Task<IList<User>> ListAlumniUsersForChatAsync(Guid userid, CancellationToken cancellationToken = default);
    }
}
