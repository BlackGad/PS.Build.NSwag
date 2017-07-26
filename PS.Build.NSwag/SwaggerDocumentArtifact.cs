using System;
using System.IO;
using System.Net;
using PS.Build.Services;
using PS.Build.Types;

namespace PS.Build.NSwag
{
    class SwaggerDocumentArtifact
    {
        private readonly ILogger _logger;
        private readonly string _source;

        #region Constructors

        public SwaggerDocumentArtifact(ILogger logger, string source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            _logger = logger;
            _source = source;
        }

        #endregion

        #region Members

        public string GetCachedPath(IExplorer explorer)
        {
            return Path.Combine(explorer.Directories[BuildDirectory.Intermediate],
                                "SwaggerDocuments",
                                Math.Abs(_source.GetHashCode()) + ".json");
        }

        public byte[] GetContent()
        {
            var webClient = new WebClient();
            _logger.Debug("Downloading remote document");
            var data = webClient.DownloadData(_source);
            _logger.Debug("Swagger document " + _source + " cached");
            return data;
        }

        #endregion
    }
}