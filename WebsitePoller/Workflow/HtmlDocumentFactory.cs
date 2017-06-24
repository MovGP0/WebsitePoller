using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using JetBrains.Annotations;

namespace WebsitePoller.Workflow
{
    public static class HtmlDocumentFactory
    {
        [NotNull]
        public static async Task<HtmlDocument> FromFileAsync([NotNull]string cachedFilePath, CancellationToken cancellationToken = default(CancellationToken))
        {
            if(cachedFilePath == null) throw new ArgumentNullException(nameof(cachedFilePath));
            if (!File.Exists(cachedFilePath)) throw new FileNotFoundException("The file was not found.", cachedFilePath);

            cancellationToken.ThrowIfCancellationRequested();

            var cachedWebpage = new HtmlDocument();
            using (var stream = File.OpenRead(cachedFilePath))
            {
                cachedWebpage.Load(stream, Encoding.UTF8);
                await stream.FlushAsync(cancellationToken);
            }
            return cachedWebpage;
        }
    }
}