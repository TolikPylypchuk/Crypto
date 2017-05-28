using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Crypto.Tests
{
	[TestClass]
	public class AlgorithmsTests
	{
		[TestMethod]
		public void GCDTest()
		{
			Assert.AreEqual(6, Algorithms.GCD(12, 18));
		}

		[TestMethod]
		public void ModuloTest1()
		{
			Assert.AreEqual(11, Algorithms.Modulo(88, 7, 187));
		}

		[TestMethod]
		public void ModuloTest2()
		{
			Assert.AreEqual(88, Algorithms.Modulo(11, 23, 187));
		}

		[TestMethod]
		public void ModuloAbsorbtionTest()
		{
			Assert.AreEqual(
				11,
				Algorithms.Modulo(Algorithms.Modulo(11, 23, 187), 7, 187));
		}
	}
}
