using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilis
{
    public interface IDispatcher
    {
        bool CheckAccess ( );
        Task RunAsync ( Action act );
    }
}
