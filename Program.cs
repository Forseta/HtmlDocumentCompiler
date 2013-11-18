using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NVelocity;
using NVelocity.App;

namespace DocumentionCompiler
{
    class Program
    {
        static void Main(string[] args)
        {

            string templateFile = args[0];
            string outDir = args[1];

            var model = BuildModel();

            //Load the template page
            var result = TransformTemplate(templateFile, model);

            //Save the output
            WriteResult(outDir, result);

        }

        public static TemplateModel BuildModel()
        {
            var model = new TemplateModel();
            return model;
        }

        public static void WriteResult(string outputDir, string result)
        {
            //Save output
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            string outputFile = Path.Combine(outputDir, "index.html");

            var file = new FileInfo(outputFile);
            file.Directory.Create();

            File.WriteAllText(outputFile, result);
        }

        private static string TransformTemplate(string templateFile, TemplateModel model)
        {

            Velocity.Init();

            var velocityContext = new VelocityContext();
            velocityContext.Put("model", model);

            string template = File.ReadAllText(templateFile);

            var sb = new StringBuilder();
            Velocity.Evaluate(
                velocityContext,
                new StringWriter(sb),
                "document template",
                new StringReader(template));

            return sb.ToString();
        }
    }

    public class TemplateModel
    {

        private readonly Helpers _helpers;

        public TemplateModel()
        {
            _helpers = new Helpers();
        }

        public Helpers Helpers {
            get { return _helpers; }
        }
    }

    public class Helpers
    {
        public string IncludeSource(string fileName)
        {
            string html = File.ReadAllText(fileName);
            return HttpUtility.HtmlEncode(html);
        }
    }

}
