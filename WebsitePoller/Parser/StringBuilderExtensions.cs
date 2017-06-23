using System.Text;

namespace WebsitePoller.Parser
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder AppendParameter(this StringBuilder builder, string name, string value, bool isFirst = false)
        {
            if (!isFirst) builder.Append("&");
            return builder.AppendFormat("tx_sozaltbau_pi1[{0}]={1}", name, value);
        }
    }
}