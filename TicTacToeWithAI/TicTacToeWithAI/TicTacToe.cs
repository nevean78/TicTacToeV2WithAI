using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TicTacToeWithAI
{
    class TicTacToe
    {
        public List<string> positions = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        public bool gameStatus = false;
        public string userOrMachineTurn /* X - User && O - Machine */;
        public int turnCount = 0;

        public TicTacToe()
        {
            StartGame();
        }

        public void FirstToPlay()
        {
            char[] chars = "XO".ToCharArray();
            Random r = new Random();
            int i = r.Next(chars.Length);
            userOrMachineTurn = chars[i].ToString();
        }

        public void StartGame()
        {
            FirstToPlay();

            while (!gameStatus)
            {
                TableRender();
                if (userOrMachineTurn == "X")
                    UserChoice();
                else
                    MachineChoice();
                TableRender();
                CheckGameStatus();
                ChangeTurn();
            }
        }

        private void TableRender()
        {  
            Console.Clear();

            Console.WriteLine("     |     |      ");
            Console.WriteLine("  {0}  |  {1}  |  {2}", positions[0], positions[1], positions[2]);
            Console.WriteLine("_____|_____|_____ ");
            Console.WriteLine("     |     |      ");
            Console.WriteLine("  {0}  |  {1}  |  {2}", positions[3], positions[4], positions[5]);
            Console.WriteLine("_____|_____|_____ ");
            Console.WriteLine("     |     |      ");
            Console.WriteLine("  {0}  |  {1}  |  {2}", positions[6], positions[7], positions[8]);
            Console.WriteLine("     |     |      ");
        }

        private void ChangeTurn()
        {
            userOrMachineTurn = userOrMachineTurn == "X" ? "O" : "X";
        }

        private void UserChoice()
        {
            int userChoice = 0;
            bool isParsable;
            bool isAvailable;

            Console.WriteLine("Jogador: " + userOrMachineTurn);
            Console.WriteLine("Esolha uma casa de 1 a 9 conforme as casas disponíveis na tabela!");

            do
            {
                var choice = Console.ReadLine();
                isParsable = Int32.TryParse(choice, out userChoice);
                isAvailable = isParsable ? positions.Where(x => x == Convert.ToString(userChoice)).Any() : false;

                if (!isParsable || (userChoice <= 0 || userChoice >= 10) || !isAvailable)
                {
                    Console.WriteLine("Erro! A sua escolha não é válida!");
                }
            } while (!isParsable || (userChoice <= 0 || userChoice >= 10) || !isAvailable);

            var index = positions.IndexOf(Convert.ToString(userChoice));
            positions[index] = userOrMachineTurn;
            turnCount++;

        }

        #region MachineChoices
        private void MachineChoice()
        {
            Console.WriteLine("Jogador: " + userOrMachineTurn);
            Console.WriteLine("A máquina está a fazer a sua jogada!");

            Thread.Sleep(1000);

            bool winCheck = WinCheck();

            if (!winCheck)
                DefensiveCheck();

            turnCount++;
        }

        private void DefensiveCheck()
        {
            int horizontalCheck = HorizontalCheck("X"),
                verticalCheck = VerticalCheck("X"),
                diagonalCheck = DiagonalCheck("X");

            if (horizontalCheck == -1 && verticalCheck == -1 && diagonalCheck == -1)
            {
                var freePositions = positions.Where(x => x != "X" && x != "O").ToList();
                Random r = new Random();
                IEnumerable<string> position = freePositions.OrderBy(x => r.Next()).Take(1);
                var index = positions.IndexOf(Convert.ToString(position.First()));
                positions[index] = userOrMachineTurn;
            }
            else
            if (horizontalCheck != -1)
            {
                positions[horizontalCheck] = userOrMachineTurn;
            }
            else
            if (verticalCheck != -1)
            {
                positions[verticalCheck] = userOrMachineTurn;
            }
            else
            if (diagonalCheck != -1)
            {
                positions[diagonalCheck] = userOrMachineTurn;
            }
        }

        private bool WinCheck()
        {
            int horizontalCheck = HorizontalCheck("O"),
                verticalCheck = VerticalCheck("O"),
                diagonalCheck = DiagonalCheck("O");

            if (horizontalCheck != -1)
            {
                positions[horizontalCheck] = userOrMachineTurn;
                return true;
            }
            else
            if (verticalCheck != -1)
            {
                positions[verticalCheck] = userOrMachineTurn;
                return true;
            }
            else
            if (diagonalCheck != -1)
            {
                positions[diagonalCheck] = userOrMachineTurn;
                return true;
            }
            else
                return false;
        }

        private int HorizontalCheck(string machineOrUser)
        {
            bool horizontalLine1 = (positions[0] == machineOrUser && positions[1] == machineOrUser) || (positions[1] == machineOrUser && positions[2] == machineOrUser) || (positions[0] == machineOrUser && positions[2] == machineOrUser);
            bool horizontalLine2 = (positions[3] == machineOrUser && positions[4] == machineOrUser) || (positions[4] == machineOrUser && positions[5] == machineOrUser) || (positions[3] == machineOrUser && positions[5] == machineOrUser);
            bool horizontalLine3 = (positions[6] == machineOrUser && positions[7] == machineOrUser) || (positions[7] == machineOrUser && positions[8] == machineOrUser) || (positions[6] == machineOrUser && positions[8] == machineOrUser);

            var freePositions = positions.Where(x => x != "X" && x != "O").ToList();

            if (horizontalLine1 && (freePositions.Contains("1") || freePositions.Contains("2") || freePositions.Contains("3")))
                return positions.IndexOf(positions.Where(x => x != "X" && x != "O" && (x == "1" || x == "2" || x == "3")).First().ToString());
            else
            if (horizontalLine2 && (freePositions.Contains("4") || freePositions.Contains("5") || freePositions.Contains("6")))
                return positions.IndexOf(positions.Where(x => x != "X" && x != "O" && (x == "4" || x == "5" || x == "6")).First().ToString());
            else
            if (horizontalLine3 && (freePositions.Contains("7") || freePositions.Contains("8") || freePositions.Contains("9")))
                return positions.IndexOf(positions.Where(x => x != "X" && x != "O" && (x == "7" || x == "8" || x == "9")).First().ToString());
            else
                return -1;
        }
        
        private int VerticalCheck(string machineOrUser)
        {
            bool verticalLine1 = (positions[0] == machineOrUser && positions[3] == machineOrUser) || (positions[3] == machineOrUser && positions[6] == machineOrUser) || (positions[0] == machineOrUser && positions[6] == machineOrUser);
            bool verticalLine2 = (positions[1] == machineOrUser && positions[4] == machineOrUser) || (positions[4] == machineOrUser && positions[7] == machineOrUser) || (positions[1] == machineOrUser && positions[7] == machineOrUser);
            bool verticalLine3 = (positions[2] == machineOrUser && positions[5] == machineOrUser) || (positions[5] == machineOrUser && positions[8] == machineOrUser) || (positions[2] == machineOrUser && positions[8] == machineOrUser);

            var freePositions = positions.Where(x => x != "X" && x != "O").ToList();

            if (verticalLine1 && (freePositions.Contains("1") || freePositions.Contains("4") || freePositions.Contains("7")))
                return positions.IndexOf(positions.Where(x => x != "X" && x != "O" && (x == "1" || x == "4" || x == "7")).First().ToString());
            else
            if (verticalLine2 && (freePositions.Contains("2") || freePositions.Contains("5") || freePositions.Contains("8")))
                return positions.IndexOf(positions.Where(x => x != "X" && x != "O" && (x == "2" || x == "5" || x == "8")).First().ToString());
            else
            if (verticalLine3 && (freePositions.Contains("3") || freePositions.Contains("6") || freePositions.Contains("9")))
                return positions.IndexOf(positions.Where(x => x != "X" && x != "O" && (x == "3" || x == "6" || x == "9")).First().ToString());
            else
                return -1;
        }

        private int DiagonalCheck(string machineOrUser)
        {
            bool diagonalLine1 = (positions[0] == machineOrUser && positions[4] == machineOrUser) || (positions[4] == machineOrUser && positions[8] == machineOrUser) || (positions[0] == machineOrUser && positions[8] == machineOrUser);
            bool diagonalLine2 = (positions[2] == machineOrUser && positions[4] == machineOrUser) || (positions[4] == machineOrUser && positions[6] == machineOrUser) || (positions[2] == machineOrUser && positions[6] == machineOrUser);
          
            var freePositions = positions.Where(x => x != "X" && x != "O").ToList();

            if (diagonalLine1 && (freePositions.Contains("1") || freePositions.Contains("5") || freePositions.Contains("9")))
                return positions.IndexOf(positions.Where(x => x != "X" && x != "O" && (x == "1" || x == "5" || x == "9")).First().ToString());
            else
            if (diagonalLine2 && (freePositions.Contains("3") || freePositions.Contains("5") || freePositions.Contains("7")))
                return positions.IndexOf(positions.Where(x => x != "X" && x != "O" && (x == "3" || x == "5" || x == "7")).First().ToString());
            else
                return -1;
        }

        #endregion

        #region GameStatus
        private void CheckGameStatus()
        {
            if (HorizontalLine() || VerticalLine() || DiagonalLine())
            {
                gameStatus = true;
                Console.WriteLine("O jogador '" + userOrMachineTurn + "' venceu a partida!");
            }
            else if (turnCount == 9)
            {
                gameStatus = true;
                Console.WriteLine("Houve empate!");
            }
        }

        /* Check Game Lines */
        private bool HorizontalLine()
        {
            bool horizontalLine1 = positions[0] == positions[1] && positions[0] == positions[2];
            bool horizontalLine2 = positions[3] == positions[4] && positions[3] == positions[5];
            bool horizontalLine3 = positions[6] == positions[7] && positions[6] == positions[8];

            if (horizontalLine1 || horizontalLine2 || horizontalLine3)
                return true;
            else
                return false;
        }

        private bool VerticalLine()
        {
            bool verticalLine1 = positions[0] == positions[3] && positions[0] == positions[6];
            bool verticalLine2 = positions[1] == positions[4] && positions[1] == positions[7];
            bool verticalLine3 = positions[2] == positions[5] && positions[2] == positions[8];

            if (verticalLine1 || verticalLine2 || verticalLine3)
                return true;
            else
                return false;
        }

        private bool DiagonalLine()
        {
            bool diagonalLine1 = positions[0] == positions[4] && positions[0] == positions[8];
            bool diagonalLine2 = positions[2] == positions[4] && positions[2] == positions[6];

            if (diagonalLine1 || diagonalLine2)
                return true;
            else
                return false;
        }
        #endregion
    }
}
