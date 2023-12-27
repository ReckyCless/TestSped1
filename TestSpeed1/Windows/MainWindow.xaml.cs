using System;
using System.Collections.Generic;
using System.Data;
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
using TestSpeed1.Models;

namespace TestSpeed1.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            frameMain.Navigate(new Pages.RequestsViewPage());
        }
        public void RejectChanges()
        {
            foreach (var entry in AppManager.Context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified; //Revert changes for deleted entries | Отмена изменений для удаленых значений  
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                }
            }
        }
        private void btnGoBack_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Несохраненные данные будут утеряны.\nУверены, что хотите продолжить?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
            {
                RejectChanges();
                frameMain.GoBack();
            }
        }

        private void frameMain_ContentRendered(object sender, EventArgs e)
        {
            if (frameMain.CanGoBack)
            {
                btnGoBack.Visibility = Visibility.Visible;
            }
            else
            {
                btnGoBack.Visibility = Visibility.Collapsed;
            }
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Несохраненные данные будут утеряны.\nУверены, что хотите продолжить?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
            {
                RejectChanges();
                AuthWindow authWindow = new AuthWindow();
                authWindow.Show();
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
