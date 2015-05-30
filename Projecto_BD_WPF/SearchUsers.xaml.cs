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
    /// Interaction logic for SearchUsers.xaml
    /// </summary>
    public partial class SearchUsers : Page
    {
        SqlConnection cnn;
        SqlCommand cmd;
        public SearchUsers()
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

            LoadData("SELECT * from movies.udf_GetUsers()");

        }
        private void LoadData(string query)
        {
            SqlCommand command = new SqlCommand(query, cnn);
            SqlDataAdapter sda = new SqlDataAdapter(command);
            DataTable dt = new DataTable("Users");
            sda.Fill(dt);
            search_result.ItemsSource = dt.DefaultView;
        }
        private void SearchData(SqlCommand command)
        {
            SqlDataAdapter sda = new SqlDataAdapter(command);
            DataTable dt = new DataTable("Users");
            sda.Fill(dt);
            search_result.ItemsSource = dt.DefaultView;
        }
        private void search_Click(object sender, RoutedEventArgs e)
        {
            string searchUsers = "movies.sp_searchUsers";

            char[] d = { ':', '-', '/' };
            string uUsername = username.Text;
            string uName = name.Text;
            string uEmail = email.Text;
            string uCountry = country.Text;

            string[] uBdate = birth_date.SelectedDate.Value.ToShortDateString().Split(d);

            cmd = new SqlCommand(searchUsers, cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            if (uUsername != "")
                cmd.Parameters.AddWithValue("@username", uUsername);
            if (uName != "")
                cmd.Parameters.AddWithValue("@name", uName);
            if (uBdate != null)
                cmd.Parameters.AddWithValue("@bdate", uBdate);
            if (uEmail != "")
                cmd.Parameters.AddWithValue("@email", uEmail);
            if (uCountry != "")
                cmd.Parameters.AddWithValue("@rank", uCountry);

            try
            {
                SearchData(cmd);
            }
            catch
            {
                MessageBox.Show("Error on searching Users in the database");
                return;
            }
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            username.Text = "";
            name.Text = "";
            birth_date.SelectedDate = null;
            email.Text = "";
            country.Text = "";
        }
    }
}

