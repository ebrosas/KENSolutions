using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace KenHRApp.Application.Common.Helpers
{
    public static class ServiceHelper
    {
        #region Public Methods
        public static string RetrieveXmlMessage(string xmlFile)
        {
            string message = String.Empty;
            XmlTextReader reader = null;

            try
            {
                // Read the file
                reader = new XmlTextReader(xmlFile);
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Text)
                        message = reader.Value;
                }
            }

            catch
            {
            }
            finally
            {
                // Close the file
                if (reader != null)
                    reader.Close();
            }

            return message;
        }
        #endregion
    }
}
