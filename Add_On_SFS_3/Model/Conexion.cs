﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Add_On_SFS_3.Model
{
     public class Conexion
    {
        public static DataSet Ejecutar_ds(string cmd)
        {
            string Cadecone = Settings.Default.CadeCone;
            SqlConnection con = new SqlConnection(Cadecone);
            con.Open();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd, con);
            da.Fill(ds);
            con.Close();
            return ds;

        }

        public static DataTable Ejecutar_dt(string cmd)
        {
            DataTable dt;
            string Cadecone = Settings.Default.CadeCone;
            SqlConnection con = new SqlConnection(Cadecone);
            con.Open();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd, con);
            da.Fill(ds);
            con.Close();
            dt = ds.Tables[0];
            return dt;

        }

        public static void EjecutarQuery(string cmd)
        {
            
            string Cadecone = Settings.Default.CadeCone;
            SqlConnection con = new SqlConnection(Cadecone);
            SqlCommand command = new SqlCommand(cmd, con);
            con.Open();
            command.ExecuteNonQuery();
            con.Close();
        }
    }
}
