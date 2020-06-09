﻿using System.Collections.Generic;
using Moq;
using Sdl.Community.NumberVerifier.Interfaces;
using Sdl.Community.NumberVerifier.Model;
using Sdl.Community.NumberVerifier.Tests.Utilities;
using Xunit;

namespace Sdl.Community.NumberVerifier.Tests.OmitZero
{
	public class OmitLeadingZero
	{

		[Theory]
		[InlineData(".55")]
		public string OmitZeroChecked(string number)
		{
			var numberVerifierSettings = OmitZeroSettings.OmitZeroCheckedAndPreventLocalization();
			var methodsMock = new Mock<INumberVerifierMethods>();

			NumberVerifierLocalizationsSettings.InitSeparators(numberVerifierSettings);
			var numberVerifierMain = new NumberVerifierMain(numberVerifierSettings.Object);
			numberVerifierMain.NormalizeAlphanumerics(number, new List<string>(), new List<string>(), ".", ".", false,
				numberVerifierSettings.Object.SourceOmitLeadingZero);
			var normalizedNumber = numberVerifierMain.NormalizedNumber(new SeparatorModel
			{
				MatchValue = number,
				ThousandSeparators = ".",
				DecimalSeparators = ".",
				NoSeparator = false,
				CustomSeparators = string.Empty
			});

			methodsMock.Setup(x => x.OmitZero(number));
			return normalizedNumber;
		}

		[Theory]
		[InlineData(".55")]
		public void CheckNumberIfOmitZeroIsChecked(string number)
		{
			var normalizedNumber = OmitZeroChecked(number);

			Assert.Equal("0.55", normalizedNumber);
		}

		[Theory]
		[InlineData(".55")]
		public string OmitZeroShortForm(string text)
		{
			var imockSettings = Utilities.NumberVerifierLocalizationsSettings.AllowLocalization();

			NumberVerifierLocalizationsSettings.InitSeparators(imockSettings);
			var numberVerifierMain = new NumberVerifierMain(imockSettings.Object);

			var normalizedNumber = numberVerifierMain.OmitZero(text);

			Assert.Equal("0.55", normalizedNumber);

			return normalizedNumber;
		}

		[Theory]
		[InlineData("0.55")]
		public void OmitZeroLongForm(string text)
		{
			var normalizedNumber = OmitZeroShortForm(text);

			Assert.Equal("0.55", normalizedNumber);
		}

		[Theory]
		[InlineData("-.55")]
		public string OmitZeroShortFormNegativeNumbers(string text)
		{
			var mockSettings = NumberVerifierLocalizationsSettings.AllowLocalization();

			NumberVerifierLocalizationsSettings.InitSeparators(mockSettings);
			var numberVerifierMain = new NumberVerifierMain(mockSettings.Object);

			var normalizedNumber = numberVerifierMain.OmitZero(text);

			Assert.Equal("m0.55", normalizedNumber);

			return normalizedNumber;
		}

		[Theory]
		[InlineData("−.55")]
		public void OmitZeroShortFormNegativeNumbersSpecialMinusSign(string text)
		{
			var normalizedNumber = OmitZeroShortFormNegativeNumbers(text);
			Assert.Equal("m0.55", normalizedNumber);
		}

		[Theory]
		[InlineData("-0.55", "-0,55")]
		public void OmitZeroLongFormNegativeNumbersMinusSign(string numberWithPeriod, string numberWithComma)
		{
			var mockSettings = NumberVerifierLocalizationsSettings.AllowLocalization();

			NumberVerifierLocalizationsSettings.InitSeparators(mockSettings);
			var numberVerifierMain = new NumberVerifierMain(mockSettings.Object);

			var normalizedNumberWithPeriod = numberVerifierMain.OmitZero(numberWithPeriod);
			var normalizedNumberWithComma = numberVerifierMain.OmitZero(numberWithComma);

			Assert.Equal("m0.55", normalizedNumberWithPeriod);
			Assert.Equal("m0,55", normalizedNumberWithComma);
		}

		[Theory]
		[InlineData("−0.55", "−0,55")]
		public void OmitZeroLongFormNegativeNumbersSpecialMinusSign(string numberWithPeriod, string numberWithComma)
		{
			OmitZeroLongFormNegativeNumbersMinusSign(numberWithPeriod, numberWithComma);
		}

		#region Omit leading zero option is unchecked

		/// <summary>
		/// If the option is unchecked the method shouldn't be called
		/// </summary>
		/// <param name="number"></param>
		[Theory]
		[InlineData(".55")]
		public string OmitZeroUnchecked(string number)
		{
			var numberVerifierSettings = OmitZeroSettings.OmitZeroUncheckedAndAllowLocalization();
			var methodsMock = new Mock<INumberVerifierMethods>(MockBehavior.Strict);

			NumberVerifierLocalizationsSettings.InitSeparators(numberVerifierSettings);
			var numberVerifierMain = new NumberVerifierMain(numberVerifierSettings.Object);
			numberVerifierMain.NormalizeAlphanumerics(number, new List<string>(), new List<string>(), ".", ".", false,
				numberVerifierSettings.Object.SourceOmitLeadingZero);
			var normalizedNumber = numberVerifierMain.NormalizedNumber(new SeparatorModel
			{
				MatchValue = number,
				ThousandSeparators = ".",
				DecimalSeparators = ".",
				NoSeparator = false,
				CustomSeparators = string.Empty
			});

			methodsMock.Verify(m => m.OmitZero(number), Times.Never);
			return normalizedNumber;
		}

		[Theory]
		[InlineData(".55")]
		public void CheckNumberIfOmitZeroIsUnchecked(string number)
		{
			var normalizedNumber = OmitZeroUnchecked(number);

			Assert.True(normalizedNumber != "0.55");
		}

		#endregion
	}
}