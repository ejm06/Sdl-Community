﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Sdl.Community.TmAnonymizer.Helpers;
using Sdl.Community.TmAnonymizer.Model;
using Sdl.Community.TmAnonymizer.ViewModel;

namespace Sdl.Community.TmAnonymizer.Ui
{
	/// <summary>
	/// Interaction logic for PreviewWindow.xaml
	/// </summary>
	public partial class PreviewWindow
	{
		private RichTextBox _textBox;
		public PreviewWindow()
		{
			InitializeComponent();

		}

		private void FrameworkElement_OnContextMenuOpening(object sender, ContextMenuEventArgs e)
		{
			var rtb = sender as RichTextBox;
			_textBox = rtb;
		}

		private void MenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			var docStart = _textBox.Document.ContentStart;
			var start = _textBox.Selection.Start;
			var end = _textBox.Selection.End;

			var tr = new TextRange(start, end);
			tr.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.LightSalmon);

			var dataContext = _textBox.DataContext as SourceSearchResult;
			//var position = CustomTextBox.GetPoint(docStart, docStart.GetOffsetToPosition(start));
			var startRange = new TextRange(docStart, start);
			var indexStartAbs = startRange.Text.Length;
			var text = new TextRange(docStart, _textBox.Document.ContentEnd).Text.TrimEnd();
			var wordDetails = new WordDetails
			{
				Position = indexStartAbs,//docStart.GetOffsetToPosition(start),
				Length = indexStartAbs+_textBox.Selection.Text.TrimEnd().Length, //docStart.GetOffsetToPosition(end),
				Text = _textBox.Selection.Text.TrimEnd()
				
			};
			var nextWord = GetNextWord(wordDetails, text);
			wordDetails.NextWord = nextWord;
			dataContext?.SelectedWordsDetails.Add(wordDetails);
		}
			
		private string GetNextWord(WordDetails wordDetails,string text)
		{
			var splitedWord = text.Substring(wordDetails.Length+1);
			var nextWord = splitedWord.Substring(0, splitedWord.IndexOf(" ", StringComparison.Ordinal));
			return nextWord;
		}
		private void UnselectWord(object sender, RoutedEventArgs e)
		{
			var docStart = _textBox.Document.ContentStart;
			var start = _textBox.Selection.Start;
			var end = _textBox.Selection.End;

			var tr = new TextRange(start, end);
			tr.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.White);
			var dataContext = _textBox.DataContext as SourceSearchResult;
			var wordDetails = new WordDetails
			{
				Position = docStart.GetOffsetToPosition(start),
				Length = docStart.GetOffsetToPosition(end),
				Text = _textBox.Selection.Text.TrimEnd()
			};
			dataContext?.UnselectedWordsDetails.Add(wordDetails);
		}
	}
}
