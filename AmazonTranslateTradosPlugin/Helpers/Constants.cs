﻿using System;
using System.IO;

namespace Sdl.Community.AmazonTranslateTradosPlugin.Helpers
{
	public static class Constants
	{
		public readonly static string JsonFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"SDL Community\AmazonTranslationProvider");
		public readonly static string JsonFileName = "AmazonProviderSettings.json";
	}
}