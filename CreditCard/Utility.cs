/////////////////////////////////////////////////////////////////////////////////////
//
// Company Name		: Don Bailes
// Department Name	: Computer and Information Sciences 
// File Name			: Utility.cs
// Purpose			: Implement several methods that may be useful in 
//							multiple projects 
// Author				: Don Bailes, E-Mail: bailes@etsu.edu
// Create Date			: Monday, September 27, 2010
//
//-----------------------------------------------------------------------------------
//
// Modified Date		: Monday, September 27, 2010
// Modified By			: Don Bailes
//
/////////////////////////////////////////////////////////////////////////////////////


using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Utils
{
	/// <summary>
	/// The Utility class contains several methods that may be useful in multiple projects
	/// </summary>
	public static class Utility
	{
		#region PressAnyKey
		/// <summary>
		/// Display a Press Any Key to ... message at the bottom of the screen
		/// </summary>
		/// <param name="strVerb">the term to include in the Press Any Key to ... message; defaults to "continue . . ."</param>
		public static void PressAnyKey (string strVerb = "continue ...")
		{
			Console.ForegroundColor = ConsoleColor.Red;

			if (Console.CursorTop < Console.WindowHeight - 1)
				Console.SetCursorPosition (0, Console.WindowHeight - 1);
			else
				Console.SetCursorPosition (0, Console.CursorTop + 2);

			Console.Write ("Press any key to " + strVerb);
			Console.ReadKey ( );
			Console.Clear ( );
			Console.ForegroundColor = ConsoleColor.Blue;
		} // End PressAnyKey 
		#endregion

		#region Skip n lines
		/// <summary>
		/// Skip n lines in the console window
		/// </summary>
		/// <param name="n">the number of lines to skip - defaults to 1</param>
		public static void Skip (int n = 1)
		{
			for (int i = 0; i < n; i++)
			{
				Console.WriteLine ( );
			}
		} 
		#endregion

		#region FormatText
		/// <summary>
		/// Return a string formatted for display between the designated left and right margins
		/// </summary>
		/// <param name="txt">the string to be formatted</param>
		/// <param name="leftMargin">the left margin</param>
		/// <param name="rightMargin">the right margin</param>
		/// <returns>the formatted string</returns>
		public static string FormatText (string txt, int leftMargin = 0, int rightMargin = 80)
		{
			if (String.IsNullOrEmpty (txt))
				return String.Empty;

			List<string> tokens = Tokenize (txt, ".,?!*()-+=@%&{}|\n\r :;',<>~[]");

			// Throw exception if the longest word will not fit on a line
			int max        = MaxLength (tokens);
			if (leftMargin + max > rightMargin || leftMargin < 0)
				throw new ArgumentException (String.Format ("The left margin ({0}) must be " +
					"greater than or equal to 0 and the right margin ({1}) " +
					"must allow the longest word ({2} characters) to fit on the line",
					leftMargin, rightMargin, max));

			string strOut  = "";
			string left    = "";
			int n, length;

			// Create a string of blanks for the left margin
			for (n = 0; n < leftMargin; n++)
				left += " ";

			strOut = left;
			length = leftMargin;

			// Step through the tokens one at a time
			for (n = 0; n < tokens.Count; n++)
			{
				// if end of paragraph, ignore
				if (n < tokens.Count - 1 && tokens[n] == "\n" && tokens[n + 1] != "\n")
					continue;

				// if the next word takes us beyond the right margin, start new line
				if (1 + length + tokens[n].Length > rightMargin)
				{
					length = leftMargin;
					strOut += "\n" + left;
				}

				// if we are not in first column and token is not punctuation, add a space
				if ((length != leftMargin) && (tokens[n].IndexOfAny (".,?!;:()%@".ToCharArray ( )) == -1))
				{
					strOut += " ";
				}

				// add the token and the length of the token plus the space
				strOut += tokens[n];
				length += tokens[n].Length + 1;

				// if the token is a new line character, start a new line
				if (tokens[n] == "\n")
				{
					strOut += "\n" + left;
					length = leftMargin;
				}
			}

			return strOut;
		} 
		#endregion

		#region MaxLength
		/// <summary>
		/// Return the length of the longest string in a list of strings
		/// </summary>
		/// <param name="txt">the list of strings</param>
		/// <returns>the length of the longest string in the list</returns>
		private static int MaxLength (List<string> txt)
		{
			int max = txt[0].Length;
			for (int n = 1; n < txt.Count; n++)
				if (max < txt[n].Length)
					max = txt[n].Length;
			return max;
		} 
		#endregion

		#region Tokenize
		/// <summary>
		/// Create a list of tokens from an input string using specified delimiters
		/// </summary>
		/// <param name="original">the string to be tokenized</param>
		/// <param name="delimiters">the delimiters to be used in tokenizing the input string</param>
		/// <returns></returns>
		public static List<String> Tokenize (string original, string delimiters)
		{
			List<string> Tokens = new List<string> ( );
			string work   = original;
			work = Clean (work);

			int col;
			string token;

			while (!String.IsNullOrEmpty (work))
			{
				col = work.IndexOfAny (delimiters.ToCharArray ( ));
				if (col == 0)
					col = 1;
				token = work.Substring (0, col);
				Tokens.Add (token);
				work = work.Substring (col);
				work = work.Trim (" \t".ToCharArray ( ));
			}
			return Tokens;
		} 
		#endregion

		#region Clean
		/// <summary>
		/// Convert "\r\n" to "\n" in the parameter and return the result
		/// </summary>
		/// <param name="work">the string to be cleaned</param>
		/// <returns>the cleaned string</returns>
		private static string Clean (string work)
		{
			work = work.Trim (" \t".ToCharArray ( ));
			int col   = work.IndexOf ("\r\n");
			while (col != -1)
			{
				work = work.Remove (col, 1);
				col = work.IndexOf ("\r\n");
			}
			return work;
		} 
		#endregion

		#region WelcomeMessage
		/// <summary>
		/// Display a specified welcome message in a Message Box
		/// </summary>
		/// <param name="msg">The message to be displayed</param>
		/// <param name="caption">the caption for the Message Box - the author's name is appended</param>
		/// <param name="author">the name of the author of the program</param>
		public static void WelcomeMessage (String msg, String caption = "Computer Science 2210", String author = "Don Bailes")
		{
			MessageBox.Show (null, DateTime.Today.ToLongDateString ( ) + "\n\n" + msg, caption + " - " + author,
				MessageBoxButtons.OK, MessageBoxIcon.Information);
		} 
		#endregion

		#region GoodbyeMessage
		/// <summary>
		/// Display a goodbye message
		/// </summary>
		/// <param name="msg">the message to be displayed</param>
		public static void GoodbyeMessage (String msg = "Goodbye and thank you for using this program.")
		{
			MessageBox.Show (null, msg, "Goodbye and Thank You",
				MessageBoxButtons.OK, MessageBoxIcon.Information);
		} 
		#endregion
	}
}
