using System;
using System.Collections.Generic;
using System.Data;
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
using System.Configuration;
using System.Collections;
using System.Text.RegularExpressions;

namespace oop8
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
        private int selectedIndex;

        private void ShowData(string query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGrid.ItemsSource = dt.DefaultView;
            }
        }

        private void ShowAuthorsData(string query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridAuthors.ItemsSource = dt.DefaultView;
            }
        }

        public void ConnectToDatabase()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Database connection error: " + ex.Message);
                }

            }
        }

        private List<string> GetAuthorNames()
        {
            string query = "SELECT ID, Name, Surname FROM authors";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                List<string> authorNames = new List<string>();

                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    string surname = reader.GetString(2);

                    string authorFullName = $"{id}: {name} {surname}";

                    authorNames.Add(authorFullName);
                }

                return authorNames;
            }
        }
        private List<int> GetAllBookID()
        {
            string query = "SELECT ID FROM books";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                List<int> bookids = new List<int>();

                while (reader.Read())
                {
                    int name = reader.GetInt32(0);
                    bookids.Add(name);
                }

                return bookids;
            }
        }

        private List<int> GetAllAuthorsID()
        {
            string query = "SELECT ID FROM authors";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                List<int> authorids = new List<int>();

                while (reader.Read())
                {
                    int name = reader.GetInt32(0);
                    authorids.Add(name);
                }

                return authorids;
            }
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            GetAllAuthorsInComboBox();
        }

        private void GetAllAuthorsInComboBox()
        {
            List<string> authorNames = GetAuthorNames();
            comboBoxAuthors.ItemsSource = authorNames;
        }

        public MainWindow()
        {
            InitializeComponent();
            ConnectToDatabase();
            ShowData("SELECT * FROM BOOKS");
            ShowAuthorsData("SELECT * FROM AUTHORS");
        }

        private void isSaveEnabled()
        {
            if(textBoxID.Text.Length > 0
                &&
                textBoxName.Text.Length > 0
                &&
                textBoxPath.Text.Length > 0
                &&
                textBoxDescription.Text.Length > 0
                &&
                comboBoxAuthors.SelectedItem != null)
            {
                addBook.IsEnabled = true;
            } else
            {
                addBook.IsEnabled = false;

            }
        }

        private void isSaveAuthorEnabled()
        {
            if (textBoxAuthorID.Text.Length > 0
                &&
                textBoxAuthorName.Text.Length > 0
                &&
                textBoxSurname.Text.Length > 0
                &&
                textBoxBiography.Text.Length > 0)
            {
                addAuthor.IsEnabled = true;
            }
            else
            {
                addAuthor.IsEnabled = false;

            }
        }

        private void textBoxID_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox avgGrade = (TextBox)sender;
            for (int i = 0; i < avgGrade.Text.Length; i++)
            {
                if (!char.IsDigit(avgGrade.Text[i]))
                {
                    avgGrade.Text = avgGrade.Text.Remove(i, 1);
                    avgGrade.SelectionStart = avgGrade.Text.Length;
                }
            }
            isSaveEnabled();
            isSaveAuthorEnabled();
        }

        private void textBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            isSaveEnabled();
        }

        private void textBoxPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            isSaveEnabled();
        }

        private void textBoxDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            isSaveEnabled();
        }

        private void comboBoxAuthors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isSaveEnabled();
        }

        private void addBook_Click(object sender, RoutedEventArgs e)
        {
            string idString = textBoxID.Text;
            int id = Convert.ToInt32(idString);
            if (GetAllBookID().Contains(id))
            {
                MessageBox.Show("this id already taken. please try another one.");
                return;
            }
            string name = textBoxName.Text;
            string fullAuthorName = comboBoxAuthors.SelectedItem.ToString();
            int indexOfColon = fullAuthorName.IndexOf(':');
            string author = fullAuthorName.Substring(0, indexOfColon);
            string path = textBoxPath.Text;
            string description = textBoxDescription.Text;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("AddBook", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@Bookid", Convert.ToInt32(id)));
                command.Parameters.Add(new SqlParameter("@bookname", name));
                command.Parameters.Add(new SqlParameter("@authorid", Convert.ToInt32(author)));
                command.Parameters.Add(new SqlParameter("@PICTUREPATH", path));
                command.Parameters.Add(new SqlParameter("@description", description));

                connection.Open();
                command.ExecuteNonQuery();
            }
            ShowData("SELECT * FROM BOOKS");
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                selectedIndex = dataGrid.SelectedIndex;
                backButton.IsEnabled = true;
                furtherButton.IsEnabled = true;
                DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;
                textBoxID.Text = selectedRow.Row["ID"].ToString();
                textBoxName.Text = selectedRow.Row["Name"].ToString();
                comboBoxAuthors.SelectedIndex = Convert.ToInt32(selectedRow.Row["Author"].ToString()) - 1; 
                textBoxPath.Text = selectedRow.Row["picture_path"].ToString();
                textBoxDescription.Text = selectedRow.Row["description"].ToString();
            } else
            {
                selectedIndex = -1;
                backButton.IsEnabled = false;
                furtherButton.IsEnabled = false;
            }
        }

        private void deleteBook_Click(object sender, RoutedEventArgs e)
        {
            string idString = textBoxID.Text;
            if (idString == "")
                MessageBox.Show("Введите ID");
            else
            {
                int id = Convert.ToInt32(idString);
                if (!GetAllBookID().Contains(id))
                {
                    MessageBox.Show("this ID doesnt exist. please try another one.");
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            SqlCommand command = new SqlCommand("deletebook", connection, transaction);
                            command.CommandType = CommandType.StoredProcedure;

                            command.Parameters.Add(new SqlParameter("@Bookid", id));

                            command.ExecuteNonQuery();


                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
                ShowData("SELECT * FROM BOOKS");
            }
        }

        private void updateBook_Click(object sender, RoutedEventArgs e)
        {
            
            string idString = textBoxID.Text;
            if (idString == "")
                MessageBox.Show("Введите ID");
            else
            {
                int id = Convert.ToInt32(idString);
                if (!GetAllBookID().Contains(id))
                {
                    MessageBox.Show("this ID doesnt exist. please try another one.");
                    return;
                }
                string name = textBoxName.Text;
                string fullAuthorName = comboBoxAuthors.SelectedItem.ToString();
                int indexOfColon = fullAuthorName.IndexOf(':');
                string author = fullAuthorName.Substring(0, indexOfColon);
                string path = textBoxPath.Text;
                string description = textBoxDescription.Text;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand("UPDATEBOOK", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Bookid", Convert.ToInt32(id)));
                    command.Parameters.Add(new SqlParameter("@bookname", name));
                    command.Parameters.Add(new SqlParameter("@authorid", Convert.ToInt32(author)));
                    command.Parameters.Add(new SqlParameter("@PICTUREPATH", path));
                    command.Parameters.Add(new SqlParameter("@description", description));

                    connection.Open();
                    command.ExecuteNonQuery();
                }
                ShowData("SELECT * FROM BOOKS");
            }
        }

        private void sortBooks_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            ShowData("SELECT * FROM BOOKS ORDER BY NAME " + button.Content);
        }

        private void showAll_Click(object sender, RoutedEventArgs e)
        {
            ShowData("SELECT * FROM BOOKS");
        }

        private void showBook2_Click(object sender, RoutedEventArgs e)
        {
            ShowData("SELECT * FROM BOOKS WHERE ID > 2");
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            int rowCount = dataGrid.Items.Count;
            if (selectedIndex == 0)
            {
                dataGrid.SelectedIndex = rowCount - 2;
            }
            else
            {
                dataGrid.SelectedIndex = selectedIndex - 1;
            }
        }

        private void furtherButton_Click(object sender, RoutedEventArgs e)
        {
            int rowCount = dataGrid.Items.Count;
            if (selectedIndex == rowCount - 2)
            {
                dataGrid.SelectedIndex = 0;
            } else
            {
                dataGrid.SelectedIndex = selectedIndex + 1;
            }
        }

        private void clearALl_Click(object sender, RoutedEventArgs e)
        {
            textBoxID.Text = "";
            textBoxName.Text = "";
            textBoxPath.Text = "";
            comboBoxAuthors.SelectedIndex = -1;
            textBoxDescription.Text = "";
        }

        private void textBoxAuthorName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox avgGrade = (TextBox)sender;
            
            for (int i = 0; i < avgGrade.Text.Length; i++)
            {
                if (!char.IsLetter(avgGrade.Text[i]) && avgGrade.Text[i] != '-')
                {
                    avgGrade.Text = avgGrade.Text.Remove(i, 1);
                    avgGrade.SelectionStart = avgGrade.Text.Length;
                }
            }
            isSaveAuthorEnabled();
        }

        private void textBoxSurname_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox avgGrade = (TextBox)sender;
            for (int i = 0; i < avgGrade.Text.Length; i++)
            {
                if (!char.IsLetter(avgGrade.Text[i]) && avgGrade.Text[i] != '-')
                {
                    avgGrade.Text = avgGrade.Text.Remove(i, 1);
                    avgGrade.SelectionStart = avgGrade.Text.Length;
                }
            }
            isSaveAuthorEnabled();
        }

        private void textBoxBiography_TextChanged(object sender, TextChangedEventArgs e)
        {
            isSaveAuthorEnabled();
        }

        private void addAuthor_Click(object sender, RoutedEventArgs e)
        {
            string idString = textBoxAuthorID.Text;
            int id = Convert.ToInt32(idString);
            if (GetAllAuthorsID().Contains(id))
            {
                MessageBox.Show("this id already taken. please try another one.");
                return;
            }
                string name = textBoxAuthorName.Text;
                string surname = textBoxSurname.Text;
                string biography = textBoxBiography.Text;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("AddAuthor", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@AuthorID", Convert.ToInt32(id)));
                command.Parameters.Add(new SqlParameter("@authorname", name));
                command.Parameters.Add(new SqlParameter("@authorsurname", surname));
                command.Parameters.Add(new SqlParameter("@biography", biography));

                connection.Open();
                command.ExecuteNonQuery();
            }
            ShowAuthorsData("SELECT * FROM AUTHORS");
            GetAllAuthorsInComboBox();
        }

        private void updateAuthor_Click(object sender, RoutedEventArgs e)
        {
            string idString = textBoxAuthorID.Text;
            if (idString == "")
                MessageBox.Show("Введите ID");
            else
            {
                int id = Convert.ToInt32(idString);
                if (!GetAllAuthorsID().Contains(id))
                {
                    MessageBox.Show("this ID doesnt exist. please try another one.");
                    return;
                }
                string name = textBoxAuthorName.Text;
                string surname = textBoxSurname.Text;
                string biography = textBoxBiography.Text;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand("UPDATEAUTHOR", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@AuthorID", Convert.ToInt32(id)));
                    command.Parameters.Add(new SqlParameter("@AuthorName", name));
                    command.Parameters.Add(new SqlParameter("@AuthorSurname", surname));
                    command.Parameters.Add(new SqlParameter("@Biography", biography));

                    connection.Open();
                    command.ExecuteNonQuery();
                }
                ShowAuthorsData("SELECT * FROM AUTHORS");
                GetAllAuthorsInComboBox();
            }

        }

        private void deleteAuuthor_Click(object sender, RoutedEventArgs e)
        {
            string idString = textBoxAuthorID.Text;
            if (idString == "")
                MessageBox.Show("Введите ID");
            else
            {
                int id = Convert.ToInt32(idString);
                if (!GetAllAuthorsID().Contains(id))
                {
                    MessageBox.Show("this ID doesnt exist. please try another one.");
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            SqlCommand command = new SqlCommand("deleteauthor", connection, transaction);
                            command.CommandType = CommandType.StoredProcedure;

                            command.Parameters.Add(new SqlParameter("@authorid", id));

                            command.ExecuteNonQuery();


                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
                ShowAuthorsData("SELECT * FROM AUTHORS");
                GetAllAuthorsInComboBox();
            }

        }

        private void dataGridAuthors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)dataGridAuthors.SelectedItem;
                textBoxAuthorID.Text = selectedRow.Row["ID"].ToString();
                textBoxAuthorName.Text = selectedRow.Row["Name"].ToString();
                textBoxSurname.Text = selectedRow.Row["Surname"].ToString();
                textBoxBiography.Text = selectedRow.Row["biography"].ToString();
            }
        }
    }
}
