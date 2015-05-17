using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace ExcombiiAPI.BusinessLayer
{
    public class BLCommon
    {
        #region integer properties
        private int _iResultNo = 0;

        public int IResultNo
        {
            get { return _iResultNo; }
            set { _iResultNo = value; }
        }
        #endregion

        #region string properties
        private string _sResult = "";

        public string SResult
        {
            get { return _sResult; }
            set { _sResult = value; }
        }
        #endregion

        #region Common Conversion Methods
        protected int ToInteger(object Value)
        {
            if (Value == null || Value == DBNull.Value)
                return 0;
            else if (Value.ToString() == "")
                return 0;
            else
                return Convert.ToInt32(Value);
        }

        protected string ToString(object Value)
        {
            if (Value == null || Value == DBNull.Value)
                return "";
            else
                return Value.ToString();
        }

        protected DateTime ToDateTime(object Value)
        {
            if (Value == null || Value == DBNull.Value)
                return DateTime.UtcNow.AddHours(10);
            else
            {
                try
                {
                    return Convert.ToDateTime(Value);
                }
                catch
                {
                    return new DateTime();
                }
            }
        }

        public static DateTime GetAustraliaDateTime()
        {
            return DateTime.UtcNow.AddHours(10);
        }

        public Int64 ToBigInt(object Value)
        {
            if (Value == null || Value == DBNull.Value)
                return 0;
            else if (Value.ToString() == "")
                return 0;
            else
                return Convert.ToInt64(Value);
        }

        public string ToDateTimeToString(object Value)
        {
            if (Value == null || Value == DBNull.Value)
                return "";
            else
            {
                try
                {
                    DateTime dtCurrent = Convert.ToDateTime(Value);

                    if (dtCurrent == new DateTime(1900, 1, 1))
                        return "";
                    else if (dtCurrent == new DateTime(2000, 1, 1))
                        return "";
                    else if (dtCurrent == new DateTime(2001, 1, 1))
                        return "";
                    else
                        return Convert.ToDateTime(Value).ToString("dd/MM/yyyy");
                }
                catch
                {
                    return "";
                }
            }
        }

        public string ToDateTimeToString(object Value, string sPattern)
        {
            if (Value == null || Value == DBNull.Value)
                return "";
            else
            {
                try
                {
                    DateTime dtCurrent = Convert.ToDateTime(Value);

                    if (dtCurrent == new DateTime(1900, 1, 1))
                        return "";
                    else if (dtCurrent == new DateTime(2000, 1, 1))
                        return "";
                    else if (dtCurrent == new DateTime(2001, 1, 1))
                        return "";
                    else
                        return Convert.ToDateTime(Value).ToString(sPattern);
                }
                catch
                {
                    return "";
                }
            }
        }

        public string ToTimeToString(object Value)
        {
            if (Value == null || Value == DBNull.Value)
                return "";
            else
            {
                try
                {
                    DateTime dtCurrent = Convert.ToDateTime(Value);

                    if (dtCurrent == new DateTime(1900, 1, 1))
                        return "";
                    else if (dtCurrent == new DateTime(2000, 1, 1))
                        return "";
                    else if (dtCurrent == new DateTime(2001, 1, 1))
                        return "";
                    else
                        return Convert.ToDateTime(Value).ToString("hh:mm tt");
                }
                catch
                {
                    return "";
                }
            }
        }


        protected double ToDouble(object Value)
        {
            if (Value == null || Value == DBNull.Value)
                return 0D;
            else
            {
                try
                {
                    return Convert.ToDouble(Value);
                }
                catch
                {
                    return 0D;
                }
            }
        }

        protected decimal ToDecimal(object Value)
        {
            if (Value == null || Value == DBNull.Value)
                return 0;
            else
            {
                try
                {
                    return Convert.ToDecimal(Value);
                }
                catch
                {
                    return 0;
                }
            }
        }


        protected bool ToBool(object Value)
        {
            if (Value == null || Value == DBNull.Value)
                return false;
            else
            {
                try
                {
                    return Convert.ToBoolean(Value);
                }
                catch
                {
                    return false;
                }
            }
        }

        public static string strProductName = "";
        #endregion

        #region Common checking methods
        public static bool IsDefaultDate(DateTime dtCurrent)
        {
            if (dtCurrent == null)
                return false;
            else if (dtCurrent == new DateTime(1900, 1, 1))
                return false;
            else if (dtCurrent == new DateTime(2000, 1, 1))
                return false;
            else if (dtCurrent == new DateTime(2001, 1, 1))
                return false;
            else if (dtCurrent == new DateTime(1, 1, 1))
                return false;

            else
                return true;
        }

        public static string GetDateTimeString(string Value)
        {
            try
            {
                return Convert.ToDateTime(Value).ToString("MM/dd/yyyy HH:mm:ss:fff tt");
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region email method
        public static void SendLogMail(string sModuleName, string sMessage)
        {
            try
            {
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsLogEnable"]))
                {
                    MailMessage MyMessage = new MailMessage();
                    MyMessage.From = new MailAddress(ConfigurationManager.AppSettings["LogMailFrom"].ToString());
                    MyMessage.To.Add(ConfigurationManager.AppSettings["LogMailTo"].ToString());
                    MyMessage.Subject = ConfigurationManager.AppSettings["LogMailSubject"].ToString();
                    MyMessage.IsBodyHtml = true;
                    MyMessage.BodyEncoding = System.Text.Encoding.UTF8;
                    MyMessage.Body = "Page Name: " + sModuleName + "<br/>Exception: " + sMessage;

                    SmtpClient emailClient = new SmtpClient(ConfigurationManager.AppSettings["LogMailClient"].ToString());

                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["LogIsMailCredentials"]))
                        emailClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["LogMailFrom"].ToString(), ConfigurationManager.AppSettings["LogMailPassword"].ToString());

                    emailClient.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["LogIsMailSSLEnable"]);
                    emailClient.Send(MyMessage);
                }
            }
            catch
            {

            }
        }
        #endregion

        #region Engrypt and Decrypt
        public string Encrypt(string sData)
        {
            try
            {
                byte[] encData_byte = new byte[sData.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(sData);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }

        public string Decrypt(string sData)
        {
            try
            {
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();
                byte[] todecode_byte = Convert.FromBase64String(sData);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string result = new String(decoded_char);
                return result;
            }
            catch (Exception ex)
            {
                string message = "Invalid User";
                return message;
            }
        }
        #endregion
    }
}