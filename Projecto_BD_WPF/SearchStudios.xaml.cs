﻿using System;
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
    /// Interaction logic for SearchStudios.xaml
    /// </summary>
    public partial class SearchStudios : Page
    {
        SqlConnection cnn;
        SqlCommand cmd;
        public SearchStudios()
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

            LoadData("SELECT * from movies.udf_GetStudios()");

            string getLocationQuery = "SELECT location FROM movies.udf_location()";

            complete_combo_box(getLocationQuery, location);
        }
        private void LoadData(string query)
        {
            SqlCommand command = new SqlCommand(query, cnn);
            SqlDataAdapter sda = new SqlDataAdapter(command);
            DataTable dt = new DataTable("Studios");
            sda.Fill(dt);
            search_result.ItemsSource = dt.DefaultView;
        }
        private void SearchData(SqlCommand command)
        {
            SqlDataAdapter sda = new SqlDataAdapter(command);
            DataTable dt = new DataTable("Studios");
            sda.Fill(dt);
            search_result.ItemsSource = dt.DefaultView;
        }
        private void complete_combo_box(string query, ComboBox combobox)
        {
            SqlCommand command = new SqlCommand(query, cnn);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                int count = reader.FieldCount;
                while (reader.Read())
                {
                    string item = reader.GetValue(0).ToString();

                    for (int i = 1; i < count; i++)
                    {
                        item += " - " + reader.GetValue(i).ToString();
                    }

                    combobox.Items.Add(item);
                }
            }
        }
        private void search_Click(object sender, RoutedEventArgs e)
        {
            string searchStudios = "movies.sp_searchStudios";


            char[] d = { ':', '-', '/' };
            string sID = id.Text;
            string sName = name.Text;
            string sLocation = location.Text.ToString().Split(d)[0];


            cmd = new SqlCommand(searchStudios, cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            if (sID != "")
                cmd.Parameters.AddWithValue("@ssn", sID);
            if (sName != "")
                cmd.Parameters.AddWithValue("@name", sName);
            if (sLocation != null)
                cmd.Parameters.AddWithValue("@location", sLocation);


            try
            {
                SearchData(cmd);
            }
            catch
            {
                MessageBox.Show("Error on searching Studios in the database");
                return;
            }
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            id.Text = "";
            name.Text = "";
            location.Text = "";
        }
    }
}
