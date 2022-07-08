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

            var config = LoadConfigFromJson(args[configFileIndex]);

            using var game = new DigitalRainGame(config);
            game.Run();
        }

        public static DigitalRainConfig LoadConfigFromJson(string filename)
        {
            try
            {
                return ConfigReader.FromJson(filename);
            }
            catch (IndexOutOfRangeException)
            {
                throw new Exception("Must specify a filename after --config");
            }
        }
    }
}
