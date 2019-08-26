namespace ManjuuCommon.Tools
{
    /// <summary>
    /// 常用正则表达式
    /// </summary>
    public class UsualRegularExp
    {
        /// <summary>
        /// IP地址
        /// </summary>
        /// <returns></returns>
        public const string IPADRESS=@"((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})(\.((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})){3}";


        /// <summary>
        /// 网络域名匹配
        /// </summary>
        /// <returns></returns>
        public const string NET_DOMAIN = @"([0-9a-zA-Z][0-9a-zA-Z-]{0,62}\.)+([0-9a-zA-Z][0-9a-zA-Z-]{0,62})\.?";
        
        
    }
}