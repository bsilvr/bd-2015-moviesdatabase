using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace Projecto_BD_WPF
{
    /// <summary>
    /// Interaction logic for SearchWriters.xaml
    /// </summary>
    public partial class SearchWriters : Page
    {
        SqlConnection cnn;
        SqlCommand cmd;
        public SearchWriters()
        {
            InitializeComponent();

            //ligação à base de dados
            cnn = Connection.getConnection();

            try
            {
                cnn.Open();
                MessageBox.Show("Sucessfull connection to database.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to open connection to database due to the following error:\n" + ex.Message);
            }

            LoadData("SELECT * from movies.udf_GetWriters()");
        }
        private void LoadData(string query)
        {
            SqlCommand command = new SqlCommand(query, cnn);
            SqlDataAdapter sda = new SqlDataAdapter(command);
            DataTable dt = new DataTable("Writers");
            sda.Fill(dt);
            search_result.ItemsSource = dt.DefaultView;
        }
        private void SearchData(SqlCommand command)
        {
            SqlDataAdapter sda = new SqlDataAdapter(command);
            DataTable dt = new DataTable("Writers");
            sda.Fill(dt);
            search_result.ItemsSource = dt.DefaultView;
        }

        private void search_Click(object sender, RoutedEventArgs e)
        {
            string searchWriters = "movies.sp_searchWriters";


            char[] d = { ':', '-', '/' };
            string wSSN = ssn.Text;
            string wName = name.Text;
            string wRank = rank.Text;

            string[] wBdate = birth_date.SelectedDate.Value.ToShortDateString().Split(d);

            cmd = new SqlCommand(searchWriters, cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            if (wSSN != "")
                cmd.Parameters.AddWithValue("@ssn", wSSN);
            if (wName != "")
                cmd.Parameters.AddWithValue("@name", wName);
            if (wBdate != null)
                cmd.Parameters.AddWithValue("@bdate", wBdate);
            if (wRank != "")
                cmd.Parameters.AddWithValue("@rank", wRank);

            try
            {
                SearchData(cmd);
            }
            catch
            {
                MessageBox.Show("Error on searching Writers in the database");
                return;
            }
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            ssn.Text = "";
            name.Text = "";
            birth_date.SelectedDate = null;
            rank.Text = "";
        }
    }
}
