using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Crypto.Tests
{
	[TestClass]
	[SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
	public class KeyTests
	{
		[TestMethod]
		public void CreateTest()
		{
			const int value = 42;
			var key = new Key<int>(value);

			Assert.AreEqual(value, key.Value);
		}

		[TestMethod]
		public void ToStringTest()
		{
			const int value = 42;
			var key = new Key<int>(value);

			Assert.AreEqual(value.ToString(), key.ToString());
		}

		[TestMethod]
		public void HashCodeTest()
		{
			const int value1 = 1;
			const int value2 = 42;

			var key1 = new Key<int>(value1);
			var key2 = new Key<int>(value1);
			var key3 = new Key<int>(value2);

			Assert.AreEqual(key1.GetHashCode(), key2.GetHashCode());
			Assert.AreNotEqual(key1.GetHashCode(), key3.GetHashCode());
			Assert.AreNotEqual(key2.GetHashCode(), key3.GetHashCode());
		}

		[TestMethod]
		public void EqualsTest()
		{
			const int value1 = 1;
			const int value2 = 42;

			var key1 = new Key<int>(value1);
			var key2 = new Key<int>(value1);
			var key3 = new Key<int>(value2);

			Assert.IsTrue(key1.Equals(key2));
			Assert.IsTrue(key2.Equals(key1));

			Assert.IsFalse(key1.Equals(key3));
			Assert.IsFalse(key2.Equals(key3));

			Assert.IsTrue(key1 == key2);
			Assert.IsTrue(key2 == key1);

			Assert.IsFalse(key1 == key3);
			Assert.IsFalse(key2 == key3);

			Assert.IsFalse(key1 != key2);
			Assert.IsFalse(key2 != key1);

			Assert.IsTrue(key1 != key3);
			Assert.IsTrue(key2 != key3);
		}

		[TestMethod]
		public void EqualsNullTest()
		{
			var key = new Key<int>(42);

			Assert.IsFalse(key.Equals(null));
			Assert.IsFalse(key.Equals((object)null));
			Assert.IsFalse(key == null);
			Assert.IsTrue((Key<int>)null == null);
		}

		[TestMethod]
		public void EqualsOtherTypeTest()
		{
			const int value = 42;
			var key = new Key<int>(value);

			Assert.IsFalse(key.Equals(value));
		}
	}
}
