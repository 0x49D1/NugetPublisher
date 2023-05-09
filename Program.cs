// Go to all subdirectories of current directory and find all latest versions of files with extension .nupkg. After finding - try to publish to your nuget server. PAT is: test
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace NugetPublish
{
    class Program
    {
        static async Task Main(string[] args)
        {
            DotNetEnv.Env.TraversePath().Load(".env.nugetpublisher");
            string apiKey = Environment.GetEnvironmentVariable("NugetApiKey");
            string host = Environment.GetEnvironmentVariable("NugetHost");
            string packagePattern = Environment.GetEnvironmentVariable("RegexPatternOfPackages");
            if (string.IsNullOrEmpty(apiKey))
                throw new Exception("NugetApiKey is not set. Recheck that you have `.env.nugetpublisher` env file somewhere near the executable");
            if (string.IsNullOrEmpty(host))
                throw new Exception("NugetHost is not set");
            if (string.IsNullOrEmpty(packagePattern))
                throw new Exception("RegexPatternOfPackages is not set");

            Console.WriteLine("Searching for .nupkg files...");
            var files = Directory.GetFiles(Environment.CurrentDirectory, "*.nupkg", SearchOption.AllDirectories)
                .Select(f => new FileInfo(f))
                .GroupBy(f => Path.GetFileNameWithoutExtension(f.Name).TrimEnd("0123456789.".ToCharArray()), (name, fileInfos) => fileInfos.OrderByDescending(f => f.LastWriteTime).First());

            Console.WriteLine("Found files: " + files.Count());

            foreach (var file in files)
            {
                if (!Regex.IsMatch(file.ToString(), packagePattern) || file.ToString().Contains("packages")) // probably was not packed by us
                {
                    Console.WriteLine("Project is not to be procese.* Skipping...");
                    continue;
                }

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c dotnet nuget push {file} --source {host} --api-key {apiKey}"
                };
                Process process = new Process();
                process.StartInfo = startInfo;
                process.Start();
                Console.WriteLine(file + " is publishing...");
            }

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
