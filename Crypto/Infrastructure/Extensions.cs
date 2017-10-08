using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Crypto.Infrastructure
{
	/// <summary>
	/// Contains extension methods for various classes.
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Splits the string into equal parts.
		/// </summary>
		/// <param name="str">The string to split.</param>
		/// <param name="maxLength">The length of each part.</param>
		/// <returns>The splitted string</returns>
		public static IEnumerable<string> SplitByLength(
			this string str,
			int maxLength)
		{
			for (int index = 0; index < str.Length; index += maxLength)
			{
				yield return str.Substring(
					index, Math.Min(maxLength, str.Length - index));
			}
		}

		/// <summary>
		/// Returns the number of digits of the specified number.
		/// </summary>
		/// <param name="n">The specified number.</param>
		/// <returns>The number of digits of n.</returns>
		public static int NumberOfDigits(this int n)
			=> (int)Math.Log10(n) + 1;

		/// <summary>
		/// Checks whether the number is prime.
		/// </summary>
		/// <param name="n">The number to check.</param>
		/// <returns>
		/// <c>true</c> if the number is prime.
		/// Otherwise, false.
		/// </returns>
		public static bool IsPrime(this int n)
		{
			int root = (int)Math.Sqrt(n);

			for (int i = 2; i <= root; i++)
			{
				if (n % i == 0)
				{
					return false;
				}
			}

			return true;
		}
		
		/// <summary>
		/// Converts a bit array to a byte.
		/// </summary>
		/// <param name="input">The input array.</param>
		/// <returns>The resulting byte.</returns>
		public static byte ToByte(this BitArray input)
		{
			byte result = 0;

			for (int i = 0; i < 8; i++)
			{
				if (input[i])
				{
					result |= (byte)(1 << i);
				}
			}

			return result;
		}

		/// <summary>
		/// Permutes the elements of a bit array.
		/// </summary>
		/// <param name="input">The input array.</param>
		/// <param name="indices">
		/// The new indices the input array elements.
		/// </param>
		/// <returns>The permuted array.</returns>
		public static BitArray Permute(
			this BitArray input,
			params int[] indices)
		{
			var output = new BitArray(indices.Length);

			for (int i = 0; i < indices.Length; i++)
			{
				output[i] = input[indices[i]];
			}

			return output;
		}
		
		/// <summary>
		/// Splits a bit array into two bit arrays.
		/// </summary>
		/// <param name="input">The input</param>
		/// <returns></returns>
		public static (BitArray, BitArray) Split(this BitArray input)
		{
			int half = input.Length / 2;
			return (input.Permute(Enumerable.Range(0, half).ToArray()),
					input.Permute(Enumerable.Range(half, half).ToArray()));
		}
		
		/// <summary>
		/// Concatenates the bits of the other bit array to a bit array.
		/// </summary>
		/// <param name="input">The input bit array.</param>
		/// <param name="other">The other bit array.</param>
		/// <returns>The combined bit arrays.</returns>
		public static BitArray Concat(this BitArray input, BitArray other)
		{
			int len = input.Length;

			var result = new BitArray(input)
			{
				Length = len + other.Length
			};

			for (int i = len; i < result.Length; i++)
			{
				result[i] = other[i - len];
			}

			return result;
		}

		/// <summary>
		/// Performs the cyclic transformation of a bit array
		/// using a specified key.
		/// </summary>
		/// <param name="input">The input bit array.</param>
		/// <param name="key">The key to use.</param>
		/// <returns>The transformed bit array.</returns>
		public static BitArray CyclicTransform(
			this BitArray input,
			BitArray key)
		{
			var (left, right) = input.Split();
			return right.CyclicTransformHalf(key).Xor(left).Concat(right);
		}

		/// <summary>
		/// Performs the cyclic transformation of a half of a bit array
		/// using a specified key.
		/// </summary>
		/// <param name="input">The input bit array.</param>
		/// <param name="key">The key to use.</param>
		/// <returns>The transformed bit array.</returns>
		private static BitArray CyclicTransformHalf(
			this BitArray input,
			BitArray key)
		{
			var (left, right) =
				input.Permute(3, 0, 1, 2, 1, 2, 3, 0)
				     .Xor(key)
					 .Split();

			return left.MatrixTransformLeft()
				       .Concat(right.MatrixTransformRight())
				       .Permute(1, 3, 2, 0);
		}

		/// <summary>
		/// Performs the matrix transformation of the left half of input.
		/// </summary>
		/// <param name="input">The left half of the input.</param>
		/// <returns>The transformed bit array.</returns>
		private static BitArray MatrixTransformLeft(this BitArray input)
			=> input.MatrixTransform(
				new[,]
				{
					{ 1, 0, 3, 2 },
					{ 3, 2, 1, 0 },
					{ 0, 2, 1, 3 },
					{ 3, 1, 3, 1 }
				});

		/// <summary>
		/// Performs the matrix transformation of the right half of input.
		/// </summary>
		/// <param name="input">The right half of the input.</param>
		/// <returns>The transformed bit array.</returns>
		private static BitArray MatrixTransformRight(this BitArray input)
			=> input.MatrixTransform(
				new[,]
				{
					{ 1, 1, 2, 3 },
					{ 2, 0, 1, 3 },
					{ 3, 0, 1, 0 },
					{ 2, 1, 0, 3 }
				});

		/// <summary>
		/// Performs the matrix transformation of a half of the input.
		/// </summary>
		/// <param name="input">The half of the input.</param>
		/// <param name="matrix">
		/// The matrix to use in the transformation.
		/// </param>
		/// <returns>The transformed bit array.</returns>
		private static BitArray MatrixTransform(
			this BitArray input,
			int[,] matrix)
		{
			var row = new BitArray(8, false) { [0] = input[0], [1] = input[3] };
			var col = new BitArray(8, false) { [0] = input[1], [1] = input[2] };

			int value = matrix[row.ToByte(), col.ToByte()];

			return new BitArray(2)
			{
				[0] = (value & 2) != 0,
				[1] = (value & 1) != 0
			};
		}
	}
}
