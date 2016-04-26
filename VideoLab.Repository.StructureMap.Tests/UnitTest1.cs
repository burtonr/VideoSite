using System.Collections.Generic;
using NUnit.Framework;
using StructureMap;
using VideoLab.Repository.Services;


namespace VideoLab.Repository.StructureMap.Tests
{
    [TestFixture]
    public class UnitTest1
    {
        private static IContainer _container;

        [OneTimeSetUp]
        public static void Booter()
        {
            _container = Bootstraper.Bootstrap();
        }

        [Test]
        public void TestMethod1()
        {
            var service = _container.GetInstance<VideoDataStoreService>();

            var tags = new List<string> {"Stuff"};

            var results = service.ListVideosByTags(tags);

            Assert.NotNull(results);
        }
    }
}
