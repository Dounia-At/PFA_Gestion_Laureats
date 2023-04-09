using Microsoft.AspNetCore.Mvc.Filters;

namespace PFA_Gestion_Laureats.Validation
{
    public class Authentification: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetString("Login") == null)
            {
                //context.HttpContext.Response.Redirect("/User/Login");
                context.HttpContext.Session.SetString("Login", "DouniaAt");
            }
        }
    }
}
