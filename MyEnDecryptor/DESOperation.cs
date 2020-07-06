using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MyEnDecryptor
{
    class DESOperation
    {
        public static string EncryptString(string sInputString)
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

            var btKey = Encoding.UTF8.GetBytes(sKey);
            var btIV = Encoding.UTF8.GetBytes(sIV);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] inData = Encoding.UTF8.GetBytes(sInputString);
                try
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(btKey, btIV), CryptoStreamMode.Write))
                    {
                        cs.Write(inData, 0, inData.Length);
                        cs.FlushFinalBlock();
                    }
                    return "=MeSsAgE="+Convert.ToBase64String(ms.ToArray());
                }
                catch { }
            }

            return "Error";
        }

        public static string DecryptString(string RencryptedString)
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
            var encryptedString = RencryptedString.Substring(9);

            var btKey = Encoding.UTF8.GetBytes(sKey);
            var btIV = Encoding.UTF8.GetBytes(sIV);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] inData = Convert.FromBase64String(encryptedString);
                try
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(btKey, btIV), CryptoStreamMode.Write))
                    {
                        cs.Write(inData, 0, inData.Length);
                        cs.FlushFinalBlock();
                    }
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
                catch { }
            }
            return "Error";
        }
    }
}