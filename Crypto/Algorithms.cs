namespace Crypto
{
	public static class Algorithms
	{
		/// <summary>
		/// Finds the greatest common denomintor of two numbers.
		/// </summary>
		/// <param name="a">The first number.</param>
		/// <param name="b">The second number.</param>
		/// <returns>The greatest common denomintor of a and b.</returns>
		public static int GCD(int a, int b)
		{
			while (a != 0 && b != 0)
			{
				if (a > b)
				{
					a %= b;
				} else
				{
					b %= a;
				}
			}

			return a == 0 ? b : a;
		}

		/// <summary>
		/// Finds m^p mod n.
		/// </summary>
		/// <param name="m">The divident.</param>
		/// <param name="p">The power.</param>
		/// <param name="n">The modulo</param>
		/// <returns>m^p mod n.</returns>
		public static int Modulo(int m, int p, int n)
		{
			int value = m % n;
			int result = (p & 1) != 0 ? value : 1;

			for (int i = 2; i <= p; i <<= 1)
			{
				value = (value * value) % n;

				if ((p & i) != 0)
				{
					result *= value;
					result %= n;
				}
			}

			return result;
		}
	}
}
