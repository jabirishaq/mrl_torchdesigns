using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nop.Services.Orders;

namespace Nop.Admin.Handler
{
    /// <summary>
    /// Summary description for SalesReport
    /// </summary>
    public class SalesReport : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest request = context.Request;


 
        
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}