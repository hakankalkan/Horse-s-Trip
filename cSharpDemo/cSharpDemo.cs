using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cSharpDemo
{
    public partial class cSharpDemo : Form
    {
        public cSharpDemo()
        {

            InitializeComponent();

        }

        static int boardSize = 8;
        static int attemptedMoves = 0;
        static int[] xMove = {2, 1, -1, -2, -2, -1, 1, 2};
        static int[] yMove = {1, 2, 2, 1, -1, -2, -2, -1};
        static int[,] boardGrid = new int[boardSize, boardSize];
        static int startX;
        static int startY;
        
        
        private void CurrentCell_click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            startX = clickedButton.Location.X/50;
            startY = clickedButton.Location.Y/50;

            solveKT();
        }
        static void solveKT()
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    boardGrid[i, j] = -1;

                }
            }
            boardGrid[startX, startY] = 0;
            attemptedMoves = 0;

            if (!solveKTUtil(startX, startY, 1))
                Console.WriteLine("No solution for {0} an {1}", startX, startY);
            else
            {
                printBoardAsync(boardGrid);
               
                Console.WriteLine("Total attempted moves: {0}", attemptedMoves);
            }

            bool solveKTUtil(int x, int y, int moveCount)
            {
                attemptedMoves++;
                if (attemptedMoves % 1000000 == 0) Console.WriteLine("Attempted {0} moves", attemptedMoves);
                if (attemptedMoves >= 50000000)
                    {
                        MessageBox.Show("Program has terminated because" + " " + attemptedMoves.ToString() + " " + "attempts returned no result. Try another starting cell");
                        Environment.Exit(0);
                }
                int k;
                int next_x, next_y;

                if (moveCount == boardSize * boardSize)
                    return true;

                for (k = 0; k < 8; k++)
                {
                    next_x = x + xMove[k];
                    next_y = y + yMove[k];
                
                    if (safeSquare(next_x, next_y))
                    {
                        boardGrid[next_x, next_y] = moveCount;
                        if(solveKTUtil(next_x,next_y, moveCount + 1))
                            return true;
                        else
                            boardGrid[next_x, next_y] = -1;
                    }

                }

                return false;

            }
            bool safeSquare(int x, int y)
            {
                return (x >= 0 && x < boardSize && y >= 0 && y < boardSize && boardGrid[x, y] == -1);
            }
            async Task printBoardAsync(int[,] boardToPrint)
            {
                
                for (int i = 0; i < boardSize; i++)
                {
                    for (int j = 0; j < boardSize; j++)
                    {

                        
                        for (int k = 0; k < 64; k++)
                        {
                            if(cells[k].Location.X/50 == i && cells[k].Location.Y/50 == j)
                                cells[k].Name = boardGrid[i, j].ToString();
                        }
                    }
                }
                for (int i = 0; i<64; i++)
			    {
                    for (int j = 0; j < 64; j++)
                    {
                        if (i.ToString() == cells[j].Name)
                        {
                            await Task.Delay(500);
                            cells[j].BackColor = Color.Black;
                            cells[j].Text = cells[j].Name;
                        }
                    }
			    }

                MessageBox.Show("Horse's trip successfully ended after" + " " + attemptedMoves + " " + "attempted moves.");
                Environment.Exit(0);
            }
        }
        
        static Button[] cells = new Button[64];
        public void createChessTable()
        {
            int counter = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    cells[counter] = new Button()
                    {
                        Location = new Point(i * 50, j * 50),
                        Size = new Size(50, 50),
                        BackColor = Color.LightGray,
                        ForeColor = Color.White
                    };
                    this.Controls.Add(cells[counter]);
                    cells[counter].Click += CurrentCell_click;
                    counter++;
                }
            }
        }
        private void cSharpDemo_Load(object sender, EventArgs e)
        {
            createChessTable();
        }
    }
}

