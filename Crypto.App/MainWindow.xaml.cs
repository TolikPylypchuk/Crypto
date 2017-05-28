using System;
using System.Windows;
using Crypto.Keys;
using Crypto.Systems;

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
			switch (this.model.CurrentTab.Header)
			{
				case "Шифр Цезаря":
					if (Int32.TryParse(this.model.Shift, out int shift))
					{
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
					} else
					{
						MessageBox.Show(
							"Зміщення має бути додатним числом.",
							"Помилка",
							MessageBoxButton.OK,
							MessageBoxImage.Error);
					}
					break;
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
	}
}
