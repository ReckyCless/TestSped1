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
using TestSpeed1.Models;

namespace TestSpeed1.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AppManager.CurrentUser = null;
            txtLogin.Text = "";
            txtPassword.Password = "";
        }

        private void btnAuth_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var currentUser = AppManager.Context.Users.AsEnumerable().FirstOrDefault(p => p.Login == txtLogin.Text && p.Password == txtPassword.Password);
                if (currentUser != null)
                {
                    AppManager.CurrentUser = currentUser;
                    MessageBox.Show("Авторизация успешна!");
                    MainWindow windowMain = new MainWindow();
                    windowMain.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Пользователь - не найден");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            MainWindow windowMain = new MainWindow();
            windowMain.Show();
            this.Close();
        }
    }
}
