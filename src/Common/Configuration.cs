using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;


namespace Common
{
    public class Configuration
    {
        IOptions<AppSettings> option;
        public Configuration(IOptions<AppSettings> option)
        {
            this.option = option;
        }


    }
}
