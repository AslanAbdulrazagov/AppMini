using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMini.Exceptions
{
    public class ClasroomNotFoundException:Exception
    {
        public ClasroomNotFoundException(string message):base(message)
        {
            
        }
    }
}
