/*
 * Class : CSCI 2210-201  --Data Structures--
 * File Name : CreditCardList.cs
 * 
 * Purpose	: Create a class that creates a List of Credit Card objects and
 *            implements ways to add, remove, sort, and search for individual
 *            Credit Cards in the List.
 *
 * Author:Nick Bowers
 * E-Mail: BOWERSNA@goldmail.etsu.edu
 * 
 * Create Date    : Tuesday, September 24, 2013
 * Last Modified  : Wednesday, September 25, 2013
 * Modified By	: Nick Bowers
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;

namespace CreditCardValidator
{
    class CreditCardList: Collection<CreditCard>
    {
        //private List attribute used throughout the class
        private List<CreditCard> List;
        private static bool SaveNeeded;

        /// <summary>
        /// Default constructor for a new List of Credit Cards
        /// </summary>
        public CreditCardList()
        {
            List = new List<CreditCard>();
            SaveNeeded = false;
        }
        /// <summary>
        /// Override for Object.GetHashCode()
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        /// <summary>
        /// Override for the List.Add method to add a credit card
        /// </summary>
        /// <param name="index">index for where to add</param>
        /// <param name="item">credit card being added</param>
        protected override void InsertItem(int index, CreditCard item)
        {
            base.InsertItem(index, item);
            SaveNeeded = true;
        }
        /// <summary>
        /// Overrride for the List.Remove method to remove a credit card
        /// </summary>
        /// <param name="index">index to remove card</param>
        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
            SaveNeeded = true;
        }
        /// <summary>
        /// Override for the List.Set method
        /// </summary>
        /// <param name="index">index to set item</param>
        /// <param name="item">card being set</param>
        protected override void SetItem(int index, CreditCard item)
        {
            base.SetItem(index, item);
            SaveNeeded = true;
        }
        /// <summary>
        /// Override for clearing the list of credit cards
        /// </summary>
        protected override void ClearItems()
        {
            base.ClearItems();
            SaveNeeded = true;
        }
        /// <summary>
        /// Override of IEquatable to use the .Equals method for two credit card
        /// types
        /// </summary>
        /// <param name="obj">object being compared</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        /// <summary>
        /// Allows a card to be added to the list with the + operator
        /// </summary>
        /// <param name="List">Credit Card List </param>
        /// <param name="Card">Card being added to List</param>
        /// <returns></returns>
        public static CreditCardList operator +(CreditCardList List, CreditCard Card)
        {
            //create a Temp CreditCard List
            CreditCardList Temp = new CreditCardList();
            //if the card being added isnt alread there, then add the card
            if (!Temp.List.Contains(Card))
                Temp.List.Add(Card);
            SaveNeeded = true;
            //return the List
            return Temp;
        }
        /// <summary>
        /// Allows a card to be removed from the list with the - operator
        /// </summary>
        /// <param name="List">Credit Card List</param>
        /// <param name="Card">Card being removed</param>
        /// <returns></returns>
        public static CreditCardList operator -(CreditCardList List, CreditCard Card)
        {
            //create a Temp List
            CreditCardList Temp = new CreditCardList();
            //remove the card from the List
            Temp.List.Remove(Card);
            SaveNeeded = true;
            return Temp;
        }
        /// <summary>
        /// Sorts the list of credit cards by user name
        /// </summary>
        public void Sort()
        {
            //sort the list by user name
            List.Sort();
            SaveNeeded = true;
        }
        /// <summary>
        /// Saves the current credit card list to the file
        /// </summary>
        public void Save()
        {
            //create a save file window object
            //setup the title and filter of files
            //also setup the default folder to open in
            if (SaveNeeded == true)
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Title = "Save Credit Card List";
                save.Filter = "text files|*txt";
                save.InitialDirectory = Application.StartupPath;

                if (save.ShowDialog() == DialogResult.Cancel)
                    return;

                StreamWriter Write = null;
                try
                {
                    //create a new SteamWriter to write to a file
                    Write = new StreamWriter(new FileStream(save.FileName, FileMode.Create, FileAccess.Write));
                    //for each Card in the list...
                    for (int n = 0; n < List.Count; n++)
                    {
                        //create a copy of the current credit card in position n
                        CreditCard SaveCard = new CreditCard(this.List[n]);
                        //write the correct String varibales associated with each credit card to the file
                        Write.WriteLine(SaveCard.Name + "|" + SaveCard.Telephone + "|" +
                                        SaveCard.EMailAddress + "|" + SaveCard.CreditCardNum + "|" +
                                        SaveCard.ExpDateMonth + "|" + SaveCard.ExpDateYear);
                    }
                }
                //catch any Exception and throw and Error message to screen
                catch (Exception e)
                {
                    MessageBox.Show(e.GetType() + "\n" + e.Message + "\n" + e.StackTrace, "Output Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
                finally
                {
                    if (Write != null)
                        Write.Close();
                }

            }
                //set the SaveNeeded bool to false
                SaveNeeded = false;
           
        }
        /// <summary>
        /// Overrided ToString from CreditCard class
        /// </summary>
        /// <returns>A formatted string of information about the list of credit cards</returns>
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
