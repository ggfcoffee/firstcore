using IBLL;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Extensions;
using DotNetCore.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using IDAL;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Options;
using Common;
using Microsoft.AspNetCore.Hosting;


namespace DotNetCore.Controllers
{
    public class HomeController : Controller
    {
        IMemoryCache cache;
        IAccountService AccountService;
        AppSettings Configuration;
        IHostingEnvironment Env;
        public HomeController(IMemoryCache cache,IAccountService accountService,IOptions<AppSettings> appSettings,IHostingEnvironment env)
        {
            this.cache = cache;
            this.AccountService = accountService;
            this.Configuration = appSettings.Value;
            this.Env = env;
        }
        public async Task<IActionResult> Index()
        {

            

            //string configrationMyName = Configuration.GetValue<string>("MyName");
            ViewBag.ConfigrationMyName = Configuration.MyName;

            ViewBag.Env = Env.ContentRootPath;

           
            

            IUserRepository userRepository = HttpContext.RequestServices.GetService<IUserRepository>();

            ILoggerFactory logFac = HttpContext.RequestServices.GetService<ILoggerFactory>();

            ILogger<HomeController> homeLog = logFac.CreateLogger<HomeController>();

            //System.IO.File.Create(Path.Combine(Env.ContentRootPath, "aa.txt"));

            //IMemoryCache cache = app.ApplicationServices.GetService<IMemoryCache>();
            string cacheNow = cache.Get<string>("NowTime");
            if (string.IsNullOrEmpty(cacheNow))
            {
                string value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                cache.Set<string>("NowTime", value, TimeSpan.FromSeconds(10));
                cacheNow = value;
            }
            using (SqlConnection conn = new SqlConnection("Data Source=.\\SQLServer2008R2;Initial Catalog=LifeManagerServer;User ID=sa;Password=sls_taobao;"))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("Select count(*) from sls_life_user", conn);
                object obj = command.ExecuteScalar();
                ViewBag.Count = cacheNow;
            }
            ViewBag.Result = await AccountService.GetAccountName();

            ClaimsIdentity identity = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, "高广飞")}, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            await Request.HttpContext.Authentication.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties() { IsPersistent = true });

            HttpContext.Session.Set("MySessionName", System.Text.Encoding.UTF8.GetBytes("高广飞sessionName"));


            
                Blog blog = new Blog() { Title="这里是博客的标题" };

            string blogObjStr = JsonConvert.SerializeObject(blog);
            HttpContext.Session.SetString("MyBlog", blogObjStr);
                
                //using(BinaryWriter write = new BinaryWriter())

            

                return View();
        }

        public async Task<IActionResult> About()
        {
            AuthenticateInfo info = await Request.HttpContext.Authentication.GetAuthenticateInfoAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            string CookieResult = info.Principal.Claims.First().Value;

            byte[] sessionUserNameBytes;
            string sessionUserName = string.Empty;
            if (HttpContext.Session.TryGetValue("MySessionName", out sessionUserNameBytes))
            {
                sessionUserName = System.Text.Encoding.UTF8.GetString(sessionUserNameBytes);
            }
            ViewBag.SessionUserName = sessionUserName;
            ViewBag.CookieUserName = CookieResult;

            ViewBag.Blog = JsonConvert.DeserializeObject<Blog>(HttpContext.Session.GetString("MyBlog"));



                ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public JsonResult GetJson()
        {
            var obj = new { Name = "aa",Value = "1"};
            return Json(obj);
        }
    }
}
