using System;
using System.Collections.Generic;
using System.Text;

namespace CuraDesk.Business.Exceptions
{
    public class TimeMissMatchException :Exception
    {
        public TimeMissMatchException(string message):base(message){ }
    }
}
