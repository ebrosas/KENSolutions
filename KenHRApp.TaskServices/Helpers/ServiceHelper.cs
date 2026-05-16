using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace KenHRApp.TaskServices.Helpers
{
    public static class ServiceHelper
    {
        #region Converters
        public static string ConvertStringToTitleCase(string input)
        {
            System.Globalization.CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Globalization.TextInfo textInfo = cultureInfo.TextInfo;
            return textInfo.ToTitleCase(input.ToLower());
        }

        public static int ConvertObjectToInt(object value)
        {
            int result;
            if (value != null && int.TryParse(value.ToString(), out result))
                return result;
            else
                return 0;
        }

        public static long ConvertObjectToLong(object value)
        {
            long result;
            if (value != null && long.TryParse(value.ToString(), out result))
                return result;
            else
                return 0;
        }

        public static double ConvertObjectToDouble(object value)
        {
            double result;
            if (value != null && double.TryParse(value.ToString(), out result))
                return result;
            else
                return 0;
        }

        public static bool ConvertObjectToBolean(object value)
        {
            bool result;
            if (value != null && bool.TryParse(value.ToString(), out result))
                return result;
            else
                return false;
        }

        public static bool ConvertNumberToBolean(object value)
        {
            if (value != null && Convert.ToInt32(value) == 1)
                return true;
            else
                return false;
        }

        public static DateTime? ConvertObjectToDate(object value)
        {
            DateTime result;
            if (value != null && DateTime.TryParse(value.ToString(), out result))
                return result;
            else
                return null;
        }

        public static string ConvertObjectToDateTimeString(object value)
        {
            DateTime result;

            try
            {
                if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name.Trim() != "en-GB")
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
                }

                if (value != null && DateTime.TryParse(value.ToString(), out result))
                    return result.ToString("dd-MMM-yyyy hh:mm tt");
                else
                    return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string ConvertObjectToTimeString(object value)
        {
            DateTime result;

            try
            {
                if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name.Trim() != "en-GB")
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
                }

                if (value != null && DateTime.TryParse(value.ToString(), out result))
                    return result.ToString("HH:mm:ss");
                else
                    return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string ConvertObjectToDateString(object value)
        {
            DateTime result;

            try
            {
                if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name.Trim() != "en-GB")
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
                }

                if (value != null && DateTime.TryParse(value.ToString(), out result))
                    return result.ToString("dd-MMM-yyyy");
                else
                    return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static DateTime? ConvertObjectToDateNew(object value)
        {
            try
            {
                if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name.Trim() != "en-GB")
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
                }

                DateTime result;
                if (value != null && DateTime.TryParse(value.ToString(), out result))
                    return result;
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static byte ConvertObjectToByte(object value)
        {
            byte result;

            try
            {
                if (value != null && byte.TryParse(value.ToString(), out result))
                    return result;
                else
                    return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static string ConvertObjectToUSDateString(object value)
        {
            DateTime result;

            try
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.Trim() != "en-US")
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

                if (value != null && DateTime.TryParse(value.ToString(), out result))
                    return result.ToString("MM/dd/yyyy");
                else
                    return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string ConvertObjectToBritishDateString(object value)
        {
            DateTime result;

            try
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.Trim() != "en-GB")
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

                if (value != null && DateTime.TryParse(value.ToString(), out result))
                    return result.ToString("dd/MM/yyyy");
                else
                    return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static DateTime ConvertObjectToRealDate(object value)
        {
            try
            {
                if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name.Trim() != "en-GB")
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
                }

                DateTime result;
                if (value != null && DateTime.TryParse(value.ToString(), out result))
                    return result;
                else
                    return DateTime.Now;
            }
            catch (Exception)
            {
                return DateTime.Now;
            }
        }

        public static string ConvertObjectToString(object value)
        {
            return value != null ? value.ToString().Trim() : string.Empty;
        }

        public static string GetUserFirstName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return string.Empty;

            try
            {
                if (userName.ToUpper().Trim() == "WATER TREATMENT TERMINAL")
                {
                    return userName;
                }

                string result = string.Empty;
                Match m = Regex.Match(userName, @"(\w*) (\w.*)");
                string firstName = m.Groups[1].ToString();

                if (!string.IsNullOrEmpty(firstName))
                {
                    System.Globalization.CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
                    System.Globalization.TextInfo textInfo = cultureInfo.TextInfo;
                    result = textInfo.ToTitleCase(firstName.ToLower().Trim());
                }

                return result;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string GetFirstAndLastName(string inputName)
        {
            if (string.IsNullOrEmpty(inputName))
                return string.Empty;

            try
            {
                string[] nameArray = inputName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string firstName = string.Empty;
                string lastName = string.Empty;
                string fullName = string.Empty;

                // Get the first name
                firstName = nameArray[0];

                // Get the last name
                if (nameArray.Length > 1)
                    lastName = nameArray[nameArray.Length - 1];

                fullName = string.Format("{0} {1}", firstName, lastName).Trim();

                System.Globalization.CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
                System.Globalization.TextInfo textInfo = cultureInfo.TextInfo;
                return textInfo.ToTitleCase(fullName.ToLower());
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string GetFirstAndSecondName(string inputName)
        {
            if (string.IsNullOrEmpty(inputName))
                return string.Empty;

            try
            {
                string[] nameArray = inputName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string firstName = string.Empty;
                string secondName = string.Empty;
                string fullName = string.Empty;

                // Get the first name
                firstName = nameArray[0];

                // Get the last name
                if (nameArray.Length > 1)
                    secondName = nameArray[1];

                fullName = string.Format("{0} {1}", firstName, secondName).Trim();

                System.Globalization.CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
                System.Globalization.TextInfo textInfo = cultureInfo.TextInfo;
                return textInfo.ToTitleCase(fullName.ToLower());
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string GetFirstToFourthName(string inputName, bool convertToProperCase = true)
        {
            if (string.IsNullOrEmpty(inputName))
                return string.Empty;

            try
            {
                string[] nameArray = inputName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string firstName = string.Empty;
                string secondName = string.Empty;
                string thirdName = string.Empty;
                string fourthName = string.Empty;
                string fullName = string.Empty;

                // Get the first name
                firstName = nameArray[0];

                // Get the second name
                if (nameArray.Length > 1)
                    secondName = nameArray[1];

                // Get the third name
                if (nameArray.Length > 2)
                    thirdName = nameArray[2];

                // Get the fourth name
                if (nameArray.Length > 3)
                    fourthName = nameArray[3];

                fullName = string.Format("{0} {1} {2} {3}", firstName, secondName, thirdName, fourthName).Trim();

                if (convertToProperCase)
                {
                    System.Globalization.CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
                    System.Globalization.TextInfo textInfo = cultureInfo.TextInfo;
                    return textInfo.ToTitleCase(fullName.ToLower());
                }
                else
                    return fullName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string ConvertMinuteToHourString(dynamic minuteValue)
        {
            string result = string.Empty;

            try
            {
                if (minuteValue != null)
                {
                    int inputValue = Convert.ToInt32(minuteValue);
                    if (inputValue > 0)
                    {
                        int hrs = 0;
                        int min = 0;

                        hrs = Math.DivRem(inputValue, 60, out min);
                        result = string.Format("{0}:{1}",
                            string.Format("{0:00}", hrs),
                            string.Format("{0:00}", min));
                    }
                }

                return result;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        #endregion
    }
}
