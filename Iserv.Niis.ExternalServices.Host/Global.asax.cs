using System;
using System.Web;
using Autofac.Integration.Wcf;
using Iserv.Niis.ExternalServices.Host.Container;

namespace Iserv.Niis.ExternalServices.Host
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            var container = new AutofacContainer().Build();

            AutofacHostFactory.Container = container;
        }
    }
}