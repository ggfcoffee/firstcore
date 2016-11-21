using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDAL;

namespace DAL
{
    public class UserRepository : IUserRepository
    {
        public string GetName()
        {
            return "高广飞";
        }
    }
}
