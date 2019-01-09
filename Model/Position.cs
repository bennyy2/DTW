using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Kinect;
using System.Windows.Media.Media3D;
using System.Data.OleDb;

namespace DTWTest.Model
{
    class Position
    {
        ConnectDB connectDB = new ConnectDB();
        OleDbConnection con;

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public int Joint { get; set; }
        public int Frame { get; set; }

        public Position() { }

        public Position(double x, double y, double z, int joint, int frame)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Joint = joint;
            this.Frame = frame;
        }

        public Point3D getPosition(JointType j, int poseName, int classRoom, int frame)
        {
            int s = (int)j;
            Point3D point = new Point3D();

            try
            {
                con = connectDB.connect();
                con.Open();
                OleDbCommand cmd = new OleDbCommand();
                String sqlQuery = "SELECT axis_x, axis_y, axis_z " +
                    "FROM JointPosition " +
                    "WHERE classID = @room " +
                    "AND poseID = @poseName " +
                    "AND frameNo = @frame " +
                    "AND jointID = @joint";
                cmd = new OleDbCommand(sqlQuery, con);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@room", classRoom);
                cmd.Parameters.AddWithValue("@poseName", poseName);
                cmd.Parameters.AddWithValue("@frame", frame);
                cmd.Parameters.AddWithValue("@joint", s);

                OleDbDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    point = new Point3D((double)reader["axis_x"], (double)reader["axis_y"], (double)reader["axis_z"]);
                   
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }
            return point;

        }

        public Point3D getPositionTrainee(JointType j, int poseName, int classRoom, int frame)
        {
            int s = (int)j;
            Point3D point = new Point3D();


            try
            {
                con = connectDB.connect();
                con.Open();
                OleDbCommand cmd = new OleDbCommand();
                String sqlQuery = "SELECT axis_x, axis_y, axis_z " +
                    "FROM TestJointPosition " +
                    "WHERE classID = @room " +
                    "AND poseID = @poseName " +
                    "AND frameNo = @frame " +
                    "AND jointID = @joint";
                cmd = new OleDbCommand(sqlQuery, con);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@room", classRoom);
                cmd.Parameters.AddWithValue("@poseName", poseName);
                cmd.Parameters.AddWithValue("@frame", frame);
                cmd.Parameters.AddWithValue("@joint", s);

                OleDbDataReader reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    point = new Point3D((double)reader["axis_x"], (double)reader["axis_y"], (double)reader["axis_z"]);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }
            return point;

        }

        public int lenghtFrame(int poseName, int classRoom)
        {
            int len = 0;
            try
            {
                con = connectDB.connect();
                con.Open();
                OleDbCommand cmd = new OleDbCommand();
                String sqlQuery = "SELECT MAX(frameNo) as maxFrame " +
                    "FROM JointPosition " +
                    "WHERE classID = @room " +
                    "AND poseID = @poseName ";
                cmd = new OleDbCommand(sqlQuery, con);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@room", classRoom);
                cmd.Parameters.AddWithValue("@poseName", poseName);

                OleDbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    len = (int)reader["maxFrame"];
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }

            return len;

        }

        public int lenghtFrameTrainee(int poseName, int classRoom)
        {
            int len = 0;
            try
            {
                con = connectDB.connect();
                con.Open();
                OleDbCommand cmd = new OleDbCommand();
                String sqlQuery = "SELECT MAX(frameNo) as maxFrame " +
                    "FROM TestJointPosition " +
                    "WHERE classID = @room " +
                    "AND poseID = @poseName ";
                cmd = new OleDbCommand(sqlQuery, con);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@room", classRoom);
                cmd.Parameters.AddWithValue("@poseName", poseName);

                OleDbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    len = (int)reader["maxFrame"];
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }

            return len;

        }

    }
}
