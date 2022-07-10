using Microsoft.Xna.Framework;

namespace DigitalRain.Config
{
    using Raindrop;

    public class ColorCalculatorFactory
    {
        private readonly Color _startColor;
        private readonly Color _endColor;
        private readonly float _lerpTime;

        public ColorCalculatorFactory(Color startColor, Color endColor, float lerpTime)
        {
            _startColor = startColor;
            _endColor = endColor;
            _lerpTime = lerpTime;
        }

        public ColorCalculator Create(double timespan)
        {
            return new ColorCalculator(timespan, _startColor, _endColor, _lerpTime);
        }
    }
}
