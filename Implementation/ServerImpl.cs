using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Implementation
{
    public class ServerImpl
    {
        public DateTime FechaHoraServer()
        {
            return DBImplementation.fechaHoraServidor();
        }
    }
}
