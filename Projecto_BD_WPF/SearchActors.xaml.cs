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
    /// Interaction logic for SearchActors.xaml
    /// </summary>
    public partial class SearchActors : Page
    {
        SqlConnection cnn;
        SqlCommand cmd;
        public SearchActors()
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

            LoadData("SELECT * from movies.udf_GetActors()");


        }
        private void LoadData(string query)
        {
            SqlCommand command = new SqlCommand(query, cnn);
            SqlDataAdapter sda = new SqlDataAdapter(command);
            DataTable dt = new DataTable("Actors");
            sda.Fill(dt);
            search_result.ItemsSource = dt.DefaultView;
        }
        private void SearchData(SqlCommand command)
        {
            SqlDataAdapter sda = new SqlDataAdapter(command);
            DataTable dt = new DataTable("Actors");
            sda.Fill(dt);
            search_result.ItemsSource = dt.DefaultView;
        }
        private void search_Click(object sender, RoutedEventArgs e)
        {
            string searchActors = "movies.sp_searchActors";

            char[] d = { ':', '-', '/' };
            string actorSSN = ssn.Text;
            string actorName = name.Text;
            string actorRank = rank.Text;
            string actorBio = bio.Text;


            string[] actorBdate = birth_date.SelectedDate.Value.ToShortDateString().Split(d);

            cmd = new SqlCommand(searchActors, cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (actorSSN != "")
                cmd.Parameters.AddWithValue("@ssn", actorSSN);
            if (actorName != "")
                cmd.Parameters.AddWithValue("@name", actorName);
            if (actorBdate != null)
                cmd.Parameters.AddWithValue("@bdate", actorBdate);
            if (actorRank != "")
                cmd.Parameters.AddWithValue("@rank", actorRank);
            if (actorBio != "")
                cmd.Parameters.AddWithValue("@bio", actorBio);

            try
            {
                SearchData(cmd);
            }
            catch
            {
                MessageBox.Show("Error on searching Actors in the database");
                return;
            }
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            ssn.Text = "";
            name.Text = "";
            birth_date.Text = null;
            rank.Text = "";
            bio.Text = "";
        }
    }
}
