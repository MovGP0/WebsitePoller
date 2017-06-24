using System;
using Windows.Data.Xml.Dom;
using JetBrains.Annotations;
using Microsoft.Toolkit.Uwp.Notifications;

namespace WebsitePoller.Workflow
{
    public static class NotificationMessageFactory
    {
        [NotNull]
        public static XmlDocument GetToastXml([NotNull]string message, [NotNull]Uri url)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (url == null) throw new ArgumentNullException(nameof(url));

            var toastContent = new ToastContent
            {
                Visual = new ToastVisual
                {
                    BindingGeneric = new ToastBindingGeneric
                    {
                        Children =
                        {
                            new AdaptiveText
                            {
                                Text = "Website Poller",
                                HintMaxLines = 1
                            },
                            new AdaptiveText
                            {
                                Text = message
                            },
                            new AdaptiveText
                            {
                                Text = url.ToString()
                            }
                        }
                    }
                }
            };
            var content = toastContent.GetContent();

            var toastXml = new XmlDocument();
            toastXml.LoadXml(content);
            return toastXml;
        }
    }
}