using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FiveInLineKarsanovM
{
    public partial class MainForm : Form
    {
        const int SIZE = 9;
        int[,] field = new int[SIZE, SIZE];
        const int STARTBALLSCOUNT = 7;
        const int ADDEDBALLSAFTERMOVECOUNT = 3;
        int transferedBall = 0;
        int oldCellX = -1;
        int oldCellY = -1;
        enum BallType : int
        {
            None = 0,
            Red = 1,
            Blue = 2,
            Green = 3,
            Yellow = 4,
            Brown = 5,
            Purple = 6,
            Pink = 7
        }
        //BallType[,] field = new BallType[SIZE, SIZE];

        public MainForm()
        {
            InitializeComponent();
            Field.ColumnHeadersVisible = false;
            Field.RowHeadersVisible = false;
            Field.Rows.Add(SIZE);
            //Field.CurrentCell.Style.ForeColor = Color.White;
            AddNewBalls(STARTBALLSCOUNT);
            ShowField();
            //Game();
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

        void ShowField()
        {
            for (var i = 0; i < SIZE; i++)
            {
                for (var j = 0; j < SIZE; j++)
                {
                    if (field[i, j] == 1) Field[j, i].Style.BackColor = Color.Red;
                    else
                        if (field[i, j] == 2) Field[j, i].Style.BackColor = Color.Blue;
                    else
                        if (field[i, j] == 3) Field[j, i].Style.BackColor = Color.Green;
                    else
                        if (field[i, j] == 4) Field[j, i].Style.BackColor = Color.Yellow;
                    else
                        if (field[i, j] == 5) Field[j, i].Style.BackColor = Color.Brown;
                    else
                        if (field[i, j] == 6) Field[j, i].Style.BackColor = Color.Purple;
                    else
                        if (field[i, j] == 7) Field[j, i].Style.BackColor = Color.Pink;
                    else
                        if (field[i, j] == 0) Field[j, i].Style.BackColor = Color.White;
                }
            }
        }

        void AddNewBalls(int ballsCount)
        {
            var rnd = new Random();
            var x = rnd.Next(0, SIZE - 1);
            var y = rnd.Next(0, SIZE - 1);
            var involvedColores = new int[7];
            var ballColor = rnd.Next(0, involvedColores.Length - 1);
            for (var i = 0; i < ballsCount; i++)
            {
                while (field[x, y] != 0)
                {
                    x = rnd.Next(0, SIZE);
                    y = rnd.Next(0, SIZE);
                }
                while (involvedColores[ballColor] != 0)
                {
                    ballColor = rnd.Next(0, involvedColores.Length);
                }
                field[x, y] = ballColor + 1;
                involvedColores[ballColor] = 1;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

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

        private void Field_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //var transferedBall = 0;
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
                    AddNewBalls(3);
                    ShowField();
                    if (IsGameOver())
                        MessageBox.Show("Твой счет:");
                }
                else
                {
                    MessageBox.Show("К полю нет пути");
                }
                oldCellY = oldCellX = -1;
                transferedBall = 0;
            }
            //else if (field[e.RowIndex, e.ColumnIndex] != 0 && transferedBall != 0)
            //{
            //    MessageBox.Show("Поле уже занято!");
            //    oldCellY = oldCellX = -1;
            //    transferedBall = 0;
            //}
        }
    }
}
