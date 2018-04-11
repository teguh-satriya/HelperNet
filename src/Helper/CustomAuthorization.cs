using System;
using System.Web.Mvc;

namespace Helper
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    class CustomAuthorization : AuthorizeAttribute
    {
        private const string IS_AUTHORIZED = "isAuthorized";

        public string RedirectUrl = "~/Errors/unauthorized";

        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            bool isAuthorized = base.AuthorizeCore(httpContext);

            httpContext.Items.Add(IS_AUTHORIZED, isAuthorized);

            return isAuthorized;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            var isAuthorized = filterContext.HttpContext.Items[IS_AUTHORIZED] != null
                ? Convert.ToBoolean(filterContext.HttpContext.Items[IS_AUTHORIZED])
                : false;

            if ((!isAuthorized && filterContext.RequestContext.HttpContext.User.Identity.IsAuthenticated) || !filterContext.RequestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.RequestContext.HttpContext.Response.Redirect(RedirectUrl);
            }
        }
    }
}
