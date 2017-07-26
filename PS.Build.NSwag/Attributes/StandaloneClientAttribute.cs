using System;
using System.ComponentModel;
using PS.Build.Services;
using PS.Build.Types;

namespace PS.Build.NSwag.Attributes
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    [Designer("PS.Build.Adaptation")]
    public class StandaloneClientAttribute : Attribute
    {
        private readonly string _className;
        private readonly string _ns;
        private readonly string _swaggerDocumentSource;

        #region Constructors

        public StandaloneClientAttribute(string className, string ns, string swaggerDocumentSource)
        {
            if (swaggerDocumentSource == null) throw new ArgumentNullException(nameof(swaggerDocumentSource));
            _swaggerDocumentSource = swaggerDocumentSource;
            _className = string.IsNullOrWhiteSpace(className) ? "MyClass" : className;
            _ns = string.IsNullOrWhiteSpace(ns) ? "MyNamespace" : ns;
        }

        #endregion

        #region Members

        private void PreBuild(IServiceProvider provider)
        {
            var logger = (ILogger)provider.GetService(typeof(ILogger));
            var artifactory = (IArtifactory)provider.GetService(typeof(IArtifactory));
            var explorer = (IExplorer)provider.GetService(typeof(IExplorer));

            logger.Info("Standalone swagger client generation started");
            try
            {
                var document = new SwaggerDocumentArtifact(logger, _swaggerDocumentSource);
                var documentPath = document.GetCachedPath(explorer);

                artifactory.Artifact(documentPath, BuildItem.None)
                           .Content(() => document.GetContent())
                           .Dependencies()
                           .TagDependency(_swaggerDocumentSource);
                logger.Debug("Swagger document artifact defined. Will be stored in: " + documentPath);

                var client = new SwaggerClientArtifact(_className, _ns);

                artifactory.Artifact(client.GetCachedPath(explorer), BuildItem.Compile)
                           .Content(() => client.GetContent(documentPath))
                           .Dependencies()
                           .TagDependency(_className)
                           .TagDependency(_ns)
                           .FileDependency(documentPath);
                logger.Debug("Swagger client compile artifact defined");
            }
            catch (Exception e)
            {
                logger.Error(e.GetBaseException().Message);
            }
        }

        #endregion
    }
}