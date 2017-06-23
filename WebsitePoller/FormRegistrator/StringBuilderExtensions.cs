using System;
using System.Text;
using JetBrains.Annotations;
using RestSharp.Extensions.MonoHttp;

namespace WebsitePoller.FormRegistrator
{
    public static class StringBuilderExtensions
    {
        [NotNull]
        public static StringBuilder AppendParameter(
            [NotNull]this StringBuilder builder, 
            [NotNull]string name, 
            [NotNull]string value, 
            bool isFirst = false)
        {
            if(builder == null) throw new ArgumentNullException(nameof(builder));
            if(name == null) throw new ArgumentNullException(nameof(name));
            if(string.IsNullOrWhiteSpace(name)) throw new ArgumentException("May not be a white space.", nameof(name));
            if(value == null) throw new ArgumentNullException(nameof(value));
            
            var result = AppendParameterInternal(builder, name, value, isFirst);
            return result;
        }

        [NotNull]
        private static StringBuilder AppendParameterInternal(
            [NotNull] this StringBuilder builder,
            [NotNull] string name, 
            [NotNull] string value, 
            bool isFirst)
        {
            var encodedValue = HttpUtility.UrlEncode(value);
            if (!isFirst) builder.Append("&");
            return builder.AppendFormat("tx_sozaltbau_pi1[{0}]={1}", name, encodedValue);
        }
    }
}