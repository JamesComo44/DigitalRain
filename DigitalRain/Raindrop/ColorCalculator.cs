using Microsoft.Xna.Framework;
using System;

namespace DigitalRain.Raindrop
{
    public class ColorCalculator
    {
        private static int _maxAlpha = 255;

        private readonly double _timespan;
        private readonly Color _lerpStartColor;
        private readonly Color _lerpEndColor;
        private readonly float _lerpTime;
        public Color StartColor { get { return _lerpStartColor; } }

        public ColorCalculator(double timespan, Color startColor, Color endColor, float lerpTime)
        {
            // Could separate further into an AlphaLerper and a ColorLerper.
            // But for now we just add blank lines to show the two responsibilties XD.
            _timespan = timespan;

            _lerpStartColor = startColor;
            _lerpEndColor = endColor;
            _lerpTime = lerpTime;
        }

        public Color Calculate(double timeRemaining)
        {
            var timeElapsed = Math.Min(_timespan - timeRemaining, _timespan);
            var timePercentage = timeElapsed / _timespan;

            var lerpTimeElapsed = (float)Math.Min(timeElapsed, _lerpTime);
            var lerpPercentage = lerpTimeElapsed / _lerpTime;
            Color color = Color.Lerp(_lerpStartColor, _lerpEndColor, lerpPercentage);

            color.A = (byte)((1 - timePercentage) * _maxAlpha);

            return color;
        }
    }
}
