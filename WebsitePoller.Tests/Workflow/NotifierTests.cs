using System;
using System.Threading;
using Windows.UI.Notifications;
using NSubstitute;
using NUnit.Framework;
using Serilog;
using WebsitePoller.Workflow;

namespace WebsitePoller.Tests.Workflow
{
    [TestFixture]
    public sealed class NotifierTests
    {
        public sealed class Constructor
        {
            [Test]
            public void ShouldThrowArgumentNullException()
            {
                Assert.That(() => new Notifier(null, x => new ToastNotification(x)), Throws.ArgumentNullException);
            }

            [Test]
            public void ShouldThrowArgumentNullException2()
            {
                var toastNotifier = Substitute.For<IToastNotifier>();
                Assert.That(() => new Notifier(toastNotifier, null), Throws.ArgumentNullException);
            }

            [Test]
            public void ShouldAllowMocking()
            {
                var toastNotifier = Substitute.For<IToastNotifier>();
                Assert.That(() => new Notifier(toastNotifier, x => new ToastNotification(x)), Throws.Nothing);
            }
        }

        public sealed class Notify
        {
            [Test]
            public void ShouldThrowArgumentNullExceptions()
            {
                var toastNotifier = Substitute.For<IToastNotifier>();
                var notifier = new Notifier(toastNotifier, x => new ToastNotification(x));

                Assert.Multiple(() =>
                {
                    Assert.That(() => notifier.Notify(null, new Uri("http://localhost/")), Throws.ArgumentNullException, "message");
                    Assert.That(() => notifier.Notify("message", null), Throws.ArgumentNullException, "uri");
                });
            }

            [Test]
            public void ShouldShowMessagen()
            {
                var toastNotifier = Substitute.For<IToastNotifier>();
                var notifier = new Notifier(toastNotifier, x => new ToastNotification(x));
                notifier.Notify("message", new Uri("http://localhost/"));

                Assert.That(() => toastNotifier.Received().Show(Arg.Any<ToastNotification>()), Throws.Nothing, "did not show a toast notification");
            }

            [Test]
            public void ShouldLogDismissedNotification()
            {
                var logger = Substitute.For<ILogger>();
                var notifierLogger = Substitute.For<ILogger>();
                logger.ForContext<Notifier>().Returns(notifierLogger);
                Log.Logger = logger;

                var innerToastNotifier = ToastNotificationManager.CreateToastNotifier("Website Poller");
                var toastNotifier = new ToastNotifierWrapper(innerToastNotifier);
                ToastNotification toast = null;
                var notifier = new Notifier(toastNotifier, x =>
                {
                    toast = new ToastNotification(x);
                    return toast;
                });

                // act
                notifier.Notify("message", new Uri("http://localhost/"));
                innerToastNotifier.Hide(toast);

                Thread.Sleep(1000);

                // assert
                Assert.That(() =>
                {
                    notifierLogger.Received(1).Debug(Arg.Any<string>());
                }, Throws.Nothing, "logged hide");
            }
        }
    }
}
