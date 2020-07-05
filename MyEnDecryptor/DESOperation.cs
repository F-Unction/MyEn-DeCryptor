using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace MyEnDecryptor
{
    class DESOperation
    {
        public static string EncryptString(string sInputString)
        {
            try
            {
                var sKey = "";
                var sIV = "";
                if (Form1.form1.textBox3.Text == "")
                {
                    sKey = DateTime.Now.ToString("yyyyMMdd");
                    sIV = DateTime.Now.ToString("yyyyMMdd");
                }
                else
                {
                    sKey = Form1.form1.textBox3.Text;
                    sIV = Form1.form1.textBox3.Text;
                }

                byte[] data = Encoding.UTF8.GetBytes(sInputString);
                DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
                DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                DES.IV = ASCIIEncoding.ASCII.GetBytes(sIV);

                ICryptoTransform desencrypt = DES.CreateEncryptor();
                byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
                return ":MESSAGE-" + BitConverter.ToString(result);
            }
            catch { }

            return "Error";
        }

        public static string DecryptString(string RsInputString)
        {
            try
            {
                var sKey = "";
                var sIV = "";
                if (Form1.form1.textBox3.Text == "")
                {
                    sKey = DateTime.Now.ToString("yyyyMMdd");
                    sIV = DateTime.Now.ToString("yyyyMMdd");
                }
                else
                {
                    sKey = Form1.form1.textBox3.Text;
                    sIV = Form1.form1.textBox3.Text;
                }
                var sInputString = RsInputString.Substring(9);

                string[] sInput = sInputString.Split("-".ToCharArray());
                byte[] data = new byte[sInput.Length];
                for (int i = 0; i < sInput.Length; i++)
                {
                    data[i] = byte.Parse(sInput[i], NumberStyles.HexNumber);
                }

                DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
                DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                DES.IV = ASCIIEncoding.ASCII.GetBytes(sIV);
                ICryptoTransform desencrypt = DES.CreateDecryptor();
                byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
                return Encoding.UTF8.GetString(result);
            }
            catch { }

            return "Error";
        }
    }
}