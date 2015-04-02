/////////////////////////////////////////////////////////////////////////////////////
//
// Company Name	: Don Bailes
// Department Name	: Computer and Information Sciences 
// File Name		: Menu.cs
// Purpose			: Define a Menu class that will allow a menu to be 
//							displayed in a Console window.  The user's choice  
//							is obtained and verified. 
// Author			: Don Bailes, E-Mail: bailes@etsu.edu
// Create Date		: Tuesday, September 27, 2011
//
//-----------------------------------------------------------------------------------
//
// Modified Date	: Tuesday, September 27, 2011
// Modified By		: Don Bailes
//
/////////////////////////////////////////////////////////////////////////////////////


using System;
using System.Collections.Generic;

namespace Utils
{
	public class Menu
	{
		private	List<string> Items = new List<string> ( );
		public string Title { get; set; }

		#region Constructor
		/// <summary>
		/// Constructor 
		/// </summary>
		/// <param name="title">the title to be displayed above menu</param>
		public Menu (string title = "Menu")
		{
			Title = title;
		}
		#endregion

		#region Plus and Minus Operators
		/// <summary>
		/// Operator + adds a choice to the menu
		/// </summary>
		/// <param name="m">the menu to which the choice is added</param>
		/// <param name="item">the choice to be added</param>
		/// <returns></returns>
		public static Menu operator + (Menu m, string item)
		{
			m.Items.Add (item);
			return m;
		}

		/// <summary>
		/// Operator  - removes a choice from the menu
		/// </summary>
		/// <param name="m">the menu from which the choice is removed</param>
		/// <param name="item">the number of the choice to be removed</param>
		/// <returns></returns>
		public static Menu operator - (Menu m, int n)
		{
			if (n > 0 && n <= m.Items.Count)
				m.Items.RemoveAt (n - 1);
			return m;
		}
		#endregion

		#region Display and GetChoice methods
		/// <summary>
		/// Display the menu on the console window
		/// </summary>
		public void Display ( )
		{
			string str = "";
			Console.Clear ( );
			str = DateTime.Today.ToLongDateString ( );
			Console.SetCursorPosition (Console.WindowWidth - str.Length, 0);
			Console.WriteLine (str);
			Console.ForegroundColor = ConsoleColor.Red;

			Console.WriteLine ("\n\n\t   " + Title);
			Console.Write ("\t   ");
			for (int n = 0; n < Title.Length; n++)
				Console.Write ("-");
			Console.WriteLine ("\n");
			Console.ForegroundColor = ConsoleColor.Blue;
			for (int n = 0; n < Items.Count; n++)
				Console.WriteLine ("\t{0}. {1}", (n + 1), Items[n]);
		}

		/// <summary>
		/// Obtain the user's selection, verify it is valid, and return it
		/// </summary>
		/// <returns>the number of the user's valid selection</returns>
		public int GetChoice ( )
		{
			int choice = -1;
			string line;
			if (Items.Count < 1)
				throw new Exception ("The menu is empty");

			while (true)
			{
				Display ( );
				Console.Write ("\n\t   Type the number of your choice from the menu: ");
				Console.ForegroundColor = ConsoleColor.Red;
				line = Console.ReadLine ( );
				Console.ForegroundColor = ConsoleColor.Blue;
				if (!Int32.TryParse (line, out choice))
				{
					Console.WriteLine ("\n\t   Your choice is not a number between 1 and {0}.  Please try again.",
						Items.Count);
					Utility.PressAnyKey ( );
				}
				else
				{
					if (choice < 1 || choice > Items.Count)
					{
						Console.WriteLine ("\n\t   Your choice is not a number between 1 and {0}.  Please try again.",
						Items.Count);
						Utility.PressAnyKey ( );
					}
					else
					{
						Console.Clear ( );
						return choice;
					}
				}
			}
		}
		#endregion

		#region ReplaceChoice method
		/// <summary>
		/// Replace the designated menu item with the specified replacement
		/// </summary>
		/// <param name="item">the number of the item to be replaced</param>
		/// <param name="newChoice">the replacement menu choice</param>
		public void ReplaceChoice (int item, String newChoice)
		{
			if (item < 1 || item > Items.Count)
				throw new IndexOutOfRangeException (String.Format ("{0} is not between 1 and {1}." +
					"  There is no such choice to replace.",
					item, Items.Count));

			Items[item - 1] = newChoice;
		}
		#endregion
	}
}
