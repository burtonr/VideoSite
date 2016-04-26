namespace VideoLab.Repository.RepositoryInterfaces
{
    public interface IReadOnlyRepository<out TEntity> where TEntity : class
    {
        TEntity Get (string videoId);
    }
}