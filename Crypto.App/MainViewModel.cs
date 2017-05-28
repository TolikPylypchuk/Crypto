using System.Collections.Generic;
using System.Windows.Controls;

namespace Crypto.App
{
	public class MainViewModel
	{
		public Alphabet Alphabet { get; set; }
		public bool IsStrict { get; set; }
		public TabItem CurrentTab { get; set; }
		public string Action { get; set; }
		public string Plaintext { get; set; }
		public string Ciphertext { get; set; }

		public string Shift { get; set; }

		public List<Alphabet> AlphabetsList { get; set; } = new List<Alphabet>
		{
			Alphabets.English,
			Alphabets.EnglishLower,
			Alphabets.EnglishUpper,
			Alphabets.EnglishWithPunctuation + Alphabets.DecimalDigits,
			Alphabets.Ukrainian,
			Alphabets.UkrainianLower,
			Alphabets.UkrainianUpper,
			Alphabets.UkrainianWithPunctuation + Alphabets.DecimalDigits,
		};

		public List<string> Actions { get; set; } = new List<string>
		{
			"Зашифрувати",
			"Дешифрувати"
		};
	}
}
