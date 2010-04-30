/*
 * Created by SharpDevelop.
 * User: laiw
 * Date: 30.04.2010
 * Time: 10:40
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;


namespace Lebowski.Controller
{
	/// <summary>
	/// Description of Controller.
	/// </summary>
	public class Controller
	{
		private int fileNumber;
		public int FileNumber {
			get {
				fileNumber++;
				return fileNumber;
			}
		}
		
		public Controller()
		{
			this.fileNumber = 0;

			
		}
		
		
	}
}
