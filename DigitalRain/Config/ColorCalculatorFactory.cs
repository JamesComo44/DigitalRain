using Microsoft.Xna.Framework;

namespace DigitalRain.Config
{
    using Raindrop;

    public class ColorCalculatorFactory
    {
        private readonly Color _startColor;
        public Color EndColor { get; private set; }
        public float LerpTime { get; private set; }

        public ColorCalculatorFactory(Color startColor, Color endColor, float lerpTime)
        {
            _startColor = startColor;
            EndColor = endColor;
            LerpTime = lerpTime;
        }

        public ColorCalculator Create(double timespan)
        {
            return new ColorCalculator(timespan, _startColor, EndColor, LerpTime);
        }
    }
}
