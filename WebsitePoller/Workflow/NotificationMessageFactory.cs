using System;
using Windows.Data.Xml.Dom;
using Microsoft.Toolkit.Uwp.Notifications;

namespace WebsitePoller.Workflow
{
    public static class NotificationMessageFactory
    {
        public static XmlDocument GetToastXml(string message, Uri url)
        {
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