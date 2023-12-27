using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestSpeed1.Models
{
    partial class Requests
    {
        public string DeviceAndProblemText
        {
            get
            {
                return Devices.Name + " | " + TypesOfProblems.Name;
            }
        }
        public string StatusText
        {
            get
            {
                return "Статус: " + Statuses.Name;
            }
        }
        public string DatesOfRequest
        {
            get
            {
                if (EndDate.HasValue)
                {
                    return BeginDate.Date.ToShortDateString() + " - " + EndDate.Value.Date.ToShortDateString();
                }
                else
                {
                    return BeginDate.Date.ToShortDateString() + " - " + "...";

                }
            }
        }
        public string ClientInfo
        {
            get
            {
                return Clients.FirstName + " " + Clients.SecondName + " " + Clients.Patronymic;
            }
        }
        public Visibility AdminVisibility
        {
            get
            {
                if (AppManager.CurrentUser != null && AppManager.CurrentUser.Role == 1)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }
    }
}
