using Cursos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cursos.Permisos
{
    public class ValidarSesionAdmin : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (HttpContext.Current.Session["usuario"] == null)
            {
                filterContext.Result = new RedirectResult("~/Acceso/Login");
            }
            else
            {
                usuario u = (usuario)HttpContext.Current.Session["usuario"];
                if(u.rol_id != 2)
                {
                    filterContext.Result = new RedirectResult("~/Acceso/Error");
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}