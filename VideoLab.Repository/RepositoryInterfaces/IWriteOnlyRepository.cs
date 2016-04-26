namespace VideoLab.Repository.RepositoryInterfaces
{
    public interface IWriteOnlyRepository<in TEntity> where TEntity : class
    {
        void Save (TEntity entityToSave);
    }
}