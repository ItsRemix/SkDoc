using System.Text.RegularExpressions;

namespace SkDocAPI
{
    /// <summary>
    /// Class <c>Reader</c> is used to read files.
    /// </summary>
    public class Reader
    {
        /// <summary>
        /// List of <c>SkDoc</c> functions that reader has ever read.
        /// </summary>
        public List<SkDoc> allFunctions = new List<SkDoc>();
        private String regexComment = "#\\*([\\s\\S]*?)#\\*";
        private String regexWhole = "^#(?:\\*[\\s\\S]*?\\*)?\\s*function\\s*\\w*\\([^)]*\\):";
        private String regexFunctionName = "function\\s+(\\w+)\\s*\\(";

        /// <summary>
        /// Method <c>ReadFile</c> is used to read file and get it <c>SkDoc</c>, it also adds files to <c>allFunctions</c> list that can be get with <Reader>.allFunctions
        /// </summary>
        /// <param name="filePath">Param with file path. Example: C:\\skript\\test.sk</param>
        /// <returns><c>SkDoc?</c></returns>
        public SkDoc? ReadFile(string filePath)
        {
            string? file = string.Join(string.Empty, File.ReadAllLines(filePath));
            try
            {

                foreach (Match match in Regex.Matches(file, regexWhole))
                {
                    string functionMatch = Regex.Match(match.Value, regexFunctionName).Value;
                    string functionName = functionMatch.Replace("function", "").Replace("(", "");
                    string functionDefinition = Regex.Match(match.Value, regexComment).Value;

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

        private string? getLine(string filePath, string functionName)
        {
            var lines = File.ReadAllLines(filePath);
            for(int i = 1; i < lines.Length + 1; i++) {
                if (lines[i].Contains(functionName)) return (i+1).ToString();
            }

            return null;
        }
    }

    /// <summary>
    /// Represents SkDoc. Filled with functionName, functionDefinition, functionPath, functionLine
    /// </summary>
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

        public string GetCuteFunctionDefinition() {
            return functionDefinition.Replace("#", "\n#").Replace("#*", "").Replace("#","");
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
