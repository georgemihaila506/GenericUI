using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Data;

namespace Lab1SGBF
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionS"].ConnectionString);
        static SqlConnection connection2 = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionS"].ConnectionString);
        static void Main()
        {
            /*
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            SqlDataAdapter dataAdapterChild = new SqlDataAdapter();
            DataSet dataSet = new DataSet();
            DataSet dataSetChild = new DataSet();
            dataAdapter.SelectCommand = new SqlCommand("exec UpdateDSponsori", connection);
            dataAdapterChild.SelectCommand = new SqlCommand("exec UpdateD2Sponsori", connection);

            dataSet.Clear();
            dataAdapter.Fill(dataSet);

            dataSetChild.Clear();
            dataAdapterChild.Fill(dataSetChild);
            */
           
                Thread firstOp = new Thread(Program.doFirstWok);
                Thread secondOp = new Thread(Program.doSecondWork);
            firstOp.Start();
            secondOp.Start();
                //doFirstStuff(firstOp,10);
            //doSecondStuff(secondOp, 10);
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Interfata());
        }
        static void doFirstStuff(Thread firstOp,int x)
        {
            if (x == 0)
                return;
            try
            {
                firstOp.Start();
                //secondOp.Start();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Prima");
                doFirstStuff(firstOp, --x);   
            }
        }
        static void doSecondStuff(Thread secondOp, int x)
        {
            if (x == 0)
                return;
            try
            {
                secondOp.Start();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("second");
                doFirstStuff(secondOp, --x);
            }
        }
        static void doFirstWok(Object data)
        {
            try
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                DataSet dataSet = new DataSet();
                dataAdapter.SelectCommand = new SqlCommand("exec UpdateDSponsori", connection);
                dataSet.Clear();
                dataAdapter.Fill(dataSet);
                Console.WriteLine("Primul s a terminat!");

            }
            catch (SqlException ex)
            {
                doFirstWok(null);
            }

        }
        static void doSecondWork(Object data)
        {
            try
            {
                SqlDataAdapter dataAdapterChild = new SqlDataAdapter();
                DataSet dataSetChild = new DataSet();
                dataAdapterChild.SelectCommand = new SqlCommand("exec UpdateD2Sponsori", connection2);
                dataSetChild.Clear();
                dataAdapterChild.Fill(dataSetChild);
                Console.WriteLine("Al doilea s a terminat!");

            }
            catch (SqlException ex)
            {
                doSecondWork(null);
            }
        }
    }
}
