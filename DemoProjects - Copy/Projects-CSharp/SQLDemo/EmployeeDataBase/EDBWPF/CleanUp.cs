using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace EDBWPF
{
	public static class CleanUp
	{
		public static void ClearTextBoxes(List<TextBox> textBoxes) 
		{
			foreach (TextBox textBox in textBoxes) 
			{
				textBox.Clear();
			}
		}
	}
}
