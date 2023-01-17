using System.Web.Mvc;
using Nop.Web.Framework.UI.Paging;

namespace Nop.Plugin.Widgets.Event.Extensions
{
    public static class PagerHtmlExtension
    {
        public static Pager Pager(this HtmlHelper helper, IPageableModel pagination)
        {
            return new Pager(pagination, helper.ViewContext);
        }
    }
}
