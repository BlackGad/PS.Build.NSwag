using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using PS.Build.Services;
using PS.Build.Types;

namespace PS.Build.NSwag.Attributes
{
    [AttributeUsage(AttributeTargets.Assembly)]
    [Designer("PS.Build.Adaptation")]
    public class CleanOutputDirectoryAttribute : Attribute
    {
        private readonly string[] _generationSpecificFiles;

        #region Constructors

        public CleanOutputDirectoryAttribute()
        {
            _generationSpecificFiles = new[]
            {
                "NJsonSchema",
                "NJsonSchema.CodeGeneration",
                "NJsonSchema.CodeGeneration.CSharp",
                "NSwag.CodeGeneration",
                "NSwag.CodeGeneration.CSharp",
                "NSwag.Core",
                "PS.Build.NSwag",
            };
            _generationSpecificFiles = _generationSpecificFiles.Select(v => v.ToLowerInvariant())
                                                               .ToArray();
        }

        #endregion

        #region Members

        private void PostBuild(IServiceProvider provider)
        {
            var logger = (ILogger)provider.GetService(typeof(ILogger));
            var explorer = (IExplorer)provider.GetService(typeof(IExplorer));

            logger.Info("Cleaning NSwag related files from output directory");
            var targetDirectory = explorer.Directories[BuildDirectory.Target];
            foreach (var file in Directory.EnumerateFiles(targetDirectory))
            {
                var fileName = Path.GetFileNameWithoutExtension(file)?.ToLowerInvariant();
                if (!_generationSpecificFiles.Contains(fileName)) continue;

                try
                {
                    File.Delete(file);
                    logger.Debug("File " + file + " deleted.");
                }
                catch (Exception e)
                {
                    logger.Warn("Could not delete " + file + ". Details: " + e.GetBaseException().Message);
                }
            }
        }

        #endregion
    }
}