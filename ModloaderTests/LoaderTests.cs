using ModLoader;
using NUnit.Framework;

namespace ModloaderTests
{
    public class LoaderTests
    {
        private Loader loader;
        private SFSModDependencie[] dependencies;

        [SetUp]
        public void Setup()
        {
            this.loader = new Loader();
        }

        [Test]
        public void Test1()
        {

            Assert.Pass();
        }
    }
}