using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using System.Windows.Media.Media3D;
using Microsoft.Kinect;

namespace DTWTest
{
    class Program
    {
        static void Main(string[] args)
        {

            Library.DTW dtw = new Library.DTW();
            Model.Position position = new Model.Position();
            Library.CompareVector compareVector = new Library.CompareVector();

            int rows = position.lenghtFrameTrainee(60, 22); 
            int columns = position.lenghtFrameTrainee(60, 22);//trainee

            rows += 1;
            columns += 1;

            Console.WriteLine("row " + rows);
            Console.WriteLine("col " + columns);



            //trainer 76
            //List<int> rows = new List<int> { 71, 71, 71, 73, 73, 73, 74, 74, 74, 75, 75, 75 };

            ////trainee 224
            //List<int> columns = new List<int> { 71, 73, 74, 75 };
            
            List<Tuple<double[,], double[,]>> result1 = new List<Tuple<double[,], double[,]>>();
            List<Tuple<double[,], double[,]>> result2 = new List<Tuple<double[,], double[,]>>();
            List<Tuple<double[,], double[,]>> result3 = new List<Tuple<double[,], double[,]>>();
            List<Tuple<double[,], double[,]>> result4 = new List<Tuple<double[,], double[,]>>();

            List<JointType> legLeft = new List<JointType> { JointType.HipCenter, JointType.HipLeft,
            JointType.KneeLeft, JointType.AnkleLeft, JointType.FootLeft};

            List<JointType> legRight = new List<JointType> { JointType.HipCenter, JointType.HipRight,
            JointType.KneeRight, JointType.AnkleRight, JointType.FootRight};

            List<JointType> handLeft = new List<JointType> { JointType.HipCenter, JointType.Spine, JointType.ShoulderCenter,
            JointType.ShoulderLeft, JointType.ElbowLeft, JointType.WristLeft, JointType.HandLeft};

            List<JointType> handRight = new List<JointType> { JointType.HipCenter, JointType.Spine, JointType.ShoulderCenter,
            JointType.ShoulderRight, JointType.ElbowRight, JointType.WristRight, JointType.HandRight };


            List<List<JointType>> listsJoint = new List<List<JointType>> { legLeft, legRight, handLeft, handRight };

            


            Task thread1 = Task.Factory.StartNew(() => {
                //Some work...
                result1 = task1(legLeft, rows, columns);
            });

            Task thread2 = Task.Factory.StartNew(() => {
                //Some work...
                result2 = task1(legRight, rows, columns);
            });

            Task thread3 = Task.Factory.StartNew(() => {
                //Some work...
                result3 = task1(handLeft, rows, columns);
            });

            Task thread4 = Task.Factory.StartNew(() => {
                //Some work...
                result4 = task1(handRight, rows, columns);
            });



            Task.WaitAll(thread1, thread2, thread3, thread4);

            List<List<Tuple<double[,], double[,]>>> result = new List<List<Tuple<double[,], double[,]>>>
            { result1, result2, result3, result4};

            writeConsole(result, rows, columns);


            Console.ReadKey();
        }



        public static List<Tuple<double[,], double[,]>> task1(List<JointType> joint, int rows, int columns)
        {

            Library.DTW dtw = new Library.DTW();

            List<Tuple<double[,], double[,]>> result = new List<Tuple<double[,], double[,]>>();
           
            for (int j = 0; j < joint.Count - 1; j++)
                {
                    Tuple<double[,], double[,]> a = dtw.modifiedDTW(joint[j], joint[j + 1], 60, 22, 60, 22, rows, columns);

                    result.Add(a);
                }

            return result;
        }


        public static void writeConsole(List<List<Tuple<double[,], double[,]>>> result, int rows, int columns)
        {

            Library.DTW dtw = new Library.DTW();

            double[,] table = new double[rows, columns];
            double[,] score = new double[rows, columns];

            foreach (List<Tuple<double[,], double[,]>> i in result)
            {
                foreach (Tuple<double[,], double[,]> j in i)
                {
                    for (int r = 0; r < rows; r++)
                    {
                        for (int c = 0; c < columns; c++)
                        {
                            table[r, c] += j.Item1[r, c];
                            score[r, c] += j.Item2[r, c];
                        }
                    }
                }
            }


            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    table[r, c] /= 20;
                    score[r, c] /= 20;
                }
            }


            //Get DTW frame 0-3 
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Console.WriteLine("Dtw [" + i + "]" + "[" + j + "] = " + table[i, j]);
                }
            }


            //wrap path
            List<Tuple<int, int, double, int>> wrapPath = dtw.wrapPath(table, score, Convert.ToInt32(rows) - 1, Convert.ToInt32(columns) - 1);

            double sum = 0;

            List<double> minscore = new List<double>();

            foreach (Tuple<int, int, double, int> i in wrapPath)
            {
                sum += i.Item3;
                minscore.Add(i.Item3);
                Console.WriteLine(i.Item1 + " " + i.Item2 + " " + i.Item3);
            }

            Console.WriteLine("Score : " + sum / wrapPath.Count);

            double min = minscore.Min();
            int index = minscore.IndexOf(min);

            Console.WriteLine(wrapPath[index].Item1 + " " + wrapPath[index].Item2 + " " + wrapPath[index].Item3);

            Console.ReadKey();
        }

    }
}
