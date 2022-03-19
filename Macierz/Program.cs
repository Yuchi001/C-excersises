using System;
using System.IO;
using System.Collections.Generic;

namespace Macierz
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] matrix = GetMatrixFromFile("matrix.txt", out int xLenght, out int yLenght);
            RotateMatrix(xLenght, yLenght, matrix, 1);
            PrintMatrixIntoAFile(xLenght, yLenght, matrix);
        }
        static int[,] GetMatrixFromFile(string fileName, out int xLenght, out int yLenght)
        {
            List<int> values = new List<int>();
            StreamReader openFile = new StreamReader(fileName);
            string line;
            int linesNumber;
            for(linesNumber = 0; (line = openFile.ReadLine()) != null; linesNumber++)
            {
                string numberS = "";
                for(int i = 0; i <= line.Length; i++)
                {
                    if(i == line.Length)
                    {
                        values.Add(Int32.Parse(numberS));
                        continue;
                    }
                    if (line[i] == ' ')
                    {
                        values.Add(Int32.Parse(numberS));
                        numberS = "";
                        continue;
                    }
                    numberS += line[i];
                }
            }
            int yNumber = 0, xNumber = 0;
            int[,] matrix = new int[linesNumber, values.Count / linesNumber];
            foreach (int v in values)
            {
                if(yNumber % (values.Count / linesNumber) == 0 && yNumber != 0)
                {
                    yNumber = 0;
                    xNumber++;
                }
                matrix[xNumber, yNumber] = v;
                yNumber++;
            }
            openFile.Close();
            yLenght = values.Count / linesNumber;
            xLenght = linesNumber;
            return matrix;
        }
        static void PrintMatrixIntoAFile(int x, int y, int [,] m, string fileName = "rotatedMatrix.txt")
        {
            StreamWriter writeFile = new StreamWriter(fileName);
            for(int i = 0; i < x; i++)
            {
                for(int j =0; j < y; j++)
                {
                    writeFile.Write(m[i, j]);
                }
                writeFile.WriteLine();
            }
            writeFile.Close();
        }
        static void RotateMatrix(int maxLengthX, int maxLengthY, int[,] m, int moves = 1)
        {
            for(int mainIndex = 0; mainIndex < moves; mainIndex++)
            {
                int lengthX = maxLengthX;
                int lengthY = maxLengthY;
                int row = 0, col = 0;
                int previousValue, currentValue;
                while (row < lengthX && col < lengthY)
                {
                    if (row + 1 == lengthX || col + 1 == lengthY)
                        break;
                    previousValue = m[row + 1, col];
                    for (int i = col; i < lengthY; i++)
                    {
                        currentValue = m[row, i];
                        m[row, i] = previousValue;
                        previousValue = currentValue;
                    }
                    row++;
                    for (int i = row; i < lengthX; i++)
                    {
                        currentValue = m[i, lengthY - 1];
                        m[i, lengthY - 1] = previousValue;
                        previousValue = currentValue;
                    }
                    lengthY--;
                    if (row < lengthX)
                    {
                        for (int i = lengthY - 1; i >= col; i--)
                        {
                            currentValue = m[lengthX - 1, i];
                            m[lengthX - 1, i] = previousValue;
                            previousValue = currentValue;
                        }
                    }
                    lengthX--;
                    if (col < lengthY)
                    {
                        for (int i = lengthX - 1; i >= row; i--)
                        {
                            currentValue = m[i, col];
                            m[i, col] = previousValue;
                            previousValue = currentValue;
                        }
                    }
                    col++;
                }
            }
        }
    }
}
