using log4net;
using System;
using System.Globalization;

namespace OpenNos.Core.Bootstrapping
{
    public sealed class ConsoleStartupArguments
    {
        public bool IgnoreStartupMessages { get; set; }

        public bool IgnoreTelemetry { get; set; }

        public int? PortOverride { get; set; }
    }

    public sealed class ConsoleStartupOptions
    {
        public string Title { get; set; }

        public string BannerText { get; set; }

        public string CultureName { get; set; } = "en-US";
    }

    public static class ConsoleStartup
    {
        public static ConsoleStartupArguments ParseArguments(string[] args)
        {
            ConsoleStartupArguments parsed = new ConsoleStartupArguments();
            if (args == null || args.Length == 0)
            {
                return parsed;
            }

            int portArgIndex = Array.FindIndex(args, s => s == "--port");
            if (portArgIndex != -1
                && args.Length >= portArgIndex + 1
                && int.TryParse(args[portArgIndex + 1], out int portOverride))
            {
                parsed.PortOverride = portOverride;
            }

            foreach (string arg in args)
            {
                switch (arg)
                {
                    case "--nomsg":
                        parsed.IgnoreStartupMessages = true;
                        break;
                    case "--notelemetry":
                        parsed.IgnoreTelemetry = true;
                        break;
                }
            }

            return parsed;
        }

        public static void InitializeCulture(string cultureName)
        {
            string culture = string.IsNullOrWhiteSpace(cultureName) ? "en-US" : cultureName;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo(culture);
        }

        public static void InitializeLogger(Type type)
        {
            Logger.InitializeLogger(LogManager.GetLogger(type));
        }

        public static void SetConsoleTitle(string title)
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                Console.Title = title;
            }
        }

        public static void WriteBanner(string bannerText)
        {
            if (string.IsNullOrWhiteSpace(bannerText))
            {
                return;
            }

            int offset = (Console.WindowWidth / 2) + (bannerText.Length / 2);
            string separator = new string('=', Console.WindowWidth);
            Console.WriteLine(separator + string.Format("{0," + offset + "}\n", bannerText) + separator);
        }
    }
}
