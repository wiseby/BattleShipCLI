using System;

namespace BattleShipCLI
{
    class Program
    {
        /* GameBoard layout:
        *         0   1   2   3   4   5
        *   >   -------------------------
        *   0   | * |   |   |   | # |   |
        *   >   -------------------------
        *   1   | * |   |   |   | # |   |
        *   >   -------------------------
        *   2   |   |   |   |   | # |   |
        *   >   -------------------------
        *   3   |   |   | * | * | * |   |
        *   >   -------------------------
        *   4   |   | ~ |   |   |   |   |
        *   >   -------------------------
        *   5   |   |   |   |   |   |   |
        *   >   -------------------------
        *
        *       Hit = #
        *       Miss = ~
        *       Ship = *
        *       
        */


        static void Main(string[] args)
        {
            bool debug = false;

            if (args.Length > 0)
                {
                    if (args[0] == "--debug")
                    {
                        debug = true;
                    }
                }

            string[] responsToMiss = new string[] {
                "What the hell is going on!! Are you blind?!",
                "My grandma aims better than that!",
                "Fight harder or I´ll find someone who can!!"
            };
            string[] responsToHit = new string[5];
            string[] responsToSankShip = new string[5];

            int boardSizeX = 10;
            int boardSizeY = 10;

            int gameTiles = boardSizeX * boardSizeY;
            int shipGenerationNumber;

            int numberOfHits = 0;
            int torpedosUsed = 0;

            int fireCoordinateX = 0;
            int fireCoordinateY = 0;

            bool gameOn = true;

            char hit = '#';
            char miss = '~';
            char ship = '*';
            char torpedoStatus;

            char separatorX = '|';
            char separatorY = '-';

            Console.WriteLine("To battlestations!");
            Console.WriteLine("It's time for BattleShip!");
            Console.WriteLine("In the Command Line Interface!");
            Console.WriteLine("================================");
            Console.WriteLine("Highscore:");

            // TODO: Add function to exit program.

            // Select gameboard size.
            Console.WriteLine("\nSelect Gameboard Size:");
            Console.Write("[X]>>");
            boardSizeX = GameInput();
            Console.Write("[X]>>");
            boardSizeY = GameInput();

            Console.Write("Select number of ships:");
            Console.Write("[Ships]>>");
            shipGenerationNumber = GameInput();

            // Initialize Gameboard
            char[] interactLine = new char[boardSizeX * 4];
            char[] delimiterLine = new char[boardSizeX * 4];
            
            char[,] ships = new char[boardSizeY, boardSizeX];
            char[,] gameBoard = new char[boardSizeY, boardSizeX];
            // Set all indexes to one space " "
            for(int i = 0; i < gameBoard.GetLength(0); i++)
            {
                for(int j = 0; j < gameBoard.GetLength(1); j++)
                    gameBoard[i, j] = ' ';
            }

             // Generate ships.
            Random rand = new Random();
            for (int i = 0; i < shipGenerationNumber; i ++)
            {
                int randX = rand.Next(0, boardSizeX);
                int randY = rand.Next(0, boardSizeY);
                ships[randY, randX] = ship;
            }

            
            if (debug)
            {
                gameBoard = ships;
                Console.WriteLine("Debug Mode!");
            }

            Console.Write("Press Enter to start...");
            Console.ReadKey();

            // Not supported in Linux Mint!?
            // Console.SetWindowSize(100, gameBoard.GetLength(0)+ 35);

            do
            {
                Console.Clear();
                // Writes help for symbols.
                Console.WriteLine("=================");
                Console.WriteLine("Hit = " + hit);
                Console.WriteLine("Miss = " + miss);
                Console.WriteLine("Ship = " + ship);
                Console.WriteLine();
                Console.WriteLine("Torpedos used: " + torpedosUsed);
                Console.WriteLine("Number of hits: " + numberOfHits);
                Console.WriteLine();
                // Write out the gameboard.
                // Draw a delimiter --------

                // Keeps track of the interactLine count and index of current
                // value.
                int interactLineCount = 0;
                int interactIndex = 0;

                // For alining cordinates properly
                Console.Write(" ");
                // Print coordinates for X
                for (int i = 0; i < interactLine.Length; i++)
                {
                    if (i % 4 == 0)
                    {
                        Console.Write(" " + interactIndex + " ");
                        interactIndex++;
                        if (i % 2 == 0 && interactIndex < 10)
                        {
                            Console.Write(" ");
                        }
                    }
                    
                }

                for (int i = 0; i < (gameBoard.GetLength(0)) * 2; i++)
                {
                    Console.WriteLine();
                    
                    // Resetting interactIndex to 0 for a new interactLine.
                    interactIndex = 0;
                    

                    // varje ojämnt tal är en interactLine
                    if (i % 2 == 1)
                    {
                        // Draw a gameLine

                        for (int j = 0; j <= interactLine.Length; j++)
                        {
                            

                            // Kollar vart annat element för antingen interagera
                            // eller avskiljning.
                            if (j % 2 == 0)
                            {

                                // Var fjärde element ska vara en avskiljare
                                if (j % 4 == 0)
                                {
                                    Console.Write(separatorX);
                                }
                                else
                                {
                                    // Kontrollera om där finns något från
                                    // inteactIndex.
                                    Console.Write(gameBoard[interactLineCount, interactIndex]);
                                    interactIndex++;
                                }
                            }
                            else
                            {
                                Console.Write(" ");
                            }
                            // End Interact Line with Coordinate
                            if (j == interactLine.Length)
                                {
                                    Console.Write(" " + interactLineCount + " ");
                                }
                        }
                        interactLineCount++;
                    }
                    // Varje jämnt tal är en delimiterLine
                    else
                    {
                        for (int j = 0; j < delimiterLine.GetLength(0) + 1; j++)
                        {
                            Console.Write(separatorY);
                        }
                    }
                }
                Console.WriteLine("   ");
                for (int j = 0; j < delimiterLine.GetLength(0) + 1; j++)
                {
                    Console.Write(separatorY);
                }

                do
                {
                    // Type in coordinates
                    Console.WriteLine();
                    Console.WriteLine("General! Type in Coordinates for torpedos!");
                    Console.Write("[X]>>");
                    fireCoordinateX = GameInput();
                    Console.Write("[Y]>>");
                    fireCoordinateY = GameInput();
                    Console.Write("Torpedos away! At Coordinates: ");
                    Console.WriteLine(fireCoordinateX + "," + fireCoordinateY);

                    // Is the coordinates valid gameboard values?
                    if (CoordinateValidation(gameBoard, fireCoordinateX, fireCoordinateY))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Those coordinates doesn´t add up!");
                        Console.WriteLine("Try Again");
                    }

                } while (true);

                // Update gameboard.
                if (HitDetection(fireCoordinateX, fireCoordinateY, ships, ship))
                {
                    Console.WriteLine("Excellent commander! Thay never saw it comming!");
                    
                    numberOfHits++;
                    torpedoStatus = hit;
                }
                else
                {
                    Console.WriteLine("What the hell is going on!! Are you blind?!");
                    Console.WriteLine("Fire again God dam it!!");
                    torpedoStatus = miss;
                }
                torpedosUsed++;

                gameBoard = UpdateGame(
                    fireCoordinateX, fireCoordinateY, gameBoard, torpedoStatus, ships);

                // Check win conditions.
                if (PlayerHasWon(ships, ship))
                {
                    gameOn = false;
                }

                Console.ReadKey();
                } while (gameOn);

