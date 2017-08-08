using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ZBRA.Impress.Testing
{
    [TestClass]
    public class TestHash
    {
        [TestMethod]
        public void TestHashWithNullValues()
        {
            Assert.AreEqual(0, Hash.Create(null).GetHashCode());
        }

        [TestMethod]
        public void TestHashWithNullables()
        {
            int? testValue = null;
            Assert.AreEqual(0, Hash.Create(testValue).GetHashCode());
            testValue = 3;
            Assert.AreEqual(3, Hash.Create(testValue).GetHashCode());
        }

        [TestMethod]
        public void TestHashWithMaybe()
        {
            Maybe<long> testValue = Maybe<long>.Nothing;
            Assert.AreEqual(0, Hash.Create(testValue).GetHashCode());
            testValue = 2L.ToMaybe();
            Assert.AreEqual(2, Hash.Create(testValue).GetHashCode());
        }

        [TestMethod]
        public void TestHashWithEnumerables()
        {
            var enumerable = new List<string>() { "I'll send", "an SOS", "to the world" };
            var array = enumerable.ToArray();
            var hash1 = Hash.Create(array[0]).Add(array[1]).Add(array[2]).GetHashCode();
            var hash2 = Hash.Create(enumerable).GetHashCode();

            Assert.AreEqual(hash1, hash2);
        }
    }
}
