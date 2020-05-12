﻿using System.Windows.Forms;

namespace Sdl.Community.ExportAnalysisReports.Interfaces
{
	public interface IMessageBoxService
	{
		void ShowInformationMessage(string text, string header);

		void ShowOwnerInformationMessage(IWin32Window owner, string text, string header);

		void ShowWarningMessage(string text, string header);
	}
}