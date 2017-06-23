using System.Text;
using RestSharp.Extensions.MonoHttp;

namespace WebsitePoller.FormRegistrator
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder AppendParameter(this StringBuilder builder, string name, string value, bool isFirst = false)
        {
            var encodedValue = HttpUtility.UrlEncode(value);
            if (!isFirst) builder.Append("&");
            return builder.AppendFormat("tx_sozaltbau_pi1[{0}]={1}", name, encodedValue);
        }
    }
}