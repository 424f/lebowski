using System;
using System.Windows.Forms;

namespace Lebowski
{
	internal sealed class Program
	{
		[STAThread]
		private static void Main(string[] args)
		{
			Console.WriteLine("Lebowski");
			
			var test = new Lebowski.Test.Synchronization.DifferentialSynchronizationTest();
			test.TestThis();
			
			Console.ReadKey();
			
			/*Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new LebowskiMockups.MainForm());*/
		}
	}
}
