using System.Text.RegularExpressions;

namespace SkDoc
{

    public class Reader
    {
        public List<SkDoc> allFunctions = new List<SkDoc>();
        private String regexComment = "^#\\*[\\s\\S]*#\\*$";
        private String regexWhole = "^#(?:\\*[\\s\\S]*?\\*)?\\s*function\\s*\\w*\\([^)]*\\):";
        private String regexFunctionName = "function\\s+(\\w+)\\s*\\(";

        public SkDoc? ReadFile(string filePath)
        {
            string? line;
            string? file = "";
            try
            {

                StreamReader sr = new StreamReader(filePath);

                line = sr.ReadLine();
                file += line;

                while (line != null) {
                    line = sr.ReadLine();
                    file += line;
                }

                foreach (Match match in Regex.Matches(file, regexWhole))
                {
                    Console.WriteLine(match.Value);
                    String functionMatch = Regex.Match(match.Value, regexFunctionName).Value;
                    String functionName = functionMatch.Replace("function", "").Replace("(", "");
                    Console.WriteLine(match.Value);
                    String functionDefinition = Regex.Match(match.Value, regexComment).Value;

                    Console.WriteLine("[SKDOCAPI]" + functionDefinition);

                    SkDoc skDoc = new SkDoc(functionName, functionDefinition, filePath, getLine(filePath, functionName));
                    if (match.Success && match.Groups.Count > 0)
                    {
                        allFunctions.Add(skDoc);
                        return skDoc;
                    }
                }

            }
            catch (Exception e) { Console.WriteLine("[SkDoc API] ERROR! " + e); return null; }

            return null;
        }

        private String? getLine(string filePath, string functionName)
        {
            var lines = File.ReadAllLines(filePath);
            for(int i = 1; i < lines.Length + 1; i++) {
                if (lines[i].Contains(functionName)) return i.ToString();
            }

            return null;
        }
    }

    public class SkDoc
    {
        string functionName;
        string functionDefinition;
        string functionPath;
        string functionLine;

        public SkDoc(string functionName, string functionDefinition, string functionPath, string functionLine)
        {
            this.functionName = functionName;
            this.functionDefinition = functionDefinition;
            this.functionPath = functionPath;
            this.functionLine = functionLine;
        }

        public string GetFunctionName()
        {
            return functionName;
        }

        public string GetFunctionDefinition()
        {
            return functionDefinition;
        }

        public string GetFunctionPath()
        {
            return functionPath;
        }

        public string GetFunctionLine()
        {
            return functionLine;
        }
    }
}
