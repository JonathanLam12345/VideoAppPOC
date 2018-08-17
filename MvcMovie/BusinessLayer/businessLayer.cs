using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MvcMovie {
    public class businessLayer {
        public void DeleteVideoInfo(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MvcMovie.Properties.Settings.VideoSetting"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DeleteStudentInfo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter executeID = new SqlParameter();
                executeID.ParameterName = "@id";
                executeID.Value = id;
                cmd.Parameters.Add(executeID);
                con.Open();
                cmd.ExecuteNonQuery();
            }
         
            }
        public void updateVideoUserTb(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MvcMovie.Properties.Settings.VideoSetting"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("deleteReference", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter executeId = new SqlParameter();
                executeId.ParameterName = "@id";
                executeId.Value = id;
                cmd.Parameters.Add(executeId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
       
            public int delete(int Videoid)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MvcMovie.Properties.Settings.VideoSetting"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("updateVideoUser", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter executeId = new SqlParameter();
                    executeId.ParameterName = "@id";
                    executeId.Value = Videoid;
                    cmd.Parameters.Add(executeId);
                    con.Open();
                 int modefied =cmd.ExecuteNonQuery();
                if (con.State == System.Data.ConnectionState.Open) con.Close();
                return modefied;
                }
            
        }
        }

    }
