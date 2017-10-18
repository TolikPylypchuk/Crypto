using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Crypto.Infrastructure;
using Microsoft.Win32;

using Crypto.Systems;

namespace Crypto.Signature
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml.
	/// </summary>
	public partial class MainWindow : Window
	{
		public static readonly DependencyProperty ModelProperty;

		#region Constructors

		static MainWindow()
		{
			ModelProperty = DependencyProperty.Register(
				nameof(Model),
				typeof(MainViewModel),
				typeof(MainWindow));
		}

		public MainWindow()
		{
			this.InitializeComponent();
			this.DataContext = this.Model = new MainViewModel
			{
				Alphabet = Alphabets.ASCII
			};

			var args = Environment.GetCommandLineArgs();

			if (args.Length == 2)
			{
				this.Model.Path = args[1];
			}
		}

		#endregion

		#region Properties

		public MainViewModel Model
		{
			get => (MainViewModel)this.GetValue(ModelProperty);
			set => this.SetValue(ModelProperty, value);
		}

		#endregion

		#region Command bindings

		private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			var dialog = new OpenFileDialog
			{
				Title = "Open the File",
				InitialDirectory = Environment.CurrentDirectory
			};

			if (dialog.ShowDialog() == true)
			{
				this.Model.Path = dialog.FileName;
			}
		}

		private async void Encrypt_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			var dialog = new SaveFileDialog
			{
				Title = "Save Encrypted File As",
				InitialDirectory = Environment.CurrentDirectory,
				Filter = "All (*.*)|*.*"
			};

			if (dialog.ShowDialog() == true)
			{
				var rsa = new RSA(this.Model.Alphabet);
				var key = new Key<(int N, int Value)>(
					(this.Model.N, this.Model.E));

				using (var reader = File.OpenText(this.Model.Path))
				using (var writer = File.CreateText(dialog.FileName))
				{
					await writer.WriteAsync(
						rsa.Encrypt(await reader.ReadToEndAsync(), key));
				}
			}
		}
		
		private async void Decrypt_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			var dialog = new SaveFileDialog
			{
				Title = "Save Decrypted File As",
				InitialDirectory = Environment.CurrentDirectory,
				Filter = "All (*.*)|*.*"
			};

			if (dialog.ShowDialog() == true)
			{
				var rsa = new RSA(this.Model.Alphabet);
				var key = new Key<(int N, int Value)>(
					(this.Model.N, this.Model.D));

				using (var reader = File.OpenText(this.Model.Path))
				using (var writer = File.CreateText(dialog.FileName))
				{
					string textToDecrypt = await reader.ReadToEndAsync();
					string text = rsa.Decrypt(textToDecrypt, key);
					await writer.WriteAsync(text);
				}
			}
		}
		
		private async void Sign_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			var dialog = new SaveFileDialog
			{
				Title = "Save Signature File As",
				InitialDirectory = Environment.CurrentDirectory,
				Filter = "All (*.*)|*.*"
			};

			if (dialog.ShowDialog() == true)
			{
				int hash = await this.GetHash(this.Model.Path);
				
				using (var stream = File.Create(dialog.FileName))
				{
					var bytes = BitConverter.GetBytes(
						Algorithms.Modulo(hash, this.Model.E, this.Model.N));

					await stream.WriteAsync(bytes, 0, 4);
				}
			}
		}

		private async void Check_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			var dialog = new OpenFileDialog
			{
				Title = "Open the Signature File",
				InitialDirectory = Environment.CurrentDirectory
			};

			if (dialog.ShowDialog() == true)
			{
				int hash = await this.GetHash(this.Model.Path);

				using (var stream = File.OpenRead(dialog.FileName))
				{
					var bytes = new byte[4];
					await stream.ReadAsync(bytes, 0, 4);

					int value = BitConverter.ToInt32(bytes, 0);

					if (Algorithms.Modulo(value, this.Model.D, this.Model.N) == hash)
					{
						MessageBox.Show(
							"The signature is valid",
							"OK",
							MessageBoxButton.OK,
							MessageBoxImage.Information);
					} else
					{
						MessageBox.Show(
							"The signature is not valid",
							"Error",
							MessageBoxButton.OK,
							MessageBoxImage.Error);
					}
				}
			}
		}

		private void RSACommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
			=> e.CanExecute = !String.IsNullOrEmpty(this.Model?.Path) &&
				this.IsValid();

		private void Exit_Executed(object sender, ExecutedRoutedEventArgs e)
			=> Application.Current.Shutdown();

		#endregion

		#region Other methods

		private bool IsValid() => IsValid(this);

		private static bool IsValid(DependencyObject obj)
		{
			return !Validation.GetHasError(obj) &&
				   LogicalTreeHelper.GetChildren(obj)
									.OfType<DependencyObject>()
									.All(IsValid);
		}

		private async Task<int> GetHash(string file)
		{
			int result = 0;

			using (var reader = File.OpenText(file))
			{
				const int blockLength = 1024;

				var buffer = new char[blockLength];

				while (!reader.EndOfStream)
				{
					int count = await reader.ReadBlockAsync(buffer, 0, blockLength);

					for (int i = 0; i < count; i++)
					{
						result += this.Model.Alphabet.IndexOf(buffer[i]);
					}
				}
			}

			return result % this.Model.Alphabet.Length;
		}

		#endregion
	}
}
