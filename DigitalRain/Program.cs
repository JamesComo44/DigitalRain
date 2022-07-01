using System;

namespace DigitalRain
{
    public static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var configFlagIndex = Array.FindIndex(args, (arg) => arg == "--config");
            var configFileIndex = configFlagIndex + 1;
            try
            {
                var configFileName = args[configFileIndex];
                var configReader = new ConfigReader();
                var config = configReader.ReadConfig(configFileName);
                using (var game = new DigitalRainGame(config))
                    game.Run();
            }
            catch (IndexOutOfRangeException)
            {
                throw new Exception("Must specify a filename after --config");
            }
        }
    }
}
