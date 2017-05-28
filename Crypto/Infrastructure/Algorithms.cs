using System;

namespace Crypto.Infrastructure
{
	public static class Algorithms
	{
		/// <summary>
		/// Finds the greatest common denomintor of two numbers.
		/// </summary>
		/// <param name="a">The first number.</param>
		/// <param name="b">The second number.</param>
		/// <returns>The greatest common denomintor of a and b.</returns>
		public static (int Value, int X, int Y) GCD(
			int a, int b)
		{
			(int Value, int X, int Y) GCDInternal(
				int r1, int r2, int x1, int x2, int y1, int y2)
			{
				int r3 = r1 - r2 * (r1 / r2);
				int x3 = x1 - x2 * (r1 / r2);
				int y3 = y1 - y2 * (r1 / r2);

				return r3 != 0
					? GCDInternal(r2, r3, x2, x3, y2, y3)
					: (r2, x2, y2);
			}

			var result = GCDInternal(Math.Max(a, b), Math.Min(a, b), 1, 0, 0, 1);

			return a > b
				? result
				: (result.Value, result.Y, result.X);
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
