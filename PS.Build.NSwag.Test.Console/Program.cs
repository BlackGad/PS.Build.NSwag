using System.Reflection;
using PS.Build.NSwag.Attributes;

//Swagger client generation
[assembly: StandaloneClient("Test4", "PS.Build.NSwag.Test.Console", "http://petstore.swagger.io/v2/swagger.json")]
[assembly: CleanOutputDirectory]

namespace PS.Build.NSwag.Test.Console
{
    [PartialClient]
    class Program
    {
        #region Static members

        static void Main(string[] args)
        {
            var tst = new Test4
            {
                BaseUrl = "ss"
            };
            var s = Assembly.GetEntryAssembly().GetCustomAttributes();
            System.Console.ReadLine();
        }

        #endregion
    }
}