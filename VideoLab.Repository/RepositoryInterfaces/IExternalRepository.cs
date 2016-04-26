namespace VideoLab.Repository.RepositoryInterfaces
{
    public interface IExternalRepository<out TEntity> : IReadOnlyRepository<TEntity> where TEntity : class
    { }
}