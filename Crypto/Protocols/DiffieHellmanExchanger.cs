using System;
using System.Threading.Tasks;

using Crypto.Infrastructure;
using Crypto.Keys;

namespace Crypto.Protocols
{
	/// <summary>
	/// Represents the Diffie-Hellman protocol for exchanging keys.
	/// </summary>
	public class DiffieHellmanExchanger
	{
		private Func<Task<Key<int>>> externalKeySource;
		
		/// <summary>
		/// Initializes a new instance of the DiffieHellmanProtocol class.
		/// </summary>
		/// <param name="externalKeySource">
		/// The source of the external key.
		/// </param>
		public DiffieHellmanExchanger(
			Func<Task<Key<int>>> externalKeySource)
		{
			this.externalKeySource = externalKeySource;
		}

		/// <summary>
		/// Gets or sets the first initial number of the exchanger.
		/// </summary>
		public int P { get; private set; }

		/// <summary>
		/// Gets or sets the second initial number of the exchanger.
		/// </summary>
		public int Q { get; private set; }

		/// <summary>
		/// Gets or sets the participant's part of the exchanger.
		/// </summary>
		private int Part { get; set; }

		/// <summary>
		/// Sets the inital numbers of the exchanger.
		/// </summary>
		/// <param name="p">The first initial number</param>
		/// <param name="q">The second initial number</param>
		/// <returns>
		/// <c>false</c>, if the first number is not prime
		/// or the second number is greater than the first.
		/// Otherwise, <c>true</c>.
		/// </returns>
		public bool TrySetPQ(int p, int q)
		{
			if (!p.IsPrime() || q >= p)
			{
				return false;
			}

			this.P = p;
			this.Q = q;

			return true;
		}

		/// <summary>
		/// Generates and returns the participant's part of the key.
		/// </summary>
		/// <returns>The participant's part of the key.</returns>
		public int GeneratePart()
		{
			this.Part = new Random().Next(2, this.P - 1);

			int result = Algorithms.Modulo(this.Q, this.Part, this.P);

			this.OnKeyPartGenerated(result);

			return result;
		}

		public async Task<Key<int>> GenerateKey()
		{
			var otherPart = await this.externalKeySource();

			return new Key<int>(
				Algorithms.Modulo(otherPart.Value, this.Part, this.P));
		}

		protected virtual void OnKeyPartGenerated(int value)
		{
			this.KeyPartGenerated?.Invoke(
				this, new KeyPartGeneratedEventArgs(new Key<int>(value)));
		}

		public event EventHandler<KeyPartGeneratedEventArgs> KeyPartGenerated;
	}
}
