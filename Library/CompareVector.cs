using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Kinect;
using System.Windows.Media.Media3D;

namespace DTWTest.Library
{
    class CompareVector
    {
        Model.Position position = new Model.Position();

        List<JointType> legLeft = new List<JointType> { JointType.HipCenter, JointType.HipLeft,
            JointType.KneeLeft, JointType.AnkleLeft, JointType.FootLeft};

        List<JointType> legRight = new List<JointType> { JointType.HipCenter, JointType.HipRight,
            JointType.KneeRight, JointType.AnkleRight, JointType.FootRight};

        List<JointType> handLeft = new List<JointType> { JointType.HipCenter, JointType.Spine, JointType.ShoulderCenter,
            JointType.ShoulderLeft, JointType.ElbowLeft, JointType.WristLeft, JointType.HandLeft};

        List<JointType> handRight = new List<JointType> { JointType.HipCenter, JointType.Spine, JointType.ShoulderCenter,
            JointType.ShoulderRight, JointType.ElbowRight, JointType.WristRight, JointType.HandRight };

        private Vector3D getVector(Point3D start, Point3D end)
        {
            Vector3D vector3D = new Vector3D();
            vector3D.X = end.X - start.X;
            vector3D.Y = end.Y - start.Y;
            vector3D.Z = end.Z - start.Z;

            return vector3D;
        }

        private Vector3D normalize(Vector3D vector3D)
        {
            vector3D.Normalize();

            return vector3D;
        }

        private double similarityScore(Vector3D trainer, Vector3D trainee)
        {
            double score;

            score = (trainee.X * trainer.X) + (trainee.Y * trainer.Y) + (trainee.Z * trainer.Z);

            return score;
        }

        private double distance(Point3D trainer, Point3D trainee)
        {
            double cost = 0;

            cost += Math.Pow((trainee.X - trainer.X), 2);
            cost += Math.Pow((trainee.Y - trainer.Y), 2);
            cost += Math.Pow((trainee.Z - trainer.Z), 2);

            return (double)Math.Sqrt(cost);
        }

        //public double compareScoreByJoint(Skeleton s, string pose, string room, int frame)        
        public double compareScoreByJoint(JointType first, JointType sec, int poseRow, int roomRow, int poseCol, int roomCol, int frameTrainee, int frameTrainer)
        {
            double score = 0;
            double totalscore = 0;
            List<JointType> joint = new List<JointType> { first , sec };

            List<List<JointType>> listsJoint = new List<List<JointType>> { legLeft, legRight, handLeft, handRight };

            for(int i = 0; i < joint.Count-1; i++)
            {
                
                    //trainee input
                    Point3D start = position.getPositionTrainee(joint[i], poseRow, roomRow, frameTrainee);
                    Point3D end = position.getPositionTrainee(joint[i+1], poseRow, roomRow, frameTrainee);
                    Vector3D trainee = getVector(start, end);
                    Vector3D traineeN = normalize(trainee);


                    //trainer template
                    Point3D startPoint = position.getPositionTrainee(joint[i], poseCol, roomCol, frameTrainer);
                    Point3D endPoint = position.getPositionTrainee(joint[i + 1], poseCol, roomCol, frameTrainer);
                    Vector3D trainer = getVector(startPoint, endPoint);
                    Vector3D trainerN = normalize(trainer);
                    //compare
                    score = similarityScore(trainerN, traineeN);
                    Console.WriteLine(joint[i].ToString() + " to " + joint[i + 1].ToString() + " : " + score);

                    totalscore += score;
                

            }
            
            
            return totalscore;
        }

    }
}
