using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
namespace DC
{


    public static class Encryption
    {
        private static byte[] _salt = Encoding.ASCII.GetBytes("b9eafac5-27f5-446a-b8fa-f1a9f9b3788e");
        private static string key = "beb65e7b-1daa-4f8c-868a-0cffb08dfa23";




        public static string Password(int length = 6)
        {
            string strpassword;
            try
            {
                const string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

                StringBuilder sb = new StringBuilder();
                Random rnd = new Random();

                for (int i = 0; i < length; i++)
                {
                    int index = rnd.Next(chars.Length);
                    sb.Append(chars[index]);
                }

                strpassword= sb.ToString();
            }
            catch (Exception)
            {
                strpassword = "123456";
            } 
            return EncryptCommon(strpassword.ToLower());
        }
        public static string PasswordD(int length = 6)
        {
            string strpassword;
            try
            {
                const string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ*&()";

                StringBuilder sb = new StringBuilder();
                Random rnd = new Random();

                for (int i = 0; i < length; i++)
                {
                    int index = rnd.Next(chars.Length);
                    sb.Append(chars[index]);
                }

                strpassword = sb.ToString();
            }
            catch (Exception)
            {
                strpassword = "123456";
            }
            return  EncryptCommon(strpassword.ToLower());
        }

        public static string EncryptCommon(string textToEncrypt, string _key = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textToEncrypt))
                {
                    return "";
                }
                RijndaelManaged rijndaelCipher = new RijndaelManaged()
                {
                    Mode = CipherMode.CBC,
                    Padding = PaddingMode.PKCS7
                };
                key = _key == "" ? key : _key;
                rijndaelCipher.KeySize = 0x80; // 256bit key
                rijndaelCipher.BlockSize = 0x80;
                byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
                byte[] keyBytes = new byte[0x10];
                int len = pwdBytes.Length;
                if (len > keyBytes.Length)
                {
                    len = keyBytes.Length;
                }
                Array.Copy(pwdBytes, keyBytes, len);
                rijndaelCipher.Key = keyBytes;
                rijndaelCipher.IV = keyBytes;
                ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(textToEncrypt);
                return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string DecryptCommon(string textToDecrypt, string _key = "")
        {
            if (string.IsNullOrWhiteSpace(textToDecrypt))
            {
                return "";
            }
            key = _key == "" ? key : _key;
            RijndaelManaged rijndaelCipher = new RijndaelManaged()
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,

                KeySize = 0x80,
                BlockSize = 0x80
            };
            byte[] encryptedData = Convert.FromBase64String(textToDecrypt);
            byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyBytes = new byte[0x10];
            int len = pwdBytes.Length;
            if (len > keyBytes.Length)
            {
                len = keyBytes.Length;
            }
            Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            rijndaelCipher.IV = keyBytes;
            byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
            return Encoding.UTF8.GetString(plainText);
        }
    }

    public static class ReserveDomain
    {

        public static List<string> Reserved = new List<string>
                 {
                         "www",
                         "http",
                         "api",
                         "secure",
                         "help",
                         "live",
                         "mail",
                         "ftp",
                         "text",
                         "txt",
                         "flixsign",
                         "https",
                         "product",
                         "registration",
                         "register",
                         "login",
                         "logout",
                         "signin",
                         "signout",
                         "email",
                         "smtp",
                         "pop",
                         "pop3",
                         "mailserver",
                         "verify",
                         "awverify",
                         "web",
                         "website",
                         "localhost",
                         "averify.www",
                         "calender",
                         "fax",
                         "azure",
                         "imap",
                         "mobilemail",
                         "manage",
                 };
    }

    public static class ListToDataTable
    {
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }


}
