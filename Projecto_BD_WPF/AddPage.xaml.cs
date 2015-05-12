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
using System.Windows.Shapes;

namespace Projecto_BD_WPF
{
    /// <summary>
    /// Interaction logic for AddPage.xaml
    /// </summary>
    public partial class AddPage : Page
    {
        public AddPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddMovies addMovie = new AddMovies();
            this.NavigationService.Navigate(addMovie);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AddActors addActor = new AddActors();
            this.NavigationService.Navigate(addActor);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            AddStudios addStudio = new AddStudios();
            this.NavigationService.Navigate(addStudio);
        }
    }
}
