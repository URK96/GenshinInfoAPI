﻿using System;
using System.Security.Cryptography;
using System.Text;

namespace GenshinInfo
{
    public static class Utils
    {
        public static string AnalyzeServer(string uid)
        {
            return string.IsNullOrWhiteSpace(uid) ? string.Empty : uid[0] switch
            {
                '1' or '2' => "cn_gf01",
                '5' => "cn_qd01",
                '6' => "os_usa",
                '7' => "os_euro",
                '8' => "os_asia",
                '9' => "os_cht",
                _ => string.Empty
            };
        }

        public static string GenerateDS()
        {
            const string R = "abcdef";
            const string DSSalt = "6cqshh5dhw73bzxn20oexa9k516chk7s";

            long epoch = DateTimeOffset.Now.ToUnixTimeMilliseconds() / 1000;
            string hashOriginal = $"salt={DSSalt}&t={epoch}&r={R}";
            byte[] hashCodes = new MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(hashOriginal));

            StringBuilder sb = new();

            foreach (byte code in hashCodes)
            {
                sb.Append(code.ToString("x2"));
            }

            return $"{epoch},{R},{sb}";
        }
    }
}
