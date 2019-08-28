using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ManjuuCommon.Tools
{
    public static class StringHelper
    {
        #region 去除字符串空格，null时返回string.Empty

        /// <summary>
        /// 去除字符串空格，null时返回string.Empty
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string StringWithTrim(string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                return input.Trim();
            }
            return string.Empty;
        }
        #endregion

        #region 去除字符串所有空格
        public static string TrimAll(string input)
        {
            if (string.IsNullOrEmpty(input)) { return string.Empty; }

            const string reg = @"[\s]";
            return Regex.Replace(input, reg, string.Empty);
        }
        #endregion

        #region 字符转全角
        /// <summary>
        /// 字符转全角
        /// 全角空格码12288，半角空格码32
        /// 其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        /// </summary>
        /// <param name="originString">源字符串</param>
        /// <returns></returns>
        public static string ToSBC(string originString)
        {
            if (string.IsNullOrWhiteSpace(originString))
            {
                return string.Empty;
            }


            char[] targetChr = originString.ToCharArray();

            for (int i = 0; i < targetChr.Length; i++)
            {
                if (32 == targetChr[i])
                {
                    targetChr[i] = (char)12288; //半角空格转全角
                    continue;
                }

                if (targetChr[i] > 32 && targetChr[i] < 127)
                {
                    targetChr[i] = (char)((int)targetChr[i] + 65248);
                }
            }


            return new string(targetChr); //返回转换后的字符串

        }
        #endregion

        #region 字符转半角
        /// <summary>
        /// 字符转半角
        /// 全角空格码12288，半角空格码32
        /// 其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        /// </summary>
        /// <param name="originString">源字符串</param>
        /// <returns></returns>
        public static string ToDBC(string originString)
        {
            if (string.IsNullOrWhiteSpace(originString))
            {
                return string.Empty;
            }

            char[] targetChr = originString.ToCharArray();

            for (int i = 0; i < targetChr.Length; i++)
            {
                if (12288 == targetChr[i])
                {
                    targetChr[i] = (char)32;
                    continue;
                }

                if (targetChr[i] > 65280 && targetChr[i] < 65375)
                {
                    targetChr[i] = (char)((int)targetChr[i] - 65248);
                }

            }


            return new string(targetChr);


        }
        #endregion

    }
}
