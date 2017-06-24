using System.IO;
using HtmlAgilityPack;

namespace WebsitePoller.Tests.Parser
{
    public static class AnExtensions
    {
        public static HtmlNodeCollection TableRows(this IAn an)
        {
            var doc = new HtmlDocument();
            var assemblyPath = an.AssemblyPath();
            var path = Path.Combine(assemblyPath, "Parser", "table-rows.xml");

            using (var file = File.OpenRead(path))
            using (var fileReader = new StreamReader(file))
            {
                var line = string.Empty;
                while ((line = fileReader.ReadLine()) != null)
                {
                    var row = HtmlNode.CreateNode(line);
                    doc.DocumentNode.AppendChild(row);
                }
            }
            return doc.DocumentNode.ChildNodes;
        }
    }
}