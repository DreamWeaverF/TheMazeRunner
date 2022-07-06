using UnityEditor;
using System.Xml;
using System.IO;

namespace TheMazeRunner
{
    public class OnGenerateCSProjectProcessor : AssetPostprocessor
    {
        public static string OnGeneratedCSProject(string path, string content)
        {
            if (path.EndsWith("Client.csproj"))
            {
                content =  content.Replace("<Compile Include=\"Assets\\Codes\\Client\\Empty.cs\" />", string.Empty);
                content =  content.Replace("<None Include=\"Assets\\Codes\\Client\\Client.asmdef\" />", string.Empty);
                return GenerateCustomProject(path, content, @"Codes\Client\**\*.cs");
            }
            
            if (path.EndsWith("Common.csproj"))
            {
                content =  content.Replace("<Compile Include=\"Assets\\Codes\\Common\\Empty.cs\" />", string.Empty);
                content =  content.Replace("<None Include=\"Assets\\Codes\\Common\\Common.asmdef\" />", string.Empty);
                return GenerateCustomProject(path, content, @"Codes\Common\**\*.cs");
            }
            
            if (path.EndsWith("Server.csproj"))
            {
                content =  content.Replace("<Compile Include=\"Assets\\Codes\\Server\\Empty.cs\" />", string.Empty);
                content =  content.Replace("<None Include=\"Assets\\Codes\\Server\\Server.asmdef\" />", string.Empty);
                return GenerateCustomProject(path, content, @"Codes\Server\**\*.cs");
            }
            return content;
        }

        private static string GenerateCustomProject(string path, string content, string codesPath)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(content);

            var newDoc = doc.Clone() as XmlDocument;

            var rootNode = newDoc.GetElementsByTagName("Project")[0];

            var itemGroup = newDoc.CreateElement("ItemGroup", newDoc.DocumentElement.NamespaceURI);
            var compile = newDoc.CreateElement("Compile", newDoc.DocumentElement.NamespaceURI);

            compile.SetAttribute("Include", codesPath);
            itemGroup.AppendChild(compile);

            //var projectReference = newDoc.CreateElement("ProjectReference", newDoc.DocumentElement.NamespaceURI);
            //projectReference.SetAttribute("Include", @"..\Share\Analyzer\Share.Analyzer.csproj");
            //projectReference.SetAttribute("OutputItemType", @"Analyzer");
            //projectReference.SetAttribute("ReferenceOutputAssembly", @"false");

            //var project = newDoc.CreateElement("Project", newDoc.DocumentElement.NamespaceURI);
            //project.InnerText = @"{d1f2986b-b296-4a2d-8f12-be9f470014c3}";
            //projectReference.AppendChild(project);

            //var name = newDoc.CreateElement("Name", newDoc.DocumentElement.NamespaceURI);
            //name.InnerText = "Analyzer";
            //projectReference.AppendChild(project);

            //itemGroup.AppendChild(projectReference);

            rootNode.AppendChild(itemGroup);

            using (StringWriter sw = new StringWriter())
            {

                using (XmlTextWriter tx = new XmlTextWriter(sw))
                {
                    tx.Formatting = Formatting.Indented;
                    newDoc.WriteTo(tx);
                    tx.Flush();
                    return sw.GetStringBuilder().ToString();
                }
            }
        }
    }
}