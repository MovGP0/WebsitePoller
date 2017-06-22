using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Serilog;

namespace WebsitePoller.Workflow
{
    public static class HtmlDocumentExtensions
    {
        private static ILogger Log => Serilog.Log.ForContext(typeof(HtmlDocumentExtensions));

        public static async Task SaveHtmlDocumentToFileWithLoggingAsync(this HtmlDocument htmlDocument, string targetPath, CancellationToken cancellationToken)
        {
            try
            {
                await SaveHtmlDocumentToFileAsync(htmlDocument, targetPath, cancellationToken);
            }
            catch (Exception e)
            {
                Log.Error(e, $"Could not save {nameof(htmlDocument)} to '{targetPath}'");
                throw;
            }
        }

        public static async Task SaveHtmlDocumentToFileAsync(this HtmlDocument htmlDocument, string targetPath, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var fileStream = File.OpenWrite(targetPath))
            {
                htmlDocument.Save(fileStream);
                await fileStream.FlushAsync(cancellationToken);
            }
        }
    }
}