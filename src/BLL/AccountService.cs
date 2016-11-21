using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IBLL;
using IDAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;

using Microsoft.Extensions.DependencyInjection;

namespace BLL
{
    public class AccountService : IAccountService
    {
        public IUserRepository UserRepository;

        public AccountService(IUserRepository userRepository)
        {
            UserRepository = userRepository;

            //这里是测试的内容提交  
            //这里是在服务器上修改的内容测试
        }

        public async Task<string> GetAccountName()
        {
            string userName = UserRepository.GetName();
            return "账户名是："+userName;
            //这里是高广飞添加的本地修改
        }

        public void Test()
        {
        }
    }
}
