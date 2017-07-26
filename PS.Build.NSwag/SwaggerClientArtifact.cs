using System;
using System.IO;
using System.Text;
using NSwag;
using NSwag.CodeGeneration.CSharp;
using PS.Build.Services;
using PS.Build.Types;

namespace PS.Build.NSwag
{
    class SwaggerClientArtifact
    {
        private readonly string _className;
        private readonly string _ns;

        #region Constructors

        public SwaggerClientArtifact(string className, string ns)
        {
            if (className == null) throw new ArgumentNullException(nameof(className));
            if (ns == null) throw new ArgumentNullException(nameof(ns));
            _className = className;
            _ns = ns;
        }

        #endregion

        #region Members

        public string GetCachedPath(IExplorer explorer)
        {
            return Path.Combine(explorer.Directories[BuildDirectory.Intermediate],
                                "SwaggerClients",
                                Math.Abs(_className.GetHashCode()) + Math.Abs(_ns.GetHashCode()) + ".cs");
        }

        public byte[] GetContent(string documentPath)
        {
            var settings = new SwaggerToCSharpClientGeneratorSettings
            {
                ClassName = _className,
                CSharpGeneratorSettings =
                {
                    Namespace = _ns
                }
            };

            var document = SwaggerDocument.FromFileAsync(documentPath).Result;

            var generator = new SwaggerToCSharpClientGenerator(document, settings);
            var code = generator.GenerateFile();
            return Encoding.UTF8.GetBytes(code);
        }

        #endregion
    }
}