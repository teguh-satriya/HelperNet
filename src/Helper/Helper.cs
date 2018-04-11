using System;
using System.Security.Cryptography;
using System.Text;

namespace Helper
{
    public class Helper
    {
        public static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = MD5.Create();  //or use SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        public static string IndonesianDate(DateTime date)
        {
            string month = "";
            switch (date.Month)
            {
                case 1:
                    month = "Januari";
                    break;
                case 2:
                    month = "Februari";
                    break;
                case 3:
                    month = "Maret";
                    break;
                case 4:
                    month = "April";
                    break;
                case 5:
                    month = "Mei";
                    break;
                case 6:
                    month = "Juni";
                    break;
                case 7:
                    month = "Juli";
                    break;
                case 8:
                    month = "Agustus";
                    break;
                case 9:
                    month = "September";
                    break;
                case 10:
                    month = "Oktober";
                    break;
                case 11:
                    month = "November";
                    break;
                case 12:
                    month = "Desember";
                    break;
            }

            return date.Day.ToString() + " " + month + " " + date.Year.ToString();
        }
    }
}
