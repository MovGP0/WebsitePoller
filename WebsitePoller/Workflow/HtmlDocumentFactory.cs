using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace WebsitePoller.Workflow
{
    public static class HtmlDocumentFactory
    {
        public static async Task<HtmlDocument> FromFileAsync(string cachedFilePath, CancellationToken cancellationToken = default(CancellationToken))
        {
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