using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSpeed1.Models
{
    partial class Clients
    {
        public string FullName
        {
            get
            {
                return $"{SecondName} {FirstName} {Patronymic}";
            }
        }
    }
}
