using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab1SGBF
{
    public partial class Interfata : Form
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionS"].ConnectionString);
        SqlDataAdapter dataAdapter = new SqlDataAdapter();
        SqlDataAdapter dataAdapterChild = new SqlDataAdapter();
        DataSet dataSet = new DataSet();
        DataSet dataSetChild = new DataSet();
        string id;
        string fid;
        List<Control> listControls = new List<Control>();
        public Interfata()
        {
            InitializeComponent();
            this.Text = ConfigurationManager.AppSettings["ParentTableName"];
            List<string> labels = ConfigurationManager.AppSettings["LabelNames"].Split(',').ToList();
            //numeLabel.Text = labels[0];
            //prenumeLabel.Text = labels[1];
            //salariuLabel.Text = labels[2];

            int firstIndex = 500;
            int secondIndex = 110;
            foreach (var l in labels)
            {
                Label label = new Label();
                label.Text = l;
                label.Location = new Point(firstIndex - 110, secondIndex);
                TextBox text = new TextBox();
                text.Width = 100;
                text.Location = new Point(firstIndex, secondIndex);
                this.Controls.Add(label);
                this.Controls.Add(text);
                listControls.Add(text);
                //firstIndex += 50;
                secondIndex += 25;
            }
            refreshButton_Click(null, null);


        }

        private void parinteGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            id=parinteGrid.SelectedRows[0].Cells[ConfigurationManager.AppSettings["ParentID"]].Value.ToString();
            dataAdapterChild.SelectCommand = new SqlCommand(ConfigurationManager.AppSettings["SelectFromChild"], connection);
            dataAdapterChild.SelectCommand.Parameters.AddWithValue("@id", id);
            dataSetChild.Clear();
            dataAdapterChild.Fill(dataSetChild);
            fiuGrid.DataSource = dataSetChild.Tables[0];
            
        } 
        private void fiuGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            fid = fiuGrid.SelectedRows[0].Cells[ConfigurationManager.AppSettings["ChildID"]].Value.ToString();
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            dataAdapter.SelectCommand = new SqlCommand(ConfigurationManager.AppSettings["SelectFromParent"], connection);
            dataSet.Clear();
            dataAdapter.Fill(dataSet);
            parinteGrid.DataSource = dataSet.Tables[0];
        }

        private void addButton_Click(object sender, EventArgs e)
        {

            try
            {
                dataAdapterChild.InsertCommand = new SqlCommand(ConfigurationManager.AppSettings["InsertQuery"],connection);
                string[] parameters = ConfigurationManager.AppSettings["ColumnNmesInsertParameters"].Split(',');

                for(int i=0;i<parameters.Count()-1;i++)
                {
                    dataAdapterChild.InsertCommand.Parameters.AddWithValue(parameters[i], listControls[i].Text.ToString());
                }
                dataAdapterChild.InsertCommand.Parameters.AddWithValue(parameters[parameters.Count()-1], id);
                connection.Open();
                dataAdapterChild.InsertCommand.ExecuteNonQuery();
                MessageBox.Show("A fost adaugat cu succes!");
                connection.Close();
                dataSetChild.Clear();
                dataAdapterChild.Fill(dataSetChild);
                fiuGrid.DataSource = dataSetChild.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare!");
                connection.Close();
            }
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            var nume = "";
            var prenume = "";
            var salariu = "";

            try
            {
                dataAdapterChild.UpdateCommand = new SqlCommand(ConfigurationManager.AppSettings["UpdateQuery"], connection);
                string[] parameters = ConfigurationManager.AppSettings["ColumnNmesUpdateParameters"].Split(',');
                for (int i = 0; i < parameters.Count() - 1; i++)
                {
                    dataAdapterChild.UpdateCommand.Parameters.AddWithValue(parameters[i], listControls[i].Text.ToString());
                }

                dataAdapterChild.UpdateCommand.Parameters.AddWithValue(parameters[parameters.Count()-1], fid);
                connection.Open();
                dataAdapterChild.UpdateCommand.ExecuteNonQuery();
                MessageBox.Show("A fost actualizat cu succes!");
                connection.Close();
                dataSetChild.Clear();
                dataAdapterChild.Fill(dataSetChild);
                fiuGrid.DataSource = dataSetChild.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare!");
                connection.Close();
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                dataAdapterChild.DeleteCommand = new SqlCommand(ConfigurationManager.AppSettings["DeleteQuery"], connection);
                dataAdapterChild.DeleteCommand.Parameters.AddWithValue(ConfigurationManager.AppSettings["ColumnNmesDeleteParameters"], fid);
                connection.Open();
                dataAdapterChild.DeleteCommand.ExecuteNonQuery();
                MessageBox.Show("A fost sters cu succes!");
                connection.Close();
                dataSetChild.Clear();
                dataAdapterChild.Fill(dataSetChild);
                fiuGrid.DataSource = dataSetChild.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare!");
                connection.Close();
            }
        }
    }
}
