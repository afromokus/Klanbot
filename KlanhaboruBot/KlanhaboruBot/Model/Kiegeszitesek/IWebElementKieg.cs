using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlanhaboruBot.Model.Kiegeszitesek
{
    public static class IWebElementKieg
    {
        public static string getInnerHTML(this IWebElement webElement)
        {
            try
            {
                return webElement.GetProperty("innerHTML");
            }
            catch
            {
                //System.Windows.Forms.MessageBox.Show("hiba: " + webElement.Text);
                return "";
            }
        }
    }

}
