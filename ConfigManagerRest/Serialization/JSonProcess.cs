﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigManagerRest.Serialization
{
    public static class JSonProcess
    {
        public static string DecodeJSString(this string JsonText)
        {
            StringBuilder builder;
            char ch, ch2;
            int num, num2, num3, num4, num5, num6, num7, num8;
            if (string.IsNullOrEmpty(JsonText) || !JsonText.Contains(@"\"))
            {
                return JsonText;
            }
            builder = new StringBuilder();
            num = JsonText.Length;
            num2 = 0;
            while (num2 < num)
            {
                ch = JsonText[num2];
                if (ch != 0x5c)
                {
                    builder.Append(ch);
                }
                else if (num2 < (num - 5) && JsonText[num2 + 1] == 0x75)
                {
                    num3 = HexToInt(JsonText[num2 + 2]);
                    num4 = HexToInt(JsonText[num2 + 3]);
                    num5 = HexToInt(JsonText[num2 + 4]);
                    num6 = HexToInt(JsonText[num2 + 5]);
                    if (num3 < 0 || num4 < 0 | num5 < 0 || num6 < 0)
                    {
                        builder.Append(ch);
                    }
                    else
                    {
                        ch = (char)((((num3 << 12) | (num4 << 8)) | (num5 << 4)) | num6);
                        num2 += 5;
                        builder.Append(ch);
                    }
                }
                else if (num2 < (num - 3) && JsonText[num2 + 1] == 0x78)
                {
                    num7 = HexToInt(JsonText[num2 + 2]);
                    num8 = HexToInt(JsonText[num2 + 3]);
                    if (num7 < 0 || num8 < 0)
                    {
                        builder.Append(ch);
                    }
                    else
                    {
                        ch = (char)((num7 << 4) | num8);
                        num2 += 3;
                        builder.Append(ch);
                    }
                }
                else
                {
                    if (num2 < (num - 1))
                    {
                        ch2 = JsonText[num2 + 1];
                        if (ch2 == 0x5c)
                        {
                            builder.Append(@"\");
                            num2 += 1;
                        }
                        else if (ch2 == 110)
                        {
                            builder.Append("\n");
                            num2 += 1;
                        }
                        else if (ch2 == 0x74)
                        {
                            builder.Append("\t");
                            num2 += 1;
                        }
                    }
                    builder.Append(ch);
                }
                num2 += 1;
            }
            return builder.ToString();
        }

        public static string EncodeJSString(this string sInput)
        {
            StringBuilder builder;
            string str;
            char ch;
            int num;
            builder = new StringBuilder(sInput);
            builder.Replace(@"\", @"\\");
            builder.Replace("\r", @"\r");
            builder.Replace("\n", @"\n");
            builder.Replace("\"", "\\\"");
            str = builder.ToString();
            builder = new StringBuilder();
            num = 0;
            while (num < str.Length)
            {
                ch = str[num];
                if (0x7f >= ch)
                {
                    builder.Append(ch);
                }
                else
                {
                    builder.AppendFormat(@"\u{0:X4}", (int)ch);
                }
                num += 1;
            }
            return builder.ToString();
        }

        private static int HexToInt(char h)
        {
            if (h < 0x30 || h > 0x39)
            {
                if (h < 0x61 || h > 0x66)
                {
                    if (h < 0x41 || h > 0x46)
                    {
                        return -1;
                    }
                    return ((h - 0x41) + 10);
                }
                return ((h - 0x61) + 10);
            }
            return (h - 0x30);
        }
    }
}
