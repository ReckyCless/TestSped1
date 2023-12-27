using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для RequestsAddEditPage.xaml
    /// </summary>
    public partial class RequestsAddEditPage : Page
    {
        private Requests currentElem = new Requests();
        public RequestsAddEditPage(Requests elemData)
        {
            InitializeComponent();

            dtBeginDate.DisplayDateStart = new DateTime(1950, 01, 01);
            dtBeginDate.DisplayDateEnd = DateTime.Today;

            dtEndDate.DisplayDateStart = new DateTime(1950, 01, 01);
            dtEndDate.DisplayDateEnd = DateTime.Today;

            if (elemData != null)
            {
                Title = "Заявки. Редактирование";
                currentElem = elemData;
            }
            else
            {
                currentElem.Status = 2; 
                currentElem.BeginDate = DateTime.Now;

                if (AppManager.CurrentUser != null)
                {
                    var currentUser = AppManager.CurrentUser;
                    if (AppManager.Context.Clients.Any(p => p.UserID == currentUser.ID))
                    {
                        currentElem.Clients = AppManager.Context.Clients.FirstOrDefault(p => p.UserID == AppManager.CurrentUser.ID);
                    }
                    else
                    {
                        var client = new Clients();
                        client.Users = currentUser;
                        currentElem.Clients = client;
                    }
                }
                else
                {
                    currentElem.Clients = new Clients();
                }
            }

            DataContext = currentElem;

            cboxStatus.ItemsSource = AppManager.Context.Statuses.ToList();
            cboxDevice.ItemsSource = AppManager.Context.Devices.ToList();
            cboxProblem.ItemsSource = AppManager.Context.TypesOfProblems.ToList();
            cboxWorkers.ItemsSource = AppManager.Context.Workers.ToList();

        }

        #region Regexes
        private void TextValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[а-яА-ЯёЁa-zA-Z0-9+,.(){}""''-]+$");
            e.Handled = !regex.IsMatch(e.Text);
        }
        private void TextBox_PreviewKeyDownNoSpace(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
        #endregion

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Check if textboxes is filled | Проверка на заполенение полей
            StringBuilder err = new StringBuilder();
            if (string.IsNullOrWhiteSpace(currentElem.Clients.FirstName))
                err.AppendLine("Укажите имя");
            if (string.IsNullOrWhiteSpace(currentElem.Clients.SecondName))
                err.AppendLine("Укажите фамилию");
            if (currentElem.Devices == null) 
                err.AppendLine("Укажите устройство");
            if (currentElem.TypesOfProblems == null)
                err.AppendLine("Укажите тип неполадки");
            if (string.IsNullOrWhiteSpace(currentElem.Description))
                err.AppendLine("Опишите проблему");
            if (currentElem.BeginDate > DateTime.Now)
                err.AppendLine("Дата начала не может быть позже сегоднешней");
            if (currentElem.BeginDate < new DateTime(1000, 1, 1))
                err.AppendLine("Дата заявления не может быть раньше 1000 года");
            if (currentElem.EndDate.HasValue)
            {
                if (currentElem.BeginDate > currentElem.EndDate)
                    err.AppendLine("Дата заявления не может быть позже даты завершения");
                if (currentElem.EndDate < new DateTime(1000, 1, 1))
                    err.AppendLine("Дата завершения не может быть раньше 1000 года");
            }
            if (currentElem.Statuses != null && currentElem.Statuses.ID != 2 && currentElem.Statuses.ID != 4)
            {   
                if (currentElem.Workers == null)
                {
                    err.AppendLine("Добавьте работника");
                }
            }

            if (err.Length > 0)
            {
                MessageBox.Show(err.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            currentElem.Description = currentElem.Description.Trim();
            currentElem.Description = Regex.Replace(currentElem.Description, @"\s+", " ");


            if (currentElem.ID == 0)
            {
                AppManager.Context.Requests.Add(currentElem);
            }

            try
            {
                AppManager.Context.SaveChanges();
                MessageBox.Show("Данные сохранены");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void cboxStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cmb = sender as ComboBox;
            var status = (Statuses)cmb.SelectedItem;
            if (status.ID == 3)
            {
                currentElem.EndDate = DateTime.Now;
                dtEndDate.SelectedDate = DateTime.Now;
            }
        }
    }
}
