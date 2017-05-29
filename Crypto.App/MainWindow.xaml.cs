using System;
using System.Threading.Tasks;
using System.Windows;

using Crypto.Keys;
using Crypto.Protocols;
using Crypto.Systems;

using SimpleSubstitutionKey = Crypto.Key<(string From, string To)>;
using RSAKey = Crypto.Key<(int N, int Value)>;

namespace Crypto.App
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml.
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			this.InitializeComponent();
		}

		private void ActionButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				switch (this.model.CurrentTab.Header)
				{
					case "Шифр Цезаря":
						this.ExecuteCaesarCipher();
						break;
					case "Шифр простої заміни":
						this.ExecuteSimpleSubstitutionSipher();
						break;
					case "Шифр Віженера":
						this.ExecuteVisenereCipher();
						break;
					case "Шифр RSA":
						this.ExecuteRSA();
						break;
				}
			} catch
			{
				MessageBox.Show(
					"Неправильні дані.",
					"Помилка",
					MessageBoxButton.OK,
					MessageBoxImage.Error);
			}
}

		private void ExecuteCaesarCipher()
		{
			int shift = Int32.Parse(this.model.Shift);
			
			var cipher = new CaesarCipher(this.model.Alphabet)
			{
				IsStrict = this.model.IsStrict
			};

			if (this.model.Action == "Зашифрувати")
			{
				this.Encrypt(cipher, new Key<int>(shift));
			} else
			{
				this.Decrypt(
					cipher,
					KeyGenerator.GetCaesarDecryptionKey(
						new Key<int>(shift)));
			}
			
		}

		private void ExecuteSimpleSubstitutionSipher()
		{
			var cipher = new SimpleSubstitutionCipher(this.model.Alphabet)
			{
				IsStrict = this.model.IsStrict
			};

			if (this.model.Action == "Зашифрувати")
			{
				this.Encrypt(
					cipher,
					new SimpleSubstitutionKey(
						(this.model.SSCFrom, this.model.SSCTo)));
			} else
			{
				this.Decrypt(
					cipher,
					KeyGenerator.GetSimpleSubstitutionDecryptionKey(
						new SimpleSubstitutionKey(
							(this.model.SSCFrom, this.model.SSCTo))));
			}
		}

		private void ExecuteVisenereCipher()
		{
			var cipher = new VisenereCipher(this.model.Alphabet)
			{
				IsStrict = this.model.IsStrict
			};

			if (this.model.Action == "Зашифрувати")
			{
				this.Encrypt(
					cipher,
					new Key<string>(this.model.VisenereKey));
			} else
			{
				this.Decrypt(
					cipher,
					new Key<string>(this.model.VisenereKey));
			}
		}

		private void ExecuteRSA()
		{
			var system = new RSA(this.model.Alphabet)
			{
				IsStrict = this.model.IsStrict
			};

			if (this.model.Action == "Зашифрувати")
			{
				int p = Int32.Parse(this.model.RSAP);
				int q = Int32.Parse(this.model.RSAQ);

				var keys = KeyGenerator.GenerateRSAKeys(p, q);

				this.nTextBox.Text = this.model.RSAN = keys.EncKey.Value.N.ToString();
				this.eTextBox.Text = this.model.RSAE = keys.EncKey.Value.Value.ToString();
				this.dTextBox.Text = this.model.RSAD = keys.DecKey.Value.Value.ToString();

				this.Encrypt(system, keys.EncKey);
			} else
			{
				int d = Int32.Parse(this.model.RSAD);
				int n = Int32.Parse(this.model.RSAN);
				this.Decrypt(system, new RSAKey((n, d)));
			}
		}
		
		private void Encrypt<T>(ICryptosystem<T> system, Key<T> key)
			where T : IEquatable<T>
		{
			this.model.Ciphertext = system.Encrypt(this.model.Plaintext, key);
			this.ciphertextTextBox.Text = this.model.Ciphertext;
		}

		private void Decrypt<T>(ICryptosystem<T> system, Key<T> key)
			where T : IEquatable<T>
		{
			this.model.Plaintext = system.Decrypt(this.model.Ciphertext, key);
			this.plaintextTextBox.Text = this.model.Plaintext;
		}

		private void Init_Click(object sender, RoutedEventArgs e)
		{
			this.model.ExchangeInProgress = true;
			this.aBorder.IsEnabled = this.bBorder.IsEnabled = true;

			this.model.ExchengerA = new DiffieHellmanExchanger();
			this.model.ExchengerB = new DiffieHellmanExchanger();

			this.model.DHAToSend = this.model.DHBToSend =
				this.model.DHA = this.model.DHB =
				this.aTextBox.Text = this.bTextBox.Text = null;

			this.aKey.Text = this.bKey.Text = String.Empty;
			this.aGenerate.IsEnabled = this.bGenerate.IsEnabled = false;

			int p = Int32.Parse(this.model.DHP);
			int q = Int32.Parse(this.model.DHQ);

			if (!this.model.ExchengerA.TrySetPQ(p, q))
			{
				MessageBox.Show(
					"Неправильні дані.",
					"Помилка",
					MessageBoxButton.OK,
					MessageBoxImage.Error);
				return;
			}

			this.model.ExchengerB.TrySetPQ(p, q);

			this.aReceivedKey.Text = this.bReceivedKey.Text = String.Empty;

			this.model.ExchengerA.ExternalKeySource =
				() => Task.Run(
					() =>
					{
						if (!String.IsNullOrEmpty(this.model.DHBToSend))
						{
							return new Key<int>(Int32.Parse(this.model.DHBToSend));
						}

						Key<int> result = null;

						this.model.ExchengerB.KeyPartGenerated +=
							(sender1, e1) => result = e1.Key;

						while (result == null) { }

						return result;
					});

			this.model.ExchengerB.ExternalKeySource =
				() => Task.Run(
					() =>
					{
						if (!String.IsNullOrEmpty(this.model.DHAToSend))
						{
							return new Key<int>(Int32.Parse(this.model.DHAToSend));
						}

						Key<int> result = null;

						this.model.ExchengerA.KeyPartGenerated +=
							(sender2, e2) => result = e2.Key;

						while (result == null) { }

						return result;
					});
		}

		private async void GenerateA_Click(object sender, RoutedEventArgs e)
		{
			this.exchangerA.Text = "Абонент A: Генерація ключа...";
			await Task.Delay(2000);

			int partToSend = this.model.ExchengerA.GeneratePart();

			this.exchangerA.Text = "Абонент A";
			this.model.DHAToSend = partToSend.ToString();
			this.aTextBox.Text = this.model.ExchengerA.Part.ToString();

			this.bReceivedKey.Text = $"Отримав {partToSend}";
			this.aGenerate.IsEnabled = true;
		}

		private async void GenerateB_Click(object sender, RoutedEventArgs e)
		{
			this.exchangerB.Text = "Абонент B: Генерація ключа...";
			await Task.Delay(2000);

			int partToSend = this.model.ExchengerB.GeneratePart();

			this.exchangerB.Text = "Абонент B";
			this.model.DHBToSend = partToSend.ToString();
			this.bTextBox.Text = this.model.ExchengerB.Part.ToString();

			this.aReceivedKey.Text = $"Отримав {partToSend}";
			this.bGenerate.IsEnabled = true;
		}

		private async void GenerateAFull_Click(object sender, RoutedEventArgs e)
		{
			this.exchangerA.Text = "Абонент A: Очікування ключа...";
			
			var key = await this.model.ExchengerA.GenerateKey();

			this.aKey.Text = key.Value.ToString();

			this.exchangerA.Text = "Абонент A";
		}

		private async void GenerateBFull_Click(object sender, RoutedEventArgs e)
		{
			this.exchangerB.Text = "Абонент B: Очікування ключа...";

			var key = await this.model.ExchengerB.GenerateKey();

			this.bKey.Text = key.Value.ToString();

			this.exchangerB.Text = "Абонент B";
		}
	}
}
