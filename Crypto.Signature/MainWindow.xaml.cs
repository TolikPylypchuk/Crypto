using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
			this.DataContext = this.Model = new MainViewModel();
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
				InitialDirectory = Environment.CurrentDirectory
			};

			if (dialog.ShowDialog() == true)
			{
				this.Model.PlaintextPath = dialog.FileName;
			}
		}

		private void Exit_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		private async void Encrypt_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			var dialog = new SaveFileDialog
			{
				InitialDirectory = Environment.CurrentDirectory
			};

			if (dialog.ShowDialog() == true)
			{
				var rsa = new RSA(this.Model.Alphabet);
				var key = new Key<(int N, int Value)>(
					(this.Model.N, this.Model.E));

				using (var reader = File.OpenText(this.Model.PlaintextPath))
				using (var writer = File.CreateText(dialog.FileName))
				{
					await writer.WriteLineAsync(
						rsa.Encrypt(await reader.ReadLineAsync(), key));
				}
			}
		}
		
		private void Decrypt_Executed(object sender, ExecutedRoutedEventArgs e)
		{

		}
		
		private void Sign_Executed(object sender, ExecutedRoutedEventArgs e)
		{

		}

		private void RSACommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
			=> e.CanExecute = !String.IsNullOrEmpty(this.Model?.PlaintextPath) &&
				this.IsValid();

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

		#endregion
	}
}
