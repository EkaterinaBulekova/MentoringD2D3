using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GameOfLife
{
    class Grid
    {

        private readonly int SizeX;
        private readonly int SizeY;
        private readonly Cell[,] cells;
        private readonly Cell[,] nextGenerationCells;
        private static Random rnd;
        private Canvas drawCanvas;
        private readonly Ellipse[,] cellsVisuals;

        
        public Grid(Canvas c)
        {
            drawCanvas = c;
            rnd = new Random();
            SizeX = (int) (c.Width / 5);
            SizeY = (int)(c.Height / 5);
            cells = new Cell[SizeX, SizeY];
            nextGenerationCells = new Cell[SizeX, SizeY];
            cellsVisuals = new Ellipse[SizeX, SizeY];
            drawCanvas.MouseDown += MouseMove;
            drawCanvas.MouseMove += MouseMove;
            // UNOPTIMIZED
            // for (int i = 0; i < SizeX; i++)
            //     for (int j = 0; j < SizeY; j++)
            //     {
            //         cells[i, j] = new Cell(i, j, 0, GetRandomBoolean());
            //         nextGenerationCells[i, j] = new Cell(i, j, 0, false);
            //     }
            //SetRandomPattern();

            InitGrid(); // OPTIMIZED

            //InitCellsVisuals();
            //UpdateGraphics();
            
        }



        public void Clear()
        {
            for (int i = 0; i < SizeX; i++)
                for (int j = 0; j < SizeY; j++)
                {
                    //cells[i, j] = new Cell(i, j, 0, false);                 // UNOPTIMIZED
                    //nextGenerationCells[i, j] = new Cell(i, j, 0, false);   // UNOPTIMIZED

                    cells[i, j].Clear();                                      // OPTIMIZED
                    nextGenerationCells[i, j].Clear();                        // OPTIMIZED
                    cellsVisuals[i, j].Fill = Brushes.Gray;
                }
        }



        private void MouseMove(object sender, MouseEventArgs e)
        {
            //var cellVisual = sender as Ellipse; //UNOPTIMIZED

            //int i = (int) cellVisual.Margin.Left / 5; //UNOPTIMIZED
            //int j = (int) cellVisual.Margin.Top / 5; //UNOPTIMIZED

            var position = e.GetPosition(sender as Canvas);
            int i = (int)position.X / 5;
            int j = (int)position.Y / 5;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!cells[i, j].IsAlive)
                {
                    cells[i, j].IsAlive = true;
                    cells[i, j].Age = 0;
                    cellsVisuals[i, j].Fill = Brushes.White;
                }
            }
        }

         private void InitGrid()
        {
            for (int i = 0; i < SizeX; i++)
                for (int j = 0; j < SizeY; j++)
                {
                    var isAlive = GetRandomBoolean();
                    cells[i, j] = new Cell(i, j, 0, isAlive);
                    nextGenerationCells[i, j] = new Cell(i, j, 0, false);
                    InitVisual(i, j, isAlive);
                    drawCanvas.Children.Add(cellsVisuals[i, j]);
                }
        }

        private void UpdateVisual(int row, int col, bool isAlive, int age)
        {
            cellsVisuals[row, col].Fill = isAlive
                       ? (age < 2
                             ? Brushes.White
                             : Brushes.DarkGray)
                       : Brushes.Gray;
        }

        private void InitVisual(int row, int col, bool isAlive)
        {
            cellsVisuals[row, col] = new Ellipse()
            {
                Width = 5,
                Height = 5,
                Margin = new Thickness(row * 5, col * 5, 0, 0),
                Fill = isAlive ? Brushes.White : Brushes.Gray,
            };
        }

        //UNOPTIMIZED
        //public void UpdateGraphics()
        //{
        //    for (int i = 0; i < SizeX; i++)
        //        for (int j = 0; j < SizeY; j++)
        //            cellsVisuals[i, j].Fill = cells[i, j].IsAlive
        //                                          ? (cells[i, j].Age < 2 ? Brushes.White : Brushes.DarkGray)
        //                                          : Brushes.Gray;
        //}

        //UNOPTIMIZED
        //public void InitCellsVisuals()
        //{
        //    for (int i = 0; i < SizeX; i++)
        //        for (int j = 0; j < SizeY; j++)
        //        {
        //            cellsVisuals[i, j] = new Ellipse();
        //            cellsVisuals[i, j].Width = cellsVisuals[i, j].Height = 5;
        //            double left = cells[i, j].PositionX;
        //            double top = cells[i, j].PositionY;
        //            cellsVisuals[i, j].Margin = new Thickness(left, top, 0, 0);
        //            cellsVisuals[i, j].Fill = Brushes.Gray;
        //            drawCanvas.Children.Add(cellsVisuals[i, j]);

        //            cellsVisuals[i, j].MouseMove += MouseMove;
        //            cellsVisuals[i, j].MouseLeftButtonDown += MouseMove;
        //         }
        //    UpdateGraphics();
        //}

        private static bool GetRandomBoolean()
        {
            return rnd.NextDouble() > 0.8;
        }

        //UNOPTIMIZED
        //public void SetRandomPattern()
        //{
        //    for (int i = 0; i < SizeX; i++)
        //        for (int j = 0; j < SizeY; j++)
        //            cells[i, j].IsAlive = GetRandomBoolean();
        //}

        private void UpdateToNextGeneration()
        {
            for (int i = 0; i < SizeX; i++)
                for (int j = 0; j < SizeY; j++)
                {
                    cells[i, j].IsAlive = nextGenerationCells[i, j].IsAlive;
                    cells[i, j].Age = nextGenerationCells[i, j].Age;
                }

//            UpdateGraphics();
        }


        public void Update()
        {
            bool alive = false;
            int age = 0;

            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {
//                    nextGenerationCells[i, j] = CalculateNextGeneration(i,j);          // UNOPTIMIZED
                    CalculateNextGeneration(i, j, ref alive, ref age);   // OPTIMIZED
                    nextGenerationCells[i, j].IsAlive = alive;  // OPTIMIZED
                    nextGenerationCells[i, j].Age = age;  // OPTIMIZED
                    UpdateVisual(i, j, alive, age); // OPTIMIZED
                }
            }
            UpdateToNextGeneration();
        }

        public Cell CalculateNextGeneration(int row, int column)    // UNOPTIMIZED
        {
            bool alive;
            int count, age;

            alive = cells[row, column].IsAlive;
            age = cells[row, column].Age;
            count = CountNeighbors(row, column);

            if (alive && count < 2)
                return new Cell(row, column, 0, false);
            
            if (alive && (count == 2 || count == 3))
            {
                cells[row, column].Age++;
                return new Cell(row, column, cells[row, column].Age, true);
            }

            if (alive && count > 3)
                return new Cell(row, column, 0, false);
            
            if (!alive && count == 3)
                return new Cell(row, column, 0, true);
            
            return new Cell(row, column, 0, false);
        }

        private void CalculateNextGeneration(int row, int column, ref bool isAlive, ref int age)     // OPTIMIZED
        {
            isAlive = cells[row, column].IsAlive;
            age = cells[row, column].Age;

            int count = CountNeighbors(row, column);

            if (isAlive && count < 2)
            {
                isAlive = false;
                age = 0;
            }

            if (isAlive && (count == 2 || count == 3))
            {
                cells[row, column].Age++;
                isAlive = true;
                age = cells[row, column].Age;
            }

            if (isAlive && count > 3)
            {
                isAlive = false;
                age = 0;
            }

            if (!isAlive && count == 3)
            {
                isAlive = true;
                age = 0;
            }
        }

        private int CountNeighbors(int i, int j)
        {
            int count = 0;

            if (i != SizeX - 1 && cells[i + 1, j].IsAlive) count++;
            if (i != SizeX - 1 && j != SizeY - 1 && cells[i + 1, j + 1].IsAlive) count++;
            if (j != SizeY - 1 && cells[i, j + 1].IsAlive) count++;
            if (i != 0 && j != SizeY - 1 && cells[i - 1, j + 1].IsAlive) count++;
            if (i != 0 && cells[i - 1, j].IsAlive) count++;
            if (i != 0 && j != 0 && cells[i - 1, j - 1].IsAlive) count++;
            if (j != 0 && cells[i, j - 1].IsAlive) count++;
            if (i != SizeX - 1 && j != 0 && cells[i + 1, j - 1].IsAlive) count++;

            return count;
        }
    }
}