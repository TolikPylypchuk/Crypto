using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Crypto
{
	[TestClass]
	[SuppressMessage("ReSharper", "UnusedVariable")]
	[SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
	public class AlphabetTests
	{
		[TestMethod]
		public void LengthTest()
		{
			const string text = "one";
			var alphabet = new Alphabet(text);

			Assert.AreEqual(text.Length, alphabet.Length);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullCharsTest()
		{
			var alphabet = new Alphabet(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void EmptyCharsTest()
		{
			var alphabet = new Alphabet(new char[] { });
		}

		[TestMethod]
		public void CharAtTest()
		{
			const string text = "one";
			var alphabet = new Alphabet(text);

			for (int i = 0; i < text.Length; i++)
			{
				Assert.AreEqual(text[i], alphabet.CharAt(i));
			}
		}

		[TestMethod]
		public void IndexerIntTest()
		{
			const string text = "one";
			var alphabet = new Alphabet(text);

			for (int i = 0; i < text.Length; i++)
			{
				Assert.AreEqual(text[i], alphabet[i]);
				Assert.AreEqual(alphabet[i], alphabet.CharAt(i));
			}
		}

		[TestMethod]
		public void IndexOfTest()
		{
			const string text = "one";
			var alphabet = new Alphabet(text);

			for (int i = 0; i < text.Length; i++)
			{
				Assert.AreEqual(i, alphabet.IndexOf(text[i]));
			}
		}

		[TestMethod]
		public void IndexerCharTest()
		{
			const string text = "one";
			var alphabet = new Alphabet(text);

			for (int i = 0; i < text.Length; i++)
			{
				Assert.AreEqual(i, alphabet[text[i]]);
				Assert.AreEqual(alphabet[text[i]], alphabet.IndexOf(text[i]));
			}
		}

		[TestMethod]
		public void AbsobtionTest()
		{
			const string text = "one";
			var alphabet = new Alphabet(text);

			for (int i = 0; i < text.Length; i++)
			{
				Assert.AreEqual(i, alphabet.IndexOf(alphabet[i]));
				Assert.AreEqual(text[i], alphabet.CharAt(
					alphabet.IndexOf(text[i])));
			}
		}

		[TestMethod]
		public void DistinctTest()
		{
			const string text = "test";
			var alphabet = new Alphabet(text);

			var distinctText = text.Distinct().ToList();

			for (int i = 0; i < distinctText.Count; i++)
			{
				Assert.AreEqual(distinctText[i], alphabet.CharAt(i));
			}
		}

		[TestMethod]
		public void GetEnumeratorTest()
		{
			const string text = "abc";

			var alphabet = new Alphabet(text);

			var enumerable = (IEnumerable)alphabet;

			int index = 0;
			foreach (var item in enumerable)
			{
				Assert.IsInstanceOfType(item, typeof(char));
				Assert.AreEqual(text[index++], item);
			}
		}

		[TestMethod]
		public void GetEnumeratorGenericTest()
		{
			const string text = "abc";

			var alphabet = new Alphabet(text);

			var enumerable = (IEnumerable<char>)alphabet;

			int index = 0;
			foreach (char item in enumerable)
			{
				Assert.AreEqual(text[index++], item);
			}
		}

		[TestMethod]
		public void ToStringTest()
		{
			const string text = "one";
			var alphabet = new Alphabet(text);

			Assert.AreEqual(text, alphabet.ToString());
		}

		[TestMethod]
		public void HashCodeTest()
		{
			const string text1 = "one";
			const string text2 = "qwerty";

			var alphabet1 = new Alphabet(text1);
			var alphabet2 = new Alphabet(text1);
			var alphabet3 = new Alphabet(text2);

			Assert.AreEqual(alphabet1.GetHashCode(), alphabet2.GetHashCode());
			Assert.AreNotEqual(alphabet1.GetHashCode(), alphabet3.GetHashCode());
			Assert.AreNotEqual(alphabet2.GetHashCode(), alphabet3.GetHashCode());
		}

		[TestMethod]
		public void EqualsTest()
		{
			const string text1 = "one";
			const string text2 = "qwerty";

			var alphabet1 = new Alphabet(text1);
			var alphabet2 = new Alphabet(text1);
			var alphabet3 = new Alphabet(text2);

			Assert.IsTrue(alphabet1.Equals(alphabet2));
			Assert.IsTrue(alphabet2.Equals(alphabet1));

			Assert.IsFalse(alphabet1.Equals(alphabet3));
			Assert.IsFalse(alphabet2.Equals(alphabet3));

			Assert.IsTrue(alphabet1 == alphabet2);
			Assert.IsTrue(alphabet2 == alphabet1);

			Assert.IsFalse(alphabet1 == alphabet3);
			Assert.IsFalse(alphabet2 == alphabet3);

			Assert.IsFalse(alphabet1 != alphabet2);
			Assert.IsFalse(alphabet2 != alphabet1);

			Assert.IsTrue(alphabet1 != alphabet3);
			Assert.IsTrue(alphabet2 != alphabet3);
		}

		[TestMethod]
		public void EqualsNullTest()
		{
			var alphabet = new Alphabet("one");

			Assert.IsFalse(alphabet.Equals(null));
			Assert.IsFalse(alphabet.Equals((object)null));
			Assert.IsFalse(alphabet == null);
			Assert.IsTrue((Alphabet)null == null);
		}

		[TestMethod]
		public void EqualsOtherTypeTest()
		{
			const string text = "one";
			var alphabet = new Alphabet(text);

			Assert.IsFalse(alphabet.Equals(text));
		}

		[TestMethod]
		public void UnionTest()
		{
			const string text1 = "one";
			const string text2 = "qwerty";

			var mergedText = (text1 + text2).Distinct().ToList();

			var alphabet1 = new Alphabet(text1);
			var alphabet2 = new Alphabet(text2);

			var result = Alphabet.Union(alphabet1, alphabet2);

			for (int i = 0; i < mergedText.Count; i++)
			{
				Assert.AreEqual(mergedText[i], result.CharAt(i));
			}
		}

		[TestMethod]
		public void UnionNotCommutativeTest()
		{
			const string text1 = "one";
			const string text2 = "qwerty";

			var mergedText = (text1 + text2).Distinct().ToList();

			var alphabet1 = new Alphabet(text1);
			var alphabet2 = new Alphabet(text2);

			var union1 = Alphabet.Union(alphabet1, alphabet2);
			var union2 = Alphabet.Union(alphabet2, alphabet1);

			Assert.AreNotEqual(union1, union2);
		}

		[TestMethod]
		public void PlusTest()
		{
			const string text1 = "one";
			const string text2 = "qwerty";

			var mergedText = (text1 + text2).Distinct().ToList();

			var alphabet1 = new Alphabet(text1);
			var alphabet2 = new Alphabet(text2);

			var result = alphabet1 + alphabet2;

			for (int i = 0; i < mergedText.Count; i++)
			{
				Assert.AreEqual(mergedText[i], result.CharAt(i));
			}
		}

		[TestMethod]
		public void PlusNotCommutativeTest()
		{
			const string text1 = "one";
			const string text2 = "qwerty";

			var mergedText = (text1 + text2).Distinct().ToList();

			var alphabet1 = new Alphabet(text1);
			var alphabet2 = new Alphabet(text2);

			var union1 = alphabet1 + alphabet2;
			var union2 = alphabet2 + alphabet1;

			Assert.AreNotEqual(union1, union2);
		}

		[TestMethod]
		public void BelongsTrueTest()
		{
			const string text = "aababba";
			var alphabet = new Alphabet("ab");

			Assert.IsTrue(alphabet.Belongs(text));
		}

		[TestMethod]
		public void BelongsFalseTest()
		{
			const string text = "aabcbba";
			var alphabet = new Alphabet("ab");

			Assert.IsFalse(alphabet.Belongs(text));
		}

		[TestMethod]
		public void ContainsTrueTest()
		{
			var alphabet = new Alphabet("ab");
			Assert.IsTrue(alphabet.Contains('a'));
		}

		[TestMethod]
		public void ContainsFalseTest()
		{
			var alphabet = new Alphabet("ab");
			Assert.IsFalse(alphabet.Contains('c'));
		}
	}
}
