using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace Assignment6
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            // Runs once when the application first starts
            // Initialize application-wide variables
            Application["AppName"] = "ASU Swim Club";
            Application["TotalVisitors"] = 0;
            Application["AppStartTime"] = DateTime.Now.ToString("MMMM dd, yyyy hh:mm tt");

            //Initialize counter for user opening swim club
            Application["Swimmers"] = 0;
            Application["CurrentEvent"] = "";
            Application["NextEvent"] = "";

        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // Runs when a new user session starts
            // Increment visitor count
            Application.Lock();
            Application["TotalVisitors"] = (int)Application["TotalVisitors"] + 1;
            Application.UnLock();

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // Runs when an unhandled error occurs
            Exception ex = Server.GetLastError();
            if (ex != null)
            {
                // Log error to Application state for visibility
                Application["LastError"] = ex.Message;
            }

        }

        protected void Session_End(object sender, EventArgs e)
        {
            // Runs when a session ends or times out
            Application.Lock();
            Application["TotalVisitors"] = (int)Application["TotalVisitors"] - 1;
            Application.UnLock();

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}