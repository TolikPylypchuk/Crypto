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
		/// <summary>
		/// Gets or sets the external key source.
		/// </summary>
		public Func<Task<Key<int>>> ExternalKeySource { get; set; }
		
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
		public int Part { get; private set; }

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

		/// <summary>
		/// Generates the key from this exchanger's part and the other part.
		/// </summary>
		/// <returns>
		/// The generated key.
		/// </returns>
		public async Task<Key<int>> GenerateKey()
		{
			var otherPart = await this.ExternalKeySource();

			return new Key<int>(
				Algorithms.Modulo(otherPart.Value, this.Part, this.P));
		}

		/// <summary>
		/// Raises the KeyPartGenerated event.
		/// </summary>
		/// <param name="value">The value of the key part.</param>
		protected virtual void OnKeyPartGenerated(int value)
		{
			this.KeyPartGenerated?.Invoke(
				this, new KeyPartGeneratedEventArgs(new Key<int>(value)));
		}

		/// <summary>
		/// Raised when this exchanger's key part is generated.
		/// </summary>
		public event EventHandler<KeyPartGeneratedEventArgs> KeyPartGenerated;
	}
}
