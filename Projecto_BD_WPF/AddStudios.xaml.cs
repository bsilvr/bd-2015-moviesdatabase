using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace Projecto_BD_WPF
{
    /// <summary>
    /// Interaction logic for AddStudios.xaml
    /// </summary>
    public partial class AddStudios : Page
    {
        SqlConnection cnn;
        public AddStudios()
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
                return;
            }
        }
    }
}
