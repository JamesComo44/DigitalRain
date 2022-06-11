using System;

namespace DigitalRain
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new DigitalRainGame())
                game.Run();
        }
    }
}
