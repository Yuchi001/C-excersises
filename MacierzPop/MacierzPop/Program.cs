using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace MacierzPop
{
    interface LoadData
    {
        public int[,] LoadMatrix(string fileName);
    }
    class LoadFromTxt : LoadData
    {
        public int[,] LoadMatrix(string fileName)
        {
            try
            {
                List<int[]> num = new List<int[]>();
                using StreamReader sr = new StreamReader(fileName);
                string line;
                for (int i = 0; (line = sr.ReadLine()) != null; i++)
                {
                    int[] numbers = Array.ConvertAll(line.Split(' '), s => int.Parse(s));
                    num.Add(numbers);
                }

                if (num.Count == 0)
                    return null;

                return LinqConvert(num.ToArray());
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                return null;
            }
        }
        int[,] LinqConvert(int[][] source)
        {
            return new[] { new int[source.Length, source[0].Length] }
                .Select(_ => new { x = _, y = source.Select((a, ia) => a.Select((b, ib) => _[ia, ib] = b).Count()).Count() })
                .Select(_ => _.x)
                .First();
        }
    }
    class Matrix
    {
        public int[,] matrix;
        public Matrix(int[,] m)
        {
            matrix = m;
        }
        public void PrintMatrix()
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int k = 0; k < matrix.GetLength(1); k++)
                {
                    Console.Write(matrix[i, k]);
                }
                Console.WriteLine();
            }
        }
        public void PrintMatrix(string fileName)
        {
            if(!fileName.EndsWith(".txt"))
            {
                Console.WriteLine("File could not be created:\nFile extention is not valid.");
            }
            using StreamWriter sw = new StreamWriter(fileName);
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int k = 0; k < matrix.GetLength(1); k++)
                {
                    sw.Write(matrix[i, k]);
                }
                sw.WriteLine();
            }
            Console.WriteLine($"Matrix succesfuly printed in {fileName}!");
        }
        public void RotateMatrix(int moves = 1)
        {
            int maxLengthX = matrix.GetLength(0);
            int maxLengthY = matrix.GetLength(1);
            for (int mainIndex = 0; mainIndex < moves; mainIndex++)
            {
                int lengthX = maxLengthX;
                int lengthY = maxLengthY;
                int row = 0, col = 0;
                int previousValue, currentValue;
                while (row < lengthX && col < lengthY)
                {
                    if (row + 1 == lengthX || col + 1 == lengthY)
                        break;
                    previousValue = matrix[row + 1, col];
                    for (int i = col; i < lengthY; i++)
                    {
                        currentValue = matrix[row, i];
                        matrix[row, i] = previousValue;
                        previousValue = currentValue;
                    }
                    row++;
                    for (int i = row; i < lengthX; i++)
                    {
                        currentValue = matrix[i, lengthY - 1];
                        matrix[i, lengthY - 1] = previousValue;
                        previousValue = currentValue;
                    }
                    lengthY--;
                    if (row < lengthX)
                    {
                        for (int i = lengthY - 1; i >= col; i--)
                        {
                            currentValue = matrix[lengthX - 1, i];
                            matrix[lengthX - 1, i] = previousValue;
                            previousValue = currentValue;
                        }
                    }
                    lengthX--;
                    if (col < lengthY)
                    {
                        for (int i = lengthX - 1; i >= row; i--)
                        {
                            currentValue = matrix[i, col];
                            matrix[i, col] = previousValue;
                            previousValue = currentValue;
                        }
                    }
                    col++;
                }
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            string fileName;
            Matrix matrix;
            Console.Write("File name (with extention!): ");
            fileName = Console.ReadLine();
            switch(CheckFileExtention(fileName))
            {
                case ".txt":
                    matrix = InitiateFunc(fileName, new LoadFromTxt());
                    break;
                default:
                    Console.WriteLine("This extention is not yet supported!");
                    return;
            }

            if (matrix.matrix == null)
                return;

            matrix.PrintMatrix();

            Console.Write("How many times to rotate: ");
            int times = int.Parse(Console.ReadLine());
            matrix.RotateMatrix(times);

            matrix.PrintMatrix();

            Console.Write("Print rotated matrix to a file?\nType file name to print, or -1 to skip this step. ");
            string printFileName = Console.ReadLine();
            if (printFileName == "-1")
                return;
            matrix.PrintMatrix("rotatedMatrix.txt");
        }
        static Matrix InitiateFunc<T>(string fileName, T loadObject) where T : LoadData
        {
            Matrix matrix = new Matrix(loadObject.LoadMatrix(fileName));
            return matrix;
        } 
        static string CheckFileExtention(string fileName)
        {
            int dotIndex = fileName.LastIndexOf('.');
            if (dotIndex == -1)
                return "";
            string extention = fileName.Substring(dotIndex);
            return extention;
        }
    }
}
