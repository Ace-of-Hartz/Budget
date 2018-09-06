using Budget.dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Budget.dal.i
{
    public interface ITagsRepository
    {
        Task AddTagAsync(int accountId, string tag, string description);
        Task RemoveTagAsync(int id);
        Task UpdateDescriptionAsync(int id, string description);

        Task<IEnumerable<Tags>> GetTagsAsync();
        Task<IEnumerable<Tags>> GetTagsForAccountAsync(int accountId);
        Task<IEnumerable<Tags>> GetTagsThatContainAsync(string tag);
    }
}
