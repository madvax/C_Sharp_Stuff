using System;
using System.IO; // For StreamReader
using System.Text.RegularExpressions; // For Regex

/* Example console C# program for students. -- Harold Wilson, Spetember 2018 
 * Hangman with 8-letter words. Version 1.0
 * You get 12 guesses to sole the 8-letter word.
 * A few cool items to note here:
 *     - The user input is character input
 *     - Colorozed text for unguessed letters 
 *     - Changeable work files for now puzzle
 *  
 *  One of the main objectives for students is to show how 
 *  unconditional for() loops can be used to index arrays.
 *  An additional goal is to show how to break a problem down 
 *  into codeable steps.  
 */

namespace HangMan_01
{
    class Program
    {
        static void Main(string[] args)
        {
            char[] unused_lettters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            char unused_character = '_';                   // Charactger to use for unguessed letter spaces
            char[] solution = new char[8];                 // string for the soluton  
            char[] guess = { '_', '_', '_', '_', '_', '_', '_', '_' }; // The users guess for the word initialized to all '_' characters
            int attempts = 6;                              // Num ber of allowable guesses
            int attempts_taken = 0;                        // Counter for the number of attempts taken 
            string data_file_name = "c:\\Temp\\words.dat"; // Data file for words/puzzles
            Random rnd = new Random();                     // Generic object of type Random for random puzzle selection 
            int puzzle_number = 0;                         // Random puzzle number
            string word = "";                              // One of the words at random from the data file 
            bool solved = false;                           // Flag to set if puzzle is solved
            char letter_from_user = ' ';                   // The letter the user enters 
            bool letter_already_used = true;               // Flag to set of a chosen letter has hlready been used
            bool letter_match = false;

            // ------------------------------------------------------------------------------------
            // Open the file and read in the words as potential puzzles  
            StreamReader streamReader = new StreamReader(data_file_name); //get the file
            string stringWithMultipleSpaces = streamReader.ReadToEnd(); //load file to string
            streamReader.Close();
            Regex r = new Regex(" +"); //specify delimiter (spaces)
            string[] words = r.Split(stringWithMultipleSpaces); //(convert string to array of words)

            // ------------------------------------------------------------------------------------
            // Select a random puzzle form the set of available puzzles
            puzzle_number = rnd.Next(1, words.Length);

            // ------------------------------------------------------------------------------------
            // Welcome page
            Console.WriteLine("\n\nFound {0} puzzles to pick from", words.Length);
            Console.WriteLine("Selecting puzzle number {0}", puzzle_number);

            // ------------------------------------------------------------------------------------
            // Convert the word to an array of upper case characters
            solution = words[puzzle_number].ToCharArray();
            for (int i = 0; i < solution.Length; i++)
            {
                solution[i] = char.ToUpper(solution[i]);
            }

            //-------------------------------------------------------------------------------------
            // Now we are ready to play
            Console.WriteLine("\n\n");
            //
            Console.WriteLine(words[puzzle_number]); // for debugging winning game
            //
            Console.WriteLine("Shall We begin...");
            Console.WriteLine("\n\n");
            Console.ReadKey();

            attempts_taken = 0;
            // ====================================================================================
            // *** Main Game loop *** 
            do
            {
                // ----------------------------------------------------------------------
                // handy loop variables
                letter_already_used = true;

                // ----------------------------------------------------------------------
                // Print the turn number 
                Console.Clear();
                Console.WriteLine("\n");
                Console.Write("Strike {0} of {1} used: ", attempts_taken, attempts);
                for (int p = 0; p < attempts_taken; p++)
                {
                    Console.Write("[");                         //
                    Console.ForegroundColor = ConsoleColor.Red; // Red X for a strike 
                    Console.Write("X");                         //
                    Console.ResetColor();
                    Console.Write("] ");
                }
                for (int q = 0; q < attempts - attempts_taken; q++)
                {
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Green; //
                    Console.Write("0");                           // green 0 for an unused attempt 
                    Console.ResetColor();                         //
                    Console.Write("] ");
                }
                Console.WriteLine("\n");

                // ----------------------------------------------------------------------
                // Print the guess so far
                Console.Write("Mystery Word: ");
                for (int i = 0; i < guess.Length; i++)
                {
                    Console.Write("{0} ", guess[i]);
                }
                Console.WriteLine("\n\n");

                // ----------------------------------------------------------------------
                // Print the available letters
                Console.Write("Available Letters: ");
                for (int i = 0; i < unused_lettters.Length; i++)
                {
                    Console.Write("{0} ", unused_lettters[i]);
                }

                // ----------------------------------------------------------------------
                // Get a letter from the user
                Console.WriteLine("\n");
                Console.Write("Select a letter from the list of available letters above:");
                letter_from_user = Console.ReadKey().KeyChar;
                letter_from_user = char.ToUpper(letter_from_user);

                // ----------------------------------------------------------------------
                // Check to see if the letter is already been used
                for (int i = 0; i < unused_lettters.Length; i++)
                {
                    if (unused_lettters[i] == letter_from_user)
                    {
                        // letter is found in the list so it is not already used
                        letter_already_used = false;
                        break;
                    }
                }

                // ----------------------------------------------------------------------
                // If the letter is unused then process it
                if (!letter_already_used)
                {
                    // Score the letter from the user 
                    letter_match = false;
                    for (int j = 0; j < solution.Length; j++)
                    {

                        // ----------------------------------------------------
                        // Add the letter to the guess if there are matches
                        if (letter_from_user == solution[j])
                        {
                            guess[j] = letter_from_user;
                            letter_match = true;
                        }
                    }
                    if (!letter_match) attempts_taken++;

                    // ----------------------------------------------------
                    // Remove the letter from the list of availble letters 
                    for (int k = 0; k < unused_lettters.Length; k++)
                    {
                        if (unused_lettters[k] == letter_from_user)
                        {
                            unused_lettters[k] = unused_character;
                            break;
                        }
                    }


                }
                else
                {
                    // --------------------------------------------------------
                    // Skip this selection as the letter is already used

                    // Console.WriteLine("\n\n *** LETTER {0} ALREADY USED ***", letter_from_user);
                    // Console.WriteLine("Press <Enter> to continue.")
                    // Console.ReadKey();
                }

                //-----------------------------------------------------------------------
                // Check to see if we have a winner
                solved = true;
                for (int m = 0; m < guess.Length; m++)
                {
                    if (guess[m] != solution[m])
                    {
                        solved = false;
                        break;
                    }
                }

            } while (attempts_taken < attempts && solved == false);

            // ------------------------------------------------------------------------------------
            // process game results
            string s = new string(solution);
            if (solved)
            {

                // ----------------------------------------------------------------------
                // Print winning screen
                Console.Clear();
                Console.WriteLine("\n");
                Console.WriteLine("You have used  {0} of {1} strikes.\n", attempts_taken, attempts);
                Console.Write("Mystery Word: ");
                for (int i = 0; i < guess.Length; i++)
                {
                    Console.Write("{0} ", guess[i]);
                }
                Console.WriteLine("\n\n");
                Console.Write("Available Letters: ");
                for (int i = 0; i < unused_lettters.Length; i++)
                {
                    Console.Write("{0} ", unused_lettters[i]);
                }
                Console.WriteLine("\n\nYou won!");
                Console.WriteLine("The word was \"{0}\".", s);

            }
            else
            {

                // ----------------------------------------------------------------------
                // Print Losing screen 
                Console.Clear();
                Console.WriteLine("\n");
                Console.WriteLine("You have exhauted {0} of {1} strikes.\n", attempts_taken, attempts);
                Console.Write("Mystery Word: ");
                for (int i = 0; i < guess.Length; i++)
                {
                    if (guess[i] == unused_character)
                    {
                        Console.ForegroundColor = ConsoleColor.Red; // \
                        Console.Write("{0} ", solution[i]);         //  > Print unguessed letters in red
                        Console.ResetColor();                       // /
                    }
                    else
                    {
                        Console.Write("{0} ", guess[i]);
                    }
                }
                Console.WriteLine("\n\n");
                Console.Write("Available Letters: ");
                for (int i = 0; i < unused_lettters.Length; i++)
                {
                    Console.Write("{0} ", unused_lettters[i]);
                }
                Console.WriteLine("\n\nYou lost");
                Console.WriteLine("The word was \"{0}\".", s);
            }

            Console.ReadKey(); // Hold the window open for IDE execution
        } /* End Main */
    } /* End Program */
} /* End Namespece */
