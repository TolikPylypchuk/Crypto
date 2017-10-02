using System.Collections.Generic;
using System.Windows.Controls;

using Crypto.Protocols;

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

		public string SSCFrom { get; set; }
		public string SSCTo { get; set; }

		public string VisenereKey { get; set; }

		public string RSAP { get; set; }
		public string RSAQ { get; set; }
		public string RSAN { get; set; }
		public string RSAE { get; set; }
		public string RSAD { get; set; }

		public bool ExchangeInProgress { get; set; }
		public DiffieHellmanExchanger ExchengerA { get; set; }
		public DiffieHellmanExchanger ExchengerB { get; set; }
		
		public string DHP { get; set; }
		public string DHQ { get; set; }
		public string DHA { get; set; }
		public string DHAToSend { get; set; }
		public string DHB { get; set; }
		public string DHBToSend { get; set; }

		public string SDESKey { get; set; }

		public List<Alphabet> AlphabetsList { get; set; } = new List<Alphabet>
		{
			Alphabets.ASCII,
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
