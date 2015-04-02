/*
 * Class : CSCI 2210-201    ---Data Structures---
 * File Name : CreditCard.cs
 * 
 * Purpose	: Create a class that creates a CreditCard object that can
 *            be Validated through Luhn's Algorithm to verify the card 
 *            number, also verifying the expiration date and type of card.
 *          
 *
 * Author:  Nick Bowers
 * E-Mail: BOWERSNA@goldmail.etsu.edu
 * 
 * Create Date	 : Wednesday, September 11, 2013
 * Last Modified  : Wednesday, September 25, 2013
 * Modified By	: Nick Bowers
 *
 */
using System;
using System.Text.RegularExpressions;

namespace CreditCardValidator
{
    public class CreditCard : IComparable<CreditCard>, IEquatable<CreditCard>
    {
        public string Name;
        public string CreditCardNum;
        public string ExpDateMonth;
        public string ExpDateYear;
        public string Telephone;
        public string EMailAddress;
        public string TypeCard;
        public string Expired;
   
        Regex visa = new Regex(@"^[4]\d\d\d\d\d\d"); //Pattern matching Visa card
        Regex mastercard = new Regex(@"^[5][1]\d\d\d\d|^[5][4]\d\d\d\d|^[5][5]\d\d\d\d"); //Pattern matching Mastercard 
        Regex americanExp = new Regex(@"^[3][4]\d\d\d\d|^[3][7]\d\d\d\d"); //Pattern matching American Express
        Regex discover = new Regex(@"^[6][0][1][1]\d\d|^[6][4][4]\d\d\d|^[6][5]\d\d\d\d"); //Pattern matching Discover

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CreditCard()
        {
            Name = "";
            CreditCardNum = "";
            ExpDateMonth = "";
            ExpDateYear = "";
            Telephone = "";
            EMailAddress = "";
            TypeCard = CardType.OTHER.ToString();   //set default type to OTHER
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name of the credit card holder</param>
        /// <param name="creditCardNum">Credit card number for validation</param>
        /// <param name="expDate">The expiration date of the card</param>
        /// <param name="tele">The telephone of the user</param>
        /// <param name="address">The address of the cardholder</param>
        public CreditCard(string name, string creditCardNum, string expDateMonth, string expDateYear, string tele, string address)
        {
            Name = name;
            CreditCardNum = creditCardNum;
            ExpDateMonth = expDateMonth;
            ExpDateYear = expDateYear;
            Telephone = tele;
            EMailAddress = address;
            TypeCard = CardType.OTHER.ToString();   //set default type to OTHER
        }
        /// <summary>
        /// Copy Constructor for copying one Credit Card to another
        /// </summary>
        /// <param name="Card">Card being copied</param>
        public CreditCard(CreditCard Card)
        {
            Name = Card.Name;
            CreditCardNum = Card.CreditCardNum;
            ExpDateMonth = Card.ExpDateMonth;
            ExpDateYear = Card.ExpDateYear;
            Telephone = Card.Telephone;
            EMailAddress = Card.EMailAddress;
            TypeCard = Card.TypeCard;
        }
        /// <summary>
        /// The .Equals method comparing two credit card accounts
        /// </summary>
        /// <param name="Card"></param>
        /// <returns></returns>
        public bool Equals(CreditCard Card)
        {
            if (this.Name.Equals(Card.Name) || this.CreditCardNum.Equals(Card.CreditCardNum))
            {
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// Overridden Obj.CompareTo(Obj) method 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if(obj == null)
                return base.Equals(obj);
            if (!(obj is CreditCard))
                throw new InvalidCastException("The argument is not a Credit Card.");
            else
                return Equals(obj as CreditCard);
        }
        /// <summary>
        /// Overridden object GetHashCode method
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        /// <summary>
        /// CompareTo method used to make it available to compare the
        /// names of the card holders of each credit card
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(CreditCard other)
        {
            String OName = other.Name;
            
            if (other == null)
            {
                return 1;
            }

            if (OName != null)
            {
                return this.Name.CompareTo(OName);
            }
            else
            {
                throw new ArgumentException("Object is not a Credit Card.");
            }

        }
        /// <summary>
        /// Determines whethere the Expiration date input is expired or not expired
        /// vs. the Current Date and Year
        /// </summary>
        public void CheckExpired()
        {
            DateTime CurrentDate = DateTime.Today;
            int CurrentMonth = CurrentDate.Month;
            int CurrentYear = CurrentDate.Year;
            //CurrentMonth is in range 1-12
            //CurrentYear is 2013

            int Year = System.Convert.ToInt32(ExpDateYear);
            int Month = System.Convert.ToInt32(ExpDateMonth);
            //convert the strings to integers for comparing

            //Compare the input Year to the current Year
            if (Year == CurrentYear)   //If input year is Equal to Current Year..
            {
                //since years are equal, we must compare Months
                if (Month <= CurrentMonth)
                {
                    Expired = "--- Expired ---"; //card is Expired
                }
            }
            else if (Year < CurrentYear) //if current year is greater than input year
            {
                Expired = "--- Expired ---"; //card is Expired
            }
            else
            {
                Expired = "--- Not Expired ---";
            }
        }
        /// <summary>
        /// Determine the type of card using Regular Expressions decalared above
        /// </summary>
        public void GetCardType()
        {
            //Compare the credit card number to each Regular Expression pattern created for each card to determine its type
            Match matchV = visa.Match(CreditCardNum);
            Match matchA = americanExp.Match(CreditCardNum);
            Match matchD = discover.Match(CreditCardNum);
            Match matchM = mastercard.Match(CreditCardNum);

            //Check for each success in the matches found
            //If a success was found, set the card type accordingly
            if (matchV.Success)
            {
                TypeCard = CardType.VISA.ToString();
            }
            else if (matchA.Success)
            {
                TypeCard = CardType.AMERICAN_EXPRESS.ToString();
            }
            else if (matchD.Success)
            {
                TypeCard = CardType.DISCOVER.ToString();
            }
            else if (matchM.Success)
            {
                TypeCard = CardType.MASTERCARD.ToString();
            }
        }
        /// <summary>
        /// Determines where the Credit Card numbered enter is VALID.
        /// </summary>
        public void CheckValid()
        {
            //convert string Credit Card Number to an array of char's
            char[] ArrayCardNum = this.CreditCardNum.ToCharArray();
            char NumCard;   //stores single char in array
            char OddNumCard;    //stores single char
            int INumCard;   //converted char to int is stored

            int AddFirst = 0,   //store first number set
                AddSecond = 0,  //store second number set
                Temp = 0,      //temp variable used to store int
                Catcher = 1,    //variable to indicate even numbers
                i = 0,          //variable used to increment array
                r = 1;          //variable used to increment array 

            int ArrayL = ArrayCardNum.Length - 1;   //length of array -1 for the loop

            string Left,   //stores left most number
                   Right;  //stores right most number


            for (int n = 0; n <= ArrayL; n++)
            {
                if (Catcher == 1)                                   //if the cathcer variable hasn't changed
                {
                    if (i <= ArrayL)
                    {
                        NumCard = ArrayCardNum[i];                          //single char at position i is in NumCard
                        i = i + 2;                                           //increment i to every other position   
                        INumCard = System.Int32.Parse(NumCard.ToString());  //parse the single character string to an integer

                        INumCard = INumCard * 2;                         //double the digit

                        if (INumCard >= 10)                         //if the previous number was 2 digits, we split them up and sum them together
                        {
                            Left = INumCard.ToString().Substring(0, 1); //store the sum of the positions most Left
                            Right = INumCard.ToString().Substring(1, 1);//store the sum of the positions most Right

                            AddFirst += (Convert.ToInt32(Left) + Convert.ToInt32(Right));  //store the converted left and right position sums
                            Catcher = 0;                                    //set our Catcher to 0
                        }
                        else
                        {
                            AddFirst += INumCard;                            //add the current int to the AddFirst variable
                            Catcher = 0;                                     //set our catcher to 0
                        }
                    }
                }
                else
                {
                    OddNumCard = ArrayCardNum[r];        //Temp is now the odd set of digits not being doubled
                    r = r + 2;                                //incerement n to odd position in array
                    Temp = System.Int32.Parse(OddNumCard.ToString());   //parse character into an integer value
                    AddSecond += Temp;                  //store the sum of the positions into the AddSecond variable
                    Catcher = 1;                       //reset the Catcher to 1
                }

            }
            if ((AddFirst + AddSecond) % 10 != 0)              //if the sum of the positions is not divisible by 10
            {
                TypeCard = CardType.INVALID.ToString();        //set the Type of card to INVALID
            }


        }
        /// <summary>
        /// Convert the Credit Card information into a displayable string
        /// </summary>
        /// <returns>Brief summary of credit card information</returns>
        public override String ToString()
        {
            return "\n" +
                   "\t----------------------------------------\n" +
                   "\tCard Holder:            " + Name + "\n" +
                   "\tAddress:                " + EMailAddress + "\n" +
                   "\tTelephone #:            " + Telephone + "\n" +
                   "\tCard Number:            " + CreditCardNum + "\n" +
                   "\tExpiration Date:        " + ExpDateMonth + "/" + ExpDateYear + "\n" +
                   "\t                        " + Expired + "\n" +
                   "\tCard Type               " + TypeCard + "\n" +
                   "\t----------------------------------------\n";
        }
    }//end Main
}//end Driver.cs

