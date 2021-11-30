using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jobs.Google.Drive
{
    internal interface ISpreadsheet
    {
        void CreateService();
        void CreateData();
        void UpdateData();
        void DeleteData();
    }
}
