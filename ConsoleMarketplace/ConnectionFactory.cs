using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMarketplace
{
    public class ConnectionFactory
    {
        public static IOrganizationService GetService()
        {
            string connectionString =
               "AuthType=OAuth;" +
               "Username=admin@DynaCoopMyWork1.onmicrosoft.com;" +
               "Password=0Uv*&z3p45QS;" +
               "Url=https://orgb5504b2c.crm2.dynamics.com/;" +
               "AppId=1e710d1e-e62c-4d28-98a5-d69ce2b02c38;" +
               "RedirectUri=app://58145B91-0C36-4500-8554-080854F2AC97;";

            CrmServiceClient crmServiceClient = new CrmServiceClient(connectionString);

            return crmServiceClient.OrganizationWebProxyClient;
        }
    }
}
