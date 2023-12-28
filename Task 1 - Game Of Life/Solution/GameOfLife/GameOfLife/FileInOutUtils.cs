using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class FileInOutUtils
    {
        public static void SaveGameBoard(int rows, int columns, bool[,] board)
        {
            string fileName = "game.txt";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
                WriteToFile(rows, columns, board, fileName);
            }
            else
            {
                WriteToFile(rows, columns, board, fileName);
            }
        }

        private static void WriteToFile(int rows, int columns, bool[,] board, string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine(rows);
                writer.WriteLine(columns);

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        writer.Write(board[i, j] ? "X" : " ");
                    }
                    writer.WriteLine();
                }
            }
        }

        public static void ReadFromFile(out int rows, out int columns, out bool[,] board, string fileName)
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                rows = int.Parse(reader.ReadLine());
                columns = int.Parse(reader.ReadLine());
                board = new bool[rows, columns];

                for (int i = 0; i < rows; i++)
                {
                    string line = reader.ReadLine();
                    for (int j = 0; j < columns; j++)
                    {
                        char[] chars = line.ToCharArray();
                        if (chars[j] == 'X')
                        {
                            board[i, j] = true;
                        }
                        else
                        {
                            board[i, j] = false;
                        }
                    }
                }
            }
        }
    }
}
