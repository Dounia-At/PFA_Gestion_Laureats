﻿using Microsoft.AspNetCore.Mvc.Filters;

namespace PFA_Gestion_Laureats.Validation
{
    public class IsAdmin : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetString("Role") != "AgentDirection")
            {
                context.HttpContext.Response.Redirect("/Annonce/Annonces");
            }
        }
    }
}
