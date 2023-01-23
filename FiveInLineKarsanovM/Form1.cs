using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace FiveInLineKarsanovM
{
    public partial class MainForm : Form
    {
        Image imgFreeCell, imgRedBall, imgGreenBall, imgYellowBall, imgPinkBall, imgBlueBall, imgLightBlueBall, imgBrownBall;
        const int SIZE = 9;
        int[,] field = new int[SIZE, SIZE];
        const int STARTBALLSCOUNT = 6;
        const int ADDEDBALLSAFTERMOVECOUNT = 3;
        const int BALLCOLORSCOUNT = 7;
        int transferedBall = 0;
        int oldCellX = -1;
        int oldCellY = -1;
        int score = 0;
        List<Color> ballColors = new List<Color>
        {
            Color.White,
            Color.Red,
            Color.Blue,
            Color.Green,
            Color.Yellow,
            Color.Brown,
            Color.Purple,
            Color.Pink
        };

        //enum BallType : int
        //{
        //    None = 0,
        //    Red = 1,
        //    Blue = 2,
        //    Green = 3,
        //    Yellow = 4,
        //    Brown = 5,
        //    LightBlue = 6,
        //    Pink = 7
        //}
        //BallType[,] field = new BallType[SIZE, SIZE];

        public MainForm()
        {
            InitializeComponent();
            imgFreeCell = Bitmap.FromFile("images/FreeCell.png");
            //File.WriteAllText("result.txt", "0");
            Field.Rows.Add(SIZE);
            //Field.CurrentCell.Style.ForeColor = Color.White;
            score = 0;
            labelScore.Text = score.ToString();
            AddNewBalls(STARTBALLSCOUNT);
            ShowField();
        }

        private void buttonRestart_Click(object sender, EventArgs e)
        {
            RestartGame();
        }

        bool IsGameOver()
        {
            var freeCellsCount = 0;
            for (var i = 0; i < field.GetLength(0); i++)
            {
                for (var j = 0; j < field.GetLength(1); j++)
                {
                    if (field[i, j] == 0)
                        freeCellsCount++;
                }
            }
            if (freeCellsCount < 3)
                return true;
            return false;
        }

        bool DeleteLines()
        {
            var wasDeleted = false;
            for (var i = 0; i < SIZE; i++)
            {
                for (var j = 0; j < SIZE; j++)
                {
                    if (field[i, j] != 0)
                    {
                        int k;
                        if (SIZE - j >= 5)
                        {
                            k = j;
                            while (k < SIZE && field[i, k] == field[i, j])
                            {
                                k++;
                            }
                            if (k - j >= 5)
                            {
                                wasDeleted = true;
                                for (var z = j; z < k; z++)
                                {
                                    field[i, z] = 0;
                                }
                                score += k - j;
                                labelScore.Text = score.ToString();
                            }
                        }
                        if (SIZE - i >= 5)
                        {
                            k = i;
                            while (k < SIZE && field[k, j] == field[i, j])
                            {
                                k++;
                            }
                            if (k - i >= 5)
                            {
                                wasDeleted = true;
                                for (var z = i; z < k; z++)
                                {
                                    field[z, j] = 0;
                                }
                                score += k - i;
                                labelScore.Text = score.ToString();
                            }
                        }
                    }
                }
            }
            return wasDeleted;
        }

        void ShowField()
        {
            for (var i = 0; i < SIZE; i++)
            {
                for (var j = 0; j < SIZE; j++)
                {
                    Field[j, i].Style.BackColor = ballColors[field[i, j]];
                }
            }
        }

        void AddNewBalls(int ballsCount)
        {
            var rnd = new Random();
            var x = rnd.Next(0, SIZE - 1);
            var y = rnd.Next(0, SIZE - 1);
            var involvedColores = new int[BALLCOLORSCOUNT];
            var ballColor = rnd.Next(0, BALLCOLORSCOUNT - 1);
            for (var i = 0; i < ballsCount; i++)
            {
                while (field[x, y] != 0)
                {
                    x = rnd.Next(0, SIZE);
                    y = rnd.Next(0, SIZE);
                }
                while (involvedColores[ballColor] != 0)
                {
                    ballColor = rnd.Next(0, BALLCOLORSCOUNT);
                }
                field[x, y] = ballColor + 1;
                involvedColores[ballColor] = 1;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //Field.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            //Field.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        }

        bool IsCorrectMove(int rowStart, int columnStart, int rowEnd, int columnEnd)
        {
            var wayMatrix = new int[SIZE, SIZE];
            for (var i = 0; i < SIZE; i++)
                for (var j = 0; j < SIZE; j++)
                    if (field[i, j] != 0)
                        wayMatrix[i, j] = -1;
            var counter = 1;
            wayMatrix[rowStart, columnStart] = counter;
            var hasCounter = true;
            while (hasCounter)
            {
                hasCounter = false;
                for (var i = 0; i < SIZE; i++)
                {
                    for (var j = 0; j < SIZE; j++)
                    {
                        if (wayMatrix[i, j] == counter)
                        {
                            if (i > 0 && wayMatrix[i - 1, j] == 0) wayMatrix[i - 1, j] = counter + 1;
                            if (i < SIZE - 1 && wayMatrix[i + 1, j] == 0) wayMatrix[i + 1, j] = counter + 1;
                            if (j > 0 && wayMatrix[i, j - 1] == 0) wayMatrix[i, j - 1] = counter + 1;
                            if (j < SIZE - 1 && wayMatrix[i, j + 1] == 0) wayMatrix[i, j + 1] = counter + 1;
                            hasCounter = true;
                        }
                    }
                }
                counter++;
            }
            if (wayMatrix[rowEnd, columnEnd] == 0)
                return false;
            return true;
        }

        void RestartGame()
        {
            for (var i = 0; i < SIZE; i++)
                for (var j = 0; j < SIZE; j++)
                    field[i, j] = 0;
            score = 0;
            labelScore.Text = score.ToString();
            AddNewBalls(STARTBALLSCOUNT);
            ShowField();
        }

        void RewriteRecord(int oldValue, int newValue)
        {
            if (oldValue < newValue)
            {
                File.WriteAllText("result.txt", newValue.ToString());
            }
        }

        private void Field_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var wasDeleted = false;
            if (field[e.RowIndex, e.ColumnIndex] != 0)// && transferedBall == 0)
            {
                transferedBall = field[e.RowIndex, e.ColumnIndex];
                oldCellX = e.ColumnIndex;
                oldCellY = e.RowIndex;
            }
            else if (field[e.RowIndex, e.ColumnIndex] == 0 && transferedBall != 0)
            {
                if (IsCorrectMove(oldCellY, oldCellX, e.RowIndex, e.ColumnIndex))
                {
                    field[e.RowIndex, e.ColumnIndex] = transferedBall;
                    field[oldCellY, oldCellX] = 0;
                    //oldCellY = oldCellX = -1;
                    //transferedBall = 0;
                    wasDeleted = DeleteLines();
                    if (!wasDeleted)
                    {
                        AddNewBalls(ADDEDBALLSAFTERMOVECOUNT);
                        DeleteLines();
                    }
                    ShowField();
                    if (IsGameOver())
                    {
                        RewriteRecord(Int32.Parse(File.ReadAllText("result.txt")), score);
                        MessageBox.Show("Твой счет: " + score.ToString() + "\nРекорд: " + File.ReadAllText("result.txt"));
                        RestartGame();
                    }
                }
                else
                {
                    MessageBox.Show("К полю нет пути");
                }
                oldCellY = oldCellX = -1;
                transferedBall = 0;
            }
        }
    }
}
