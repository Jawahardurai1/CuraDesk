using System;
using System.Collections.Generic;
using System.Text;

namespace CuraDesk.Business.Exceptions
{
    public class PasswordMisMatchException:Exception
    {
        public PasswordMisMatchException(string message):base(message) { }
    }
}
