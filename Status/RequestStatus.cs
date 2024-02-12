using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primary_Massager.Status
{
    public enum RequestStatus
    {
        OK,
        Registered,
        Logged_In,
        Canceled,
        NotRegistered,
        User_AlredyExists,
        User_DoesntExist,
    }
}
