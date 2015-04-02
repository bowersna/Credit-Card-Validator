/*
 * Class : CSCI 2210-201  --Data Structures--
 * File Name : Driver.cs
 * 
 * Purpose	: Create a driver class that interacts with the CreditCard class to create
 *            a credit card object from information the user inputs and displays if the
 *            user's credit card is valid along with some other information to the screen.
 *
 * Author:Nick Bowers
 * E-Mail: BOWERSNA@goldmail.etsu.edu
 * 
 * Create Date    : Wednesday, September 11, 2013
 * Last Modified  : Tuesday, September 24, 2013
 * Modified By	: Nick Bowers
 */
using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Forms;


namespace CreditCardValidator
{
    #region Enumerated List Types
    //enumerated types for the Menu choice
    enum MenuChoice
    {
        CREATE_NEW_LIST = 1,
        TXT_FILE_NEW,
        ADD_CARD,
        REMOVE_CARD,
        CARD_POS,
        CARD_NUM,
        CARD_PERSON,
        VALID_LIST,
        SORT_BY_NUM,
        DISPLAY_LIST,
        QUIT
    }
    #endregion
    #region Driver
    class Driver
    {
        /// <summary>
        /// Main method --display Menu choices to user and process the information to display a brief summary--
        /// </summary>
        /// <param name="args"></param>
        [STAThread]
        static void Main(string[] args)
        {
            Console.Title = "Credit Card Information";
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Clear();

            //string variables used to gather information about user
            String Name,
                   NumCard,
                   ExpMonth,
                   ExpYear,
                   Tele,
                   Position,
                   EMail;
            String FileName;
            //integer variable used for position in List
            int IntPos = 0;

            CreditCard Card = null;     //create an Object of the CreditCard class for use
            CreditCardList List = null; //create an Object of CreditCardList class

            Regex PhonePat = new Regex(@"^(\(?[0-9]{3}\)?)\-?[0-9]{3}\-?[0-9]{4}$"); //10-digit US phone numbers with or without 
            Regex EmailPat = new Regex(@"(?<name>\S+)@(?<domain>\S+)");             //pattern for a valid email format      
            Regex MonthPat = new Regex(@"^[0-9]{2}$");                              //2 digits 0 - 9 in a string
            Regex YearPat = new Regex(@"^[0-9]{4}$");                               //4 digits 0 - 9 in a string
            Regex CardNumLengthPat = new Regex(@"^\d{12,19}");                      //patttern matching credit card num length

            Utils.Menu menu = new Utils.Menu("Credit Card Validtor:\n\n\t   ---By: Nick Bowers---\n");
            menu = menu + "Create New Card List" + "Import A Card List From (.txt) File" +
                          "Add A Card To The List" + "Remove A Card From The List" +
                          "Display Card From Position In The List" + "Display A Card From Card Number" +
                          "Display a Card From Card Owner" + "Display List Of All Valid Cards" + "Sort By Card Number" +
                          "Display List Of Cards" + "Exit"; //set up user choices displayed on the Menu

            //accept user choice from Menu
            MenuChoice choice = (MenuChoice)menu.GetChoice();

            //while loop stopping when the user selects the Exit selection
            while (choice != MenuChoice.QUIT)
            {
                //switch statement to determine which choice was selected
                switch (choice)
                {
                    case MenuChoice.ADD_CARD:
                        if (List == null)
                        {
                            Console.WriteLine("A list may have not been created yet.");

                        }
                        else
                        {
                            //retrieve full name from user
                            Console.WriteLine("Enter first and last name of the cardholder: ");
                            Name = Console.ReadLine();

                            //retrieve phone number
                            Console.WriteLine("\nEnter cardholder 10 digit phone number: ");
                            Tele = Console.ReadLine();

                            //while format is incorrect, keep asking for correct
                            Match T = PhonePat.Match(Tele);
                            while (T.Success == false)
                            {
                                Console.WriteLine("\nFormat is incorrect, please enter a 10 digit phone number: ");
                                Tele = Console.ReadLine();
                                T = PhonePat.Match(Tele);
                            }

                            //retrieve Address
                            Console.WriteLine("\nEnter E-mail address: ");
                            EMail = Console.ReadLine();

                            //validate that email is correct format
                            Match E = EmailPat.Match(EMail);
                            while (E.Success == false)
                            {
                                Console.WriteLine("\nE-mail format is incorrect.Please try again: ");
                                EMail = Console.ReadLine();
                                E = EmailPat.Match(EMail);
                            }
                            //retrieve Card Number
                            Console.WriteLine("\nEnter card number as it apprears on the card without any spaces: ");
                            NumCard = Console.ReadLine();

                            //make sure card number length is between 12-19
                            Match Num = CardNumLengthPat.Match(NumCard);
                            while (Num.Success == false)
                            {
                                Console.WriteLine("\nThat is not a valid length of credit card number, please try again: ");
                                NumCard = Console.ReadLine();
                                Num = CardNumLengthPat.Match(NumCard);
                            }

                            //retrive Expiration date (Month)
                            Console.WriteLine("\nEnter the Expiration Month (2 digits): ");
                            ExpMonth = Console.ReadLine();

                            //check for properly entered digits
                            Match M = MonthPat.Match(ExpMonth);
                            //while the format is not correct, continue to ask for a correct format of digits
                            while (M.Success == false)
                            {
                                Console.WriteLine("\nThat is not a valid format, please enter 2 digits (such as 02 for February): ");
                                ExpMonth = Console.ReadLine();
                                M = MonthPat.Match(ExpMonth);
                            }

                            //retrive Expiration date (Year)
                            Console.WriteLine("\nEnter the Expiration Year (4 digits): ");
                            ExpYear = Console.ReadLine();

                            //check for properly entered digits
                            Match Y = YearPat.Match(ExpYear);
                            //while the format is not correct, continue to ask for a re-entry
                            while (Y.Success == false)
                            {
                                Console.WriteLine("\nThat is not a valid format, please enter 4 digits (such as 1999): ");
                                ExpYear = Console.ReadLine();
                                Y = YearPat.Match(ExpYear);
                            }

                            //create a CreditCard object with the variables input from the User
                            Card = new CreditCard(Name, NumCard, ExpMonth, ExpYear, Tele, EMail);

                            //add the card to the list of credit cards
                            List.Add(Card);

                            Console.WriteLine("Your card was entered and updated.");
                        }
                        pressKey();
                        break;

                    case MenuChoice.CREATE_NEW_LIST:
                        //check to see if a previous list needed saving
                        if (List != null)
                        {
                            List.Save();
                        }

                        //create a new list object
                        List = new CreditCardList();
                        Console.WriteLine("\nAn empty Card list was created.\n\n");
                        pressKey();
                        break;

                    case MenuChoice.TXT_FILE_NEW:
                        //open the file dialog in Windows.Forms so the user can select a .txt file'
                        //check to see if a previous list needed saving
                        if (List != null)
                        {
                            //save the list
                            List.Save();
                        }
                        //create a new open file dialog object
                        OpenFileDialog dlg = new OpenFileDialog();
                        dlg.InitialDirectory = Application.StartupPath;
                        dlg.Title = "Select (.txt) File To Read From";
                        //filter out any other files besides .txt
                        dlg.Filter = "text files|*txt";

                        if (DialogResult.Cancel != dlg.ShowDialog())
                        {
                            FileName = dlg.FileName;

                        }
                        //create StreamReader and CreditCard object
                        StreamReader Read = null;

                        try
                        {
                            //create a new List of Credit Cards
                            List = new CreditCardList();
                            FileName = dlg.FileName;
                            //read the file and while we having hit the end
                            Read = new StreamReader(FileName);

                            while (Read.Peek() != -1)
                            {
                                //splits areas of the file separated by pipe char
                                String[] Areas = Read.ReadLine().Split('|');
                                //create a credit card object from the areas read in
                                Card = new CreditCard(Areas[0], Areas[3], Areas[4],
                                                      Areas[5], Areas[1], Areas[2]);
                                //add the Card to the List
                                List.Add(Card);
                            }
                        }
                        //display error message if the file generated a problem
                        catch (Exception e)
                        {
                            MessageBox.Show(e.GetType() + "\n" + e.Message + "\n" + e.StackTrace, "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Application.Exit();
                        }
                        finally
                        {
                            //if the Read isn't null, then close 
                            if (Read != null)
                            {
                                Read.Close();
                            }
                        }
                        if (List != null)
                            //card list was generated
                            Console.WriteLine("\nA new card list was generated from the file.");
                        else
                            //card list was not generated if still null
                            Console.WriteLine("\nA card list was NOT generated.");
                        pressKey();
                        break;

                    case MenuChoice.REMOVE_CARD:
                        if (List == null)
                        {
                            Console.WriteLine("A list may have not been created yet.");
                        }
                        else
                        {
                            //remove a card from input card number
                            Console.Write("\nPlease enter the card number: ");
                            NumCard = Console.ReadLine();

                            try
                            {
                                //for each creditcard in list, search for matching number
                                for (int n = 0; n < List.Count; n++)
                                {
                                    //if we find a match, remove the card
                                    if (List[n].CreditCardNum.Equals(NumCard))
                                    {
                                        List.Remove(List[n]);
                                        Console.WriteLine("\nCard was removed.");
                                    }
                                    else if (n == List.Count && !(List[n].CreditCardNum.Equals(NumCard)))
                                    {
                                        Console.WriteLine("\nCard wasn't found.");
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show(e.GetType() + "\n" + e.Message + "\n" + e.StackTrace, "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Application.Exit();
                            }
                        }
                        pressKey();
                        break;

                    case MenuChoice.CARD_POS:
                        //gets the position of the card the user wants to display
                        Console.Write("\nPlease enter card position in the list: ");
                        Position = Console.ReadLine();
                        //parse the string position to be used within List
                        Int32.TryParse(Position, out IntPos);
                        //subtract one to the integer, since index of List starts at 0
                        IntPos--;

                        try
                        {
                            //check the input position to see if valid
                            if (IntPos > 0 || IntPos <= List.Count)
                            {
                                //validate the card, gets its type, check Exp date and display
                                List[IntPos].GetCardType();
                                List[IntPos].CheckExpired();
                                List[IntPos].CheckValid();
                                Console.WriteLine("\tCredit Card #{0}", IntPos);
                                Console.WriteLine(List[IntPos].ToString());
                            }
                            else
                            {
                                Console.WriteLine("\nThat is an invalid position in the List.");
                            }
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.GetType() + "\n" + e.Message + "\n" + e.StackTrace, "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Application.Exit();
                        }
                        pressKey();
                        break;

                    case MenuChoice.CARD_NUM:
                        //display a card from the card number they input
                        Console.Write("\nPlease enter card number: ");
                        NumCard = Console.ReadLine();

                        try
                        {
                            //check for a match in card numbers in the List
                            for (int n = 0; n < List.Count; n++)
                            {
                                if (List[n].CreditCardNum.Equals(NumCard))
                                {
                                    //validate the card, gets its type, check Exp date and display
                                    List[n].GetCardType();
                                    List[n].CheckExpired();
                                    List[n].CheckValid();
                                    Console.WriteLine("\tCredit Card #{0}", n + 1);
                                    Console.WriteLine(List[n].ToString());
                                }
                                else if (n == List.Count && !(List[n].CreditCardNum.Equals(NumCard)))
                                {
                                    Console.WriteLine("\nCard was not found in the list.");
                                }

                            }
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.GetType() + "\n" + e.Message + "\n" + e.StackTrace, "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Application.Exit();
                        }
                        pressKey();
                        break;

                    case MenuChoice.CARD_PERSON:
                        //display a creditcard account from the persons name
                        Console.Write("\nPlease enter the card holder name: ");
                        Name = Console.ReadLine();

                        try
                        {
                            //search for input name in the List
                            for (int n = 0; n < List.Count; n++)
                            {
                                //if the card is found to be in the List
                                if (List[n].Name.Equals(Name))
                                {
                                    //validate the card, gets its type, check Exp date and display
                                    List[n].GetCardType();
                                    List[n].CheckExpired();
                                    List[n].CheckValid();
                                    Console.WriteLine("\tCredit Card #{0}", n + 1);
                                    Console.WriteLine(List[n].ToString());
                                }
                                else if (n == List.Count && !(List[n].Name.Equals(Name)))
                                {
                                    Console.WriteLine("\nCard was not found in the List.");
                                } 
                            }
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.GetType() + "\n" + e.Message + "\n" + e.StackTrace, "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Application.Exit();
                        }
                        pressKey();
                        break;

                    case MenuChoice.VALID_LIST:
                        //if the list is not null, continue..
                        if (List != null)
                        {
                            for (int n = 0; n < List.Count; n++)
                            {
                                //validate each card in the loop
                                List[n].GetCardType();
                                List[n].CheckExpired();
                                List[n].CheckValid();


                                if (List[n].TypeCard.Equals("INVALID"))
                                {
                                    //if the card is INVALID, do not display
                                }
                                else
                                    //display all valid cards
                                    Console.WriteLine(List[n].ToString());
                            }
                        }
                        else
                            Console.Write("\nThere may have not been a list created yet...");

                        pressKey();
                        break;

                    case MenuChoice.SORT_BY_NUM:
                        //sort the card objects by user name
                        //check for empty list (null)
                        if (List == null)
                        {
                            Console.Write("\nA Credit Card List may have not yet been created.");
                        }
                        else
                        {
                            //sort the List using the Sort() method
                            List.Sort();
                            Console.WriteLine("\nThe list was sorted.");

                        }
                        pressKey();
                        break;

                    case MenuChoice.DISPLAY_LIST:
                        //if the List is null, display appropriate message
                        if (List == null)
                        {
                            Console.Write("\nA Credit Card List may have not yet been created.");
                        }
                        else
                            //display the entire Credit Card List
                            for (int n = 0; n < List.Count; n++)
                            {
                                List[n].GetCardType();
                                List[n].CheckExpired();
                                List[n].CheckValid();
                                Console.WriteLine("\tCredit Card #{0}", n + 1);
                                Console.WriteLine(List[n].ToString());
                            }
                        pressKey();
                        break;
                }//end of switch
                choice = (MenuChoice)menu.GetChoice();
            }//end of while loop   

        }//end of main
        /// <summary>
        /// Allows the program to be at a paused state until the user enters any key
        /// </summary>
        public static void pressKey()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(true);
        }

    }
    #endregion
}
