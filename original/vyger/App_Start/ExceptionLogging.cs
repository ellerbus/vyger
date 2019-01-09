using System;

namespace vyger
{
    public class ExceptionLogging
    {
        #region Static Methods

        public static void LogException(Exception exp)
        {
            //if (Constants.IsDeployed)
            //{
            //    //StringBuilder html = new StringBuilder();

            //    //html.Append(exp.ToKeyValuePairs().ToHtml()).AppendLine();

            //    //if (HttpContext.Current != null && HttpContext.Current.Request != null)
            //    //{
            //    //    HttpRequest req = HttpContext.Current.Request;

            //    //    html.Append(req.ToKeyValuePairs().ToHtml());
            //    //}

            //    //MailerService mailer = new MailerService();

            //    //mailer.Send("stu.ellerbusch@gmail.com", "Unhandled Exception", html.ToString());
            //}
        }

        #endregion
    }
}