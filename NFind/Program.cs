

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var options = BuildOptions(args);
            if (options.HelpMode)
            {
                Console.WriteLine("");
                return;
            }

            var sources = FindSources(options.Path, options.SkipOfflineFiles);
            foreach (var source in sources )
            {
                var finder = GetStringFinder(source, options);

                var next = finder.Next();
                while (next != null) {
                    Print(source, next, options.ShowLineNumber);

                    next = finder.Next();
                }
            }
        }

        private static void Print(ILineSource source, Line next, bool showLineNumber)
        {
            if (showLineNumber)
            {
                Console.WriteLine($"[{next.LineNumber}] {next.Text}");
            }
            else
            {
                Console.WriteLine(next.Text);
            }
        }

        private static IStringFinder GetStringFinder(ILineSource source, FindOptions options)
        {
            return new LineSourceStringFinder(source, options.StringToFind, options.CaseSensitive, options.FindContainingLines);
        }

        private static ILineSource[] FindSources(string path, bool skipOfflineFiles)
        {
            if (string.IsNullOrEmpty(path))
            {
                return [new ConsoleLineSource()];
            }
            else
            {
                var files = Directory.GetFiles(path);
                if (files.Length > 0)
                {
                    if (skipOfflineFiles)
                    {
                        files = files.Where(f => new FileInfo(f).Attributes.HasFlag(FileAttributes.Offline) == false).ToArray();
                    }

                    return files.Select(f => new FileLineSource(f)).ToArray();
                }
                else
                {
                    return [];
                }
            }
        }

        private static FindOptions BuildOptions(string[] args)
        {
            FindOptions options = new FindOptions();

            foreach (string arg in args)
            {
                if ("/v".Equals(arg))
                {
                    options.FindContainingLines = false; 
                }
                else if ("/c".Equals(arg))
                {
                    options.CountMode = true;
                }
                else if ("/n".Equals(arg))
                {
                    options.ShowLineNumber = true;
                }
                else if ("/i".Equals(arg))
                {
                    options.CaseSensitive = false;
                }
                else if ("/?".Equals(arg))
                {
                    options.HelpMode = true;
                }
                else if ("/off".Equals(arg) || "/offline".Equals(arg))
                {
                    options.SkipOfflineFiles = false;
                }
                else
                {
                    if (string.IsNullOrEmpty(options.StringToFind))
                    {
                        options.StringToFind = arg;
                    }
                    else if (string.IsNullOrEmpty(options.Path))
                    {
                        options.Path = arg;
                    }
                    else
                    {
                        options.HelpMode = true;
                    }
                }
            }

            if (string.IsNullOrEmpty(options.StringToFind))
            {
                options.HelpMode = true;
            }

            return options;
        }
    }
}
