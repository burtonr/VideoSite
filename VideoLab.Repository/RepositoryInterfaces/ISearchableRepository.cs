using System.Collections.Generic;

namespace VideoLab.Repository.RepositoryInterfaces
{
    public interface ISearchableRepository<TEntity>
    {
        IList<TEntity> Search(IList<string> searchParams, int maxResults);

        IList<TEntity> Search(string title, int maxResults);
    }
}