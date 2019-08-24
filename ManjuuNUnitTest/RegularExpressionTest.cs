using System.Text.RegularExpressions;
using ManjuuDomain.Tools;
using NUnit.Framework;

namespace ManjuuNUnitTest
{
    public class RegularExpressionTest
    {
        [Test]
        public void MatchIpAddress()
        {
            Match ipMatch = Regex.Match("正在 Ping 127.0.0.1 具有 32 字节的数据:", UsualRegularExp.IPADRESS);
            Assert.IsTrue(ipMatch.Success);
            string ip = ipMatch.Value;
            Assert.AreEqual(ip, "127.0.0.1");
        }

        [Test]
        public void MatchHostNotfound()
        {
            string notfoundMsg = "Ping 请求找不到主机 928.929.1.1。请检查该名称，然后重试。";
            string testIp = "928.929.1.1";
            bool isMatch = Regex.IsMatch(notfoundMsg, $"找不到主机(\\s+){testIp}");
            Assert.IsTrue(isMatch);
        }

        [Test]
        public void MatchFormatError()
        {
            string formatError1 = "选项 -xx 不正确。";
            string formatError2 = "选项 -n 的值有错误，有效范围从 1 到 4294967295。";
            bool isMatch1 = Regex.IsMatch(formatError1, "选项(\\s)+([\\w\\W])+不正确");
            bool isMatch2 = Regex.IsMatch(formatError2, "选项(\\s)+([\\w\\W])+错误");
            //Bad option -c.
            //Bad value for option -n, valid range is from 1 to 4294967295.
            bool isMatch3 = Regex.IsMatch("Bad option -c.", "Bad(\\s)+option");
            bool isMatch4 = Regex.IsMatch("Bad value for option -n, valid range is from 1 to 4294967295.", "Bad(\\s)+([\\w\\W])+option");
            Assert.IsTrue(isMatch1);
            Assert.IsTrue(isMatch2);
            Assert.IsTrue(isMatch3);
            Assert.IsTrue(isMatch4);
        }

         [Test]
        public void MatchLossRate()
        {
            string msg = "    数据包: 已发送 = 4，已接收 = 0，丢失 = 4 (2323299.899999% 丢失)，";
            Match lossRateMatch = Regex.Match(msg,"[\\s\\w\\W]+\\(([\\d.]+)%[\\s\\w\\W]+\\)");
            Assert.IsTrue(lossRateMatch.Success);
            Assert.AreEqual(2323299.899999.ToString(), lossRateMatch.Groups[1].Value);
        }

         [Test]
        public void MatchResponseTime()
        {
            string msg = "    最短 = 12ms，最长 = 10ms，平均 = 9ms";
            Match timeMatch = Regex.Match(msg,@"[^\d]+=[^\d]+(\d+)ms[,，][^\d]+=[^\d]+(\d+)ms[,，][^\d]+=[^\d]+(\d+)ms");

            Assert.IsTrue(timeMatch.Success);
            Assert.AreEqual(12.ToString(), timeMatch.Groups[1].Value);
            Assert.AreEqual(10.ToString(), timeMatch.Groups[2].Value);
            Assert.AreEqual(9.ToString(), timeMatch.Groups[3].Value);
        }


         [Test]
        public void MatchNetDomain()
        {
            string msg = "正在 Ping www.a.shifen.com [14.215.177.38] 具有 32 字节的数据:";
            Match timeMatch = Regex.Match(msg,$"({UsualRegularExp.NET_DOMAIN})");

            Assert.IsTrue(timeMatch.Success);
            Assert.AreEqual("www.a.shifen.com", timeMatch.Groups[1].Value);
        }
    }
}