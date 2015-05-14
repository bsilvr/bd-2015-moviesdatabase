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
    /// Interaction logic for SearchMovies.xaml
    /// </summary>
    public partial class SearchMovies : Page
    {
        SqlConnection cnn;
        public SearchMovies()
        {
            InitializeComponent();

            string connetionString = "Data Source=tcp: 193.136.175.33\\SQLSERVER2012,8293;Initial Catalog=p5g1;User ID=p5g1;Password=portugal";
            cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection ! ");
            }

            LoadData("SELECT * from movies.udf_GetMovies()");
        }

        private void LoadData(string query)
        {
            SqlCommand command = new SqlCommand(query, cnn);
            SqlDataAdapter sda = new SqlDataAdapter(command);
            DataTable dt = new DataTable("Movies");
            sda.Fill(dt);
            search_result.ItemsSource = dt.DefaultView;
        }
    }
}
