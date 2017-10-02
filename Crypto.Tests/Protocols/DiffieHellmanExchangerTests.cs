using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Crypto.Infrastructure;

namespace Crypto.Protocols
{
	[TestClass]
	[SuppressMessage("ReSharper", "UnusedVariable")]
	public class DiffieHellmanExchangerTests
	{
		[TestMethod]
		public void SetPQValidTest()
		{
			const int p = 17;
			const int q = 11;

			var exchanger = new DiffieHellmanExchanger();

			bool result = exchanger.TrySetPQ(p, q);

			Assert.IsTrue(result);
			Assert.AreEqual(p, exchanger.P);
			Assert.AreEqual(q, exchanger.Q);
		}

		[TestMethod]
		public void SetPQNotPrimeTest()
		{
			const int p = 16;
			const int q = 11;

			var exchanger = new DiffieHellmanExchanger();

			bool result = exchanger.TrySetPQ(p, q);

			Assert.IsFalse(result);
		}

		[TestMethod]
		public void SetPQNotLessTest()
		{
			const int p = 17;
			const int q = 18;

			var exchanger = new DiffieHellmanExchanger();

			bool result = exchanger.TrySetPQ(p, q);

			Assert.IsFalse(result);
		}

		[TestMethod]
		public void GeneratePartTest()
		{
			const int p = 17;
			const int q = 11;

			var exchanger = new DiffieHellmanExchanger();
			exchanger.TrySetPQ(p, q);

			int result = exchanger.GeneratePart();

			Assert.AreEqual(
				result,
				Algorithms.Modulo(exchanger.Q, exchanger.Part, exchanger.P));
		}

		[TestMethod]
		public void GeneratePartEventTest()
		{
			const int p = 17;
			const int q = 11;
			int part = -1;

			var exchanger = new DiffieHellmanExchanger();
			exchanger.TrySetPQ(p, q);

			exchanger.KeyPartGenerated += (sender, e) =>
			{
				part = e.Key.Value;
			};

			int result = exchanger.GeneratePart();
			
			Assert.AreEqual(result, part);
		}

		[TestMethod]
		public void GenerateKeyTest()
		{
			const int p = 17;
			const int q = 11;
			const int otherPart = 2;

			var exchanger = new DiffieHellmanExchanger();

			exchanger.TrySetPQ(p, q);
			exchanger.ExternalKeySource =
				() => Task.FromResult(new Key<int>(otherPart));

			int result = exchanger.GeneratePart();

			var key = exchanger.GenerateKey().Result;

			Assert.AreEqual(
				key.Value,
				Algorithms.Modulo(otherPart, exchanger.Part, exchanger.P));
		}
	}
}
