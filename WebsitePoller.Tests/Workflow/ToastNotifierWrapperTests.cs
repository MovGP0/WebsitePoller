using System;
using Windows.UI.Notifications;
using NUnit.Framework;
using WebsitePoller.Workflow;

namespace WebsitePoller.Tests.Workflow
{
    [TestFixture]
    public sealed class ToastNotifierWrapperTests
    {
        public sealed class Constructor
        {
            [Test]
            public void ShouldThrowArgumentNullException()
            {
                Assert.That(() => new ToastNotifierWrapper(null), Throws.ArgumentNullException);
            }
        }

        public sealed class Show
        {
            [Test]
            public void ShouldThrowArgumentNullException()
            {
                var notifier = ToastNotificationManager.CreateToastNotifier("Website Poller");
                var wrapper = new ToastNotifierWrapper(notifier);
                Assert.That(() => wrapper.Show(null), Throws.ArgumentNullException);
            }

            [Test]
            public void ShouldNotThrowException()
            {
                var notifier = ToastNotificationManager.CreateToastNotifier("Website Poller");
                var wrapper = new ToastNotifierWrapper(notifier);

                var toastXml = NotificationMessageFactory.GetToastXml("foo", new Uri("http://localhost/"));
                var toast = new ToastNotification(toastXml);
                
                Assert.That(() => wrapper.Show(toast), Throws.Nothing);
                notifier.Hide(toast);
            }
        }
    }
}
