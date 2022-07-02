using System;

namespace DigitalRain
{
    public static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            int configFlagIndex = Array.FindIndex(args, (arg) => arg == "--config");
            int configFileIndex = configFlagIndex + 1;
            try
            {
                string configFileName = args[configFileIndex];
                var config = ConfigReader.FromJson(configFileName);
                using var game = new DigitalRainGame(config);
                game.Run();
            }
            catch (IndexOutOfRangeException)
            {
                throw new Exception("Must specify a filename after --config");
            }
        }
    }
}
