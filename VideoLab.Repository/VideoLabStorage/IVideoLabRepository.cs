using VideoLab.Repository.RepositoryInterfaces;

namespace VideoLab.Repository.VideoLabStorage
{
    public interface IVideoLabRepository<TEntity> : IReadOnlyRepository<TEntity>, IWriteOnlyRepository<TEntity>, ISearchableRepository<TEntity> where TEntity : class 
    { }
}