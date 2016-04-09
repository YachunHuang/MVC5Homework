using System;
using System.Diagnostics;
using System.Web.Mvc;

namespace MVC5Homework.Controllers
{
    internal class 計算Action的執行時間Attribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.dtStart = DateTime.Now;
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.Controller.ViewBag.dtEnd = DateTime.Now;
            var dtTimeSpan = (DateTime)filterContext.Controller.ViewBag.dtEnd -
                (DateTime)filterContext.Controller.ViewBag.dtStart;
            //filterContext.Controller.ViewBag.dtTimespan = dtTimeSpan;

            var ControllerActionExecuteLog = string.Format("Controller:{0},Action:{1},ExcuteTime:{2}", 
                filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                filterContext.ActionDescriptor.ActionName, 
                dtTimeSpan);
            Debug.WriteLine(ControllerActionExecuteLog);
            base.OnActionExecuted(filterContext);
        }
    }
}