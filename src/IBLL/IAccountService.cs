using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IBLL
{
    public interface IAccountService
    {
        Task<string> GetAccountName();
    }
}
