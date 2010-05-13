using System;
using NUnit.Framework;

namespace TwinEditor.Test
{
	/// <summary>
	/// Description of UITester.
	/// </summary>
	
	[TestFixture]
	public class UITester
	{
		
		[STAThread]
		[Test]
		public void test1()
		{
			MainForm mainForm = new MainForm(new Controller());
			mainForm.Show();
			Assert.IsNotNull(mainForm);
		}

	}
}
