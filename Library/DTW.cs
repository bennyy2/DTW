using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Kinect;

namespace DTWTest.Library
{
    class DTW
    {
        CompareVector compare = new CompareVector();
        Model.Position position = new Model.Position();

        

        public Tuple<double[,], double[,]> modifiedDTW(JointType first, JointType sec, int poseRow, int roomRow, int poseCol, int roomCol, int rows, int columns)
        {
            //5 4
            double[,] table = new double[rows, columns];
            double[,] score = new double[rows, columns];

            for(int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Console.WriteLine(i+" "+j);

                    double cost = compare.compareScoreByJoint(first, sec, poseRow, roomRow, poseCol, roomCol , i, j);
                    if (i == 0 && j == 0)
                    {
                        table[i, j] = cost;
                        score[i, j] = cost;
                    }

                    else if (i == 0)
                    {
                        table[i, j] = cost + table[i, j - 1];
                        score[i, j] = cost;
                    }

                    else if (j == 0)
                    {
                        table[i, j] = cost + table[i - 1, j];
                        score[i, j] = cost;
                    }

                    else
                    {
                        table[i, j] = (cost + Math.Max(table[i - 1, j], Math.Max(table[i - 1, j - 1], table[i, j - 1])));
                        score[i, j] = cost;

                    }
                }
            }

            Tuple<double[,], double[,]> result = new Tuple<double[,], double[,]>(table, score);

            return result;
        }


        public List<Tuple<int, int, double, int>> wrapPath(double[,] table, double[,] score, int rows, int cols)
        {
            List<Tuple<int, int, double, int>> path = new List<Tuple<int, int, double, int>>();
            int round = 1;
            Tuple<int, int, double, int> tuple = new Tuple<int, int, double, int>(rows, cols, score[rows, cols], 1);
            path.Add(tuple);

            while (rows > 0 || cols> 0)
            {
                //Console.WriteLine("loop " + rows + " " + cols);
                tuple = check(table, score, rows, cols, round);
                path.Add(tuple);
                rows = tuple.Item1;
                cols = tuple.Item2;

            }
            
            return path;
        }

        private Tuple<int, int, double, int> check(double[,] table, double[,] score, int row, int col, int round)
        {

            Tuple<int, int, double, int> frames = new Tuple<int, int, double, int>(row, col, score[row, col], round);

            
            if (row == 0 && col > 0)
            {
                //Console.WriteLine("case1 " + row + " " + (col-1));
                frames = new Tuple<int, int, double, int>(row, col - 1, score[row, col - 1], round);
            }
            else if (row > 0 && col == 0)
            {
                //Console.WriteLine("case2 " + (row - 1) + " " + col);
                frames = new Tuple<int, int, double, int>(row - 1, col, score[row - 1, col], round);
            }
            else if (row > 0 && col > 0)
            {
                if(round == 1)
                {
                    if (table[row - 1, col] >= table[row - 1, col - 1] && table[row - 1, col] >= table[row, col - 1])
                    {
                        //Console.WriteLine("case41 " + (row-1) + " " + col);
                        round = 2;
                        frames = new Tuple<int, int, double, int>(row - 1, col, score[row - 1, col], round);
                    }
                    else if (table[row, col - 1] > table[row - 1, col - 1] && table[row, col - 1] > table[row - 1, col])
                    {
                        //Console.WriteLine("case42 " + row + " " + (col - 1));
                        frames = new Tuple<int, int, double, int>(row, col - 1, score[row, col - 1], round);
                    }
                    else
                    {
                        //Console.WriteLine("case43 " + (row - 1) + " " + (col - 1));
                        frames = new Tuple<int, int, double, int>(row - 1, col - 1, score[row - 1, col - 1], round);
                    }

                }
                else
                {
                    if (table[row, col - 1] > table[row - 1, col - 1] && table[row, col - 1] > table[row - 1, col])
                    {
                        //Console.WriteLine("case421 " + row + " " + (col - 1));
                        round = 1;
                        frames = new Tuple<int, int, double, int>(row, col - 1, score[row, col - 1], round);
                    }
                    if (table[row - 1, col] >= table[row - 1, col - 1] && table[row - 1, col] >= table[row, col - 1])
                    {
                        //Console.WriteLine("case422 " + (row - 1) + " " + col);
                        frames = new Tuple<int, int, double, int>(row - 1, col, score[row - 1, col], round);
                    }
                    else
                    {
                        //Console.WriteLine("case423 " + (row - 1) + " " + (col - 1));
                        frames = new Tuple<int, int, double, int>(row - 1, col - 1, score[row - 1, col - 1], round);
                    }
                }
            }
            
            return frames;
        }

    }
}