                Console.WriteLine("Victory is ours! The enemy has been extinguished!");
                Console.ReadKey();
        }


        public static int GameInput()
        {
            int input = 0;
            try
            {
                input = Convert.ToInt32(Console.ReadLine());
            }
            catch (System.Exception)
            {
                Console.WriteLine("Sir! Thats not the right format for the coordinates!");
                Console.WriteLine("The enemy is getting away! Try again!");
            }

            return input;
        }

        public static bool CoordinateValidation(char[,] gameBoard, int x, int y)
        {
            if (x < gameBoard.GetLength(1) && y < gameBoard.GetLength(0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool HitDetection(int x, int y, char[,] ships, char marker)
        {
            if (ships[y, x] == marker)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // Ett bra exempel på vart en strukt skulle vara lämplig.
        // Man kan göra parameterlistor mycket kortare!
        public static char[,] UpdateGame(int x, int y, char[,] gameBoard, char marker, char[,] ships)
        {
            if (gameBoard[y, x] == ' ')
            {
                gameBoard[y, x] = marker;
                ships[y, x] = marker;
                return gameBoard;
            }
            return gameBoard;
        }


        public static bool PlayerHasWon(char[,] ships, char marker)
        {
            for (int row = 0; row < ships.GetLength(0); row++)
            {
                for (int i = 0; i < ships.GetLength(1); i++)
                {
                    if (ships[row, i] == marker)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
