using log4net;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Web.config", Watch = true)]

namespace WS.BM.Deposito.Plazos
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private readonly ILog Log = LogManager.GetLogger(typeof(LoggerManager));
        protected void Application_Start()
        {
            
            log4net.Config.XmlConfigurator.Configure();
            GlobalConfiguration.Configure(WebApiConfig.Register);

            Log.Info("INICIO DE SERVICIOS WEB DESACOPLADOS INVERSIONES.");

        }
    }
}
