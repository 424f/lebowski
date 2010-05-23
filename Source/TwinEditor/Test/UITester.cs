using System;
using NUnit.Framework;
using TwinEditor.UI;

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
			MainForm mainForm = new MainForm(new ApplicationContext(), new Controller());
			mainForm.Show();
			Assert.IsNotNull(mainForm);
		}

	}
}
