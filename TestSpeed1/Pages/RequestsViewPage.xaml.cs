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

namespace TestSpeed1.Pages
{
    /// <summary>
    /// Логика взаимодействия для RequestsViewPage.xaml
    /// </summary>
    public partial class RequestsViewPage : Page
    {
        List<Requests> dataFromDB = new List<Requests>();
        public RequestsViewPage()
        {
            InitializeComponent();

            DataContext = new Requests();

            var StatusList = AppManager.Context.Statuses.ToList();
            StatusList.Insert(0, new Statuses
            {
                Name = "Без сортировки"
            });
            cboxFilter.ItemsSource = StatusList;

            cboxSort.SelectedIndex = 0;
            cboxFilter.SelectedIndex = 0;

            dataFromDB = AppManager.Context.Requests.ToList();

            UpdateListView();
        }

        #region UpdateEvents
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dataFromDB = AppManager.Context.Requests.ToList();
            UpdateListView();
        }
        private void cboxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateListView();
        }
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateListView();
        }
        private void cboxFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateListView();
        }
        private void CBoxSortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateListView();
        }
        private void CBoxOrdByProductType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateListView();
        }
        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateListView();
        }
        #endregion

        // Add + Edit + Delete buttons controls
        #region ControlsFunctions
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RequestsAddEditPage(null));
        }
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var dataDeleted = false;
            var elemsToDelete = LViewProduct.SelectedItems.Cast<Requests>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить следующие {elemsToDelete.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    foreach (var elem in elemsToDelete)
                    {
                        if (elem.Status != 3)
                        {
                            MessageBox.Show(elem.DeviceAndProblemText + " - не удален,\nтак как его статус не - 'Выполен'");
                        }
                        else
                        {
                            dataDeleted = true;
                            AppManager.Context.Requests.Remove(elem);
                        }
                    }
                    if (dataDeleted)
                    {
                        MessageBox.Show("Данные удалены!");
                        AppManager.Context.SaveChanges();
                        dataFromDB = AppManager.Context.Requests.ToList();
                        UpdateListView();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
        // Edit by double click on record
        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
                if (AppManager.CurrentUser != null && AppManager.CurrentUser.Role == 1)
                {
                    NavigationService.Navigate(new RequestsAddEditPage((sender as ListViewItem).Content as Requests));
                }
        }
        #endregion

        // Function of GridView update + Sorting
        private void UpdateListView()
        {
            //Fill the table with data based on user role 
            var dataSource = dataFromDB;
            var currentUser = new Users();
            if (AppManager.CurrentUser != null)
                currentUser = AppManager.CurrentUser;

            int countBeforeFilter = dataSource.Count;

            //Sorting
            switch (cboxSort.SelectedIndex)
            {
                case 1:
                    dataSource = dataSource.OrderBy(p => p.BeginDate).ToList();
                    break;
                case 2:
                    dataSource = dataSource.OrderByDescending(p => p.BeginDate).ToList();
                    break;
                default:
                    dataSource = dataSource.OrderBy(p => p.ID).ToList();
                    break;
            }

            //Filtration
            if (cboxFilter.SelectedIndex != 0)
            {
                dataSource = dataSource.Where(p => p.Statuses == cboxFilter.SelectedValue).ToList();
            }

            if (txtSearch.Text.Length > 0)
            {
                dataSource = dataSource.Where(p => p.Devices.Name.ToLower().Contains(txtSearch.Text.ToLower()) ||
                    p.TypesOfProblems.Name.ToLower().Contains(txtSearch.Text.ToLower()) ||
                    p.Clients.FullName.ToLower().Contains(txtSearch.Text.ToLower()) ||
                    p.Statuses.Name.ToString().ToLower().Contains(txtSearch.Text.ToLower()) ||
                    p.Description.ToString().ToLower().Contains(txtSearch.Text.ToLower())
                    ).ToList();
            }

            //Items counter
            tbkItemCounter.Text = dataSource.Count.ToString() + " из " + countBeforeFilter.ToString();

            LViewProduct.ItemsSource = dataSource;

            if (dataSource.Count < 1)
                tbkItemCounter.Text += "\nПо вашему запросу ничего не найдено. Измените фильтры.";
        }
    }
}
