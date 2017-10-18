using System.Collections.Generic;
using System.Windows;

namespace Crypto.Signature
{
	public class MainViewModel : DependencyObject
	{
		#region Dependency properties

		public static readonly DependencyProperty PathProperty;
		public static readonly DependencyProperty NProperty;
		public static readonly DependencyProperty EProperty;
		public static readonly DependencyProperty DProperty;
		public static readonly DependencyProperty AlphabetProperty;
		public static readonly DependencyProperty AlphabetsListProperty;

		#endregion

		#region Constructors

		static MainViewModel()
		{
			PathProperty = DependencyProperty.Register(
				nameof(Path),
				typeof(string),
				typeof(MainViewModel));
			
			NProperty = DependencyProperty.Register(
				nameof(N),
				typeof(int),
				typeof(MainViewModel));

			EProperty = DependencyProperty.Register(
				nameof(E),
				typeof(int),
				typeof(MainViewModel));

			DProperty = DependencyProperty.Register(
				nameof(D),
				typeof(int),
				typeof(MainViewModel));

			AlphabetProperty = DependencyProperty.Register(
				nameof(Alphabet),
				typeof(Alphabet),
				typeof(MainViewModel));

			AlphabetsListProperty = DependencyProperty.Register(
				nameof(AlphabetsList),
				typeof(List<Alphabet>),
				typeof(MainViewModel));
		}

		public MainViewModel()
		{
			this.AlphabetsList = new List<Alphabet>
			{
				Alphabets.ASCII,
				Alphabets.English,
				Alphabets.EnglishLower,
				Alphabets.EnglishUpper,
				Alphabets.EnglishWithPunctuation,
				Alphabets.Ukrainian,
				Alphabets.UkrainianLower,
				Alphabets.UkrainianUpper,
				Alphabets.UkrainianWithPunctuation
			};
		}

		#endregion

		#region Properties

		public string Path
		{
			get => (string)this.GetValue(PathProperty);
			set => this.SetValue(PathProperty, value);
		}
		
		public int N
		{
			get => (int?)this.GetValue(NProperty) ?? 0;
			set => this.SetValue(NProperty, value);
		}
		
		public int E
		{
			get => (int?)this.GetValue(EProperty) ?? 0;
			set => this.SetValue(EProperty, value);
		}

		public int D
		{
			get => (int?)this.GetValue(DProperty) ?? 0;
			set => this.SetValue(DProperty, value);
		}

		public Alphabet Alphabet
		{
			get => (Alphabet)this.GetValue(AlphabetProperty);
			set => this.SetValue(AlphabetProperty, value);
		}

		public List<Alphabet> AlphabetsList
		{
			get => (List<Alphabet>)this.GetValue(AlphabetsListProperty);
			set => this.SetValue(AlphabetsListProperty, value);
		}

		#endregion
	}
}
