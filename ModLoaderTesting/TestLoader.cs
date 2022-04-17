using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModLoader;

namespace ModLoaderTesting
{
    [TestClass]
    public class TestLoader
    {
        private PrivateObject _loader;

        [TestInitialize]
        public void Init()
        {
            Loader loader = new Loader();
            this._loader = new PrivateObject(loader);
        }

        [TestMethod]
        public void DifferentMajorVersion_verifyVersion_ReturnFalse()
        {
            // Arrange
            string currentVersion = "v2.1.1";
            string targetVersion = "v1.x.x";
            object[] toTest = new object[] { targetVersion, currentVersion, true };

            // Act
            bool result = (bool)this._loader.Invoke("verifyVersion", toTest);

            // Assert 
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TargetCompatibleWithAllMinorVersions_verifyVersion_ReturnTrue()
        {
            // Arrange
            string currentVersion = "v1.9.1";
            string targetVersion = "v1.x.x";
            object[] toTest = new object[] { targetVersion, currentVersion, true };

            // Act
            bool result = (bool)this._loader.Invoke("verifyVersion", toTest);

            // Assert 
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TargetHasX_verifyVersion_ReturnTrue()
        {
            // Arrange
            string currentVersion = "v1.1.1";
            string targetVersion = "v1.1.x";
            object[] toTest = new object[] { targetVersion, currentVersion, true };

            // Act
            bool result = (bool)this._loader.Invoke("verifyVersion", toTest);

            // Assert 
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TargetLessThatCurrent_verifyVersion_ReturnTrue()
        {
            // Arrange
            string currentVersion = "v1.3.0";
            string targetVersion = "v1.2.2";
            object[] toTest = new object[] { targetVersion, currentVersion, true };

            // Act
            bool result = (bool) this._loader.Invoke("verifyVersion", toTest);

            // Assert 
            Assert.IsTrue(result);
        }


        [TestMethod]
        public void TargetGreaterThatCurrent_verifyVersion_ReturnFalse()
        {
            // Arrange
            string currentVersion = "v1.1.1";
            string targetVersion = "v1.1.2";
            object[] toTest = new object[] { targetVersion, currentVersion, true };

            // Act
            bool result = (bool)this._loader.Invoke("verifyVersion", toTest);

            // Assert 
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TargetEqualCurrent_verifyVersion_ReturnTrue()
        {
            // Arrange
            string currentVersion = "v1.1.1";
            string targetVersion = "v1.1.1";
            object[] toTest = new object[] { targetVersion, currentVersion, true };

            // Act
            bool result = (bool)this._loader.Invoke("verifyVersion", toTest);

            // Assert 
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CurrentInvalidFormat_verifyVersion_ReturnFalse()
        {
            // Arrange
            string currentVersion = "v1.1.1 beta";
            string targetVersion = "v1.1.1";
            object[] toTest = new object[] { targetVersion, currentVersion, true };

            // Act
            bool result = (bool)this._loader.Invoke("verifyVersion", toTest);

            // Assert 
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TargetInvalidFormat_verifyVersion_ReturnFalse()
        {
            // Arrange
            string currentVersion = "v1.1.1";
            string targetVersion = "v1.1.1 beta";
            object[] toTest = new object[] { targetVersion, currentVersion, true };

            // Act
            bool result = (bool)this._loader.Invoke("verifyVersion", toTest);

            // Assert 
            Assert.IsFalse(result);
        }

        
    }
}
