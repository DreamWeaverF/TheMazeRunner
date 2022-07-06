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
                return GenerateCustomProject(path, content, @"Codes\Client\**\*.cs",new string[2] { "MessagePack", "MessagePack.Annotations" });
            }
            
            if (path.EndsWith("Common.csproj"))
            {
                content =  content.Replace("<Compile Include=\"Assets\\Codes\\Common\\Empty.cs\" />", string.Empty);
                content =  content.Replace("<None Include=\"Assets\\Codes\\Common\\Common.asmdef\" />", string.Empty);
                return GenerateCustomProject(path, content, @"Codes\Common\**\*.cs", new string[2] { "MessagePack", "MessagePack.Annotations" });
            }
            
            if (path.EndsWith("Server.csproj"))
            {
                content =  content.Replace("<Compile Include=\"Assets\\Codes\\Server\\Empty.cs\" />", string.Empty);
                content =  content.Replace("<None Include=\"Assets\\Codes\\Server\\Server.asmdef\" />", string.Empty);
                return GenerateCustomProject(path, content, @"Codes\Server\**\*.cs", new string[3] { "MessagePack", "MessagePack.Annotations" , "MySql.Data" });
            }
            return content;
        }

        private static string GenerateCustomProject(string path, string content, string codesPath, string[] nugetList)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(content);

            var newDoc = doc.Clone() as XmlDocument;

            var rootNode = newDoc.GetElementsByTagName("Project")[0];

            var itemGroup = newDoc.CreateElement("ItemGroup", newDoc.DocumentElement.NamespaceURI);
            var compile = newDoc.CreateElement("Compile", newDoc.DocumentElement.NamespaceURI);
            compile.SetAttribute("Include", codesPath);
            itemGroup.AppendChild(compile);

            if(nugetList != null && nugetList.Length > 0)
            {
                for(int i = 0; i < nugetList.Length; i++)
                {
                    var nugetRef = newDoc.CreateElement("Reference", newDoc.DocumentElement.NamespaceURI);
                    nugetRef.SetAttribute("Include", nugetList[i]);

                    var hintPath = newDoc.CreateElement("HintPath", newDoc.DocumentElement.NamespaceURI);
                    hintPath.InnerText = $"Nuget\\{nugetList[i]}.dll";
                    nugetRef.AppendChild(hintPath);

                    itemGroup.AppendChild(nugetRef);
                }
            }
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