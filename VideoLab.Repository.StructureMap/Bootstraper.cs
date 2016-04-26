using System.Collections.Generic;
using Google.Apis.YouTube.v3.Data;
using MongoDB.Bson;
using StructureMap;
using StructureMap.Graph.Scanning;
using VideoLab.Repository.APIStorage.DataStores;
using VideoLab.Repository.APIStorage.Repositories;
using VideoLab.Repository.DataStoreInterfaces;
using VideoLab.Repository.RepositoryInterfaces;
using VideoLab.Repository.VideoLabStorage;

namespace VideoLab.Repository.StructureMap
{
    public static class Bootstraper
    {
        private static IContainer _container;

        public static IContainer Bootstrap(IContainer container = null)
        {
            _container = container ?? new Container();

            _container.Configure(conf =>
            {
                conf.Scan(c => c.WithDefaultConventions());
                conf.AddRegistry<DataStoreRegistry>();
                conf.AddRegistry<VideoLabRepositoryRegistry>();
                conf.AddRegistry<ExternalRepositoryRegistry>();
                conf.AddRegistry<SearchableRepositoryRegistry>();
                conf.AddRegistry<ReadOnlyRepositoryRegistry>();
                conf.For(typeof (IYouTubeRepository)).Use(typeof(YouTubeRepository));
            });

            return _container;
        }
    }

    public class ReadOnlyRepositoryRegistry : Registry
    {
        public ReadOnlyRepositoryRegistry()
        {
            Scan(s =>
            {
                s.AssemblyContainingType(typeof(IReadOnlyRepository<>));
                s.AddAllTypesOf(typeof(IReadOnlyRepository<>));
            });
        }
    }

    public class SearchableRepositoryRegistry : Registry
    {
        public SearchableRepositoryRegistry()
        {
            Scan(s =>
            {
                s.AssemblyContainingType(typeof(ISearchableRepository<>));
                s.AddAllTypesOf(typeof(ISearchableRepository<>));
            });
        }
    }

    public class ExternalRepositoryRegistry : Registry
    {
        public ExternalRepositoryRegistry()
        {
            Scan(s =>
            {
                s.AssemblyContainingType(typeof(IExternalRepository<>));
                s.AddAllTypesOf(typeof(IExternalRepository<>));
                s.ConnectImplementationsToTypesClosing(typeof (IExternalRepository<>));
            });
        }
    }

    public class VideoLabRepositoryRegistry : Registry
    {
        public VideoLabRepositoryRegistry()
        {
            Scan(s =>
            {
                s.AssemblyContainingType(typeof(IVideoLabRepository<>));
                s.AddAllTypesOf(typeof(IVideoLabRepository<>));
            });
        }
    }

    public class DataStoreRegistry : Registry
    {
        public DataStoreRegistry()
        {
            Scan(s =>
            {
                s.AssemblyContainingType<IVideoDataStore>();
                s.AddAllTypesOf<IVideoDataStore>();
            });
        }
    }
}
