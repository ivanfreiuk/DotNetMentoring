﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GameOfLife
{
    internal class Grid
    {
        private readonly int SizeX;
        private readonly int SizeY;
        private readonly Cell[,] cells;
        private readonly Cell[,] nextGenerationCells;
        private static Random random;
        private readonly Canvas drawCanvas;
        private readonly Ellipse[,] cellsVisuals;
                
        public Grid(Canvas canvas)
        {
            drawCanvas = canvas;
            random = new Random();
            SizeX = (int) (canvas.Width / 5);
            SizeY = (int)(canvas.Height / 5);
            cells = new Cell[SizeX, SizeY];
            nextGenerationCells = new Cell[SizeX, SizeY];
            cellsVisuals = new Ellipse[SizeX, SizeY];
 
            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {
                    cells[i, j] = new Cell(i, j, 0, false);
                    nextGenerationCells[i, j] = new Cell(i, j, 0, false);
                }
            }

            SetRandomPattern();
            InitCellsVisuals();
            UpdateGraphics();
            
        }

        public void Clear()
        {
            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {
                    cells[i, j].Age = 0;
                    cells[i, j].IsAlive = false;
                    nextGenerationCells[i, j].Age = 0;
                    nextGenerationCells[i, j].IsAlive = false;
                    cellsVisuals[i, j].Fill = Brushes.Gray;
                }
            }
        }

        void MouseMove(object sender, MouseEventArgs e)
        {
            var cellVisual = sender as Ellipse;
            
            int i = (int) cellVisual.Margin.Left / 5;
            int j = (int) cellVisual.Margin.Top / 5;
            

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!cells[i, j].IsAlive)
                {
                    cells[i, j].IsAlive = true;
                    cells[i, j].Age = 0;
                    cellVisual.Fill = Brushes.White;
                }
            }
        }

        public void UpdateGraphics()
        {
            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {
                    cellsVisuals[i, j].Fill = GetColor(cells[i, j]);
                }
            }
        }

        public void InitCellsVisuals()
        {
            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {
                    cellsVisuals[i, j] = new Ellipse();
                    cellsVisuals[i, j].Width = cellsVisuals[i, j].Height = 5;
                    double left = cells[i, j].PositionX;
                    double top = cells[i, j].PositionY;
                    cellsVisuals[i, j].Margin = new Thickness(left, top, 0, 0);
                    cellsVisuals[i, j].Fill = Brushes.Gray;
                    drawCanvas.Children.Add(cellsVisuals[i, j]);

                    cellsVisuals[i, j].MouseMove += MouseMove;
                    cellsVisuals[i, j].MouseLeftButtonDown += MouseMove;
                 }
            }

            UpdateGraphics();                    
        }
        

        public static bool GetRandomBoolean()
        {
            return random.NextDouble() > 0.8;
        }

        public void SetRandomPattern()
        {
            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {
                    cells[i, j].IsAlive = GetRandomBoolean();
                }
            }
        }
        
        public void UpdateToNextGeneration()
        {
            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {
                    cells[i, j].IsAlive = nextGenerationCells[i, j].IsAlive;
                    cells[i, j].Age = nextGenerationCells[i, j].Age;
                }
            }

            UpdateGraphics();
        }
        

        public void Update()
        {
            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {
                    CalculateNextGeneration(nextGenerationCells[i, j], i, j);
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

        public void CalculateNextGeneration(Cell cell, int row, int column)     // OPTIMIZED
        {
            int count = CountNeighbors(row, column);

            if (cell.IsAlive && count < 2)
            {
                cell.IsAlive = false;
                cell.Age = 0;
            }

            if (cell.IsAlive && (count == 2 || count == 3))
            {
                cell.Age++;
                cell.IsAlive = true;
            }

            if (cell.IsAlive && count > 3)
            {
                cell.IsAlive = false;
                cell.Age = 0;
            }

            if (!cell.IsAlive && count == 3)
            {
                cell.IsAlive = true;
                cell.Age = 0;
            }
        }

        public int CountNeighbors(int i, int j)
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

        private static SolidColorBrush GetColor(Cell cell)
        {
            return cell.IsAlive
                      ? (cell.Age < 2 ? Brushes.White : Brushes.DarkGray)
                      : Brushes.Gray;
        }
    }
}