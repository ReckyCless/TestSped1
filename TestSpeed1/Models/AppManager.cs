using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSpeed1.Models
{
    public class AppManager
    {
        public static DEMODBEntities Context { get; } = new DEMODBEntities();

        public static Users CurrentUser = null;
    }
}
