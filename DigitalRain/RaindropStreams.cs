using System.Collections.Generic;

namespace DigitalRain
{
    public class RaindropStream
    {
        float _xPosition;
        float _width;

        public RaindropStream(float xPosition, float width)
        {
            _xPosition = xPosition;
            _width = width;
        }

        public void Update()
        { }

        public void Draw()
        { }
    }

    public class RaindropStreamFactory
    {
        private IUnoccupiedColumnPool _columnPool;
        private float _screenWidth;
        private float _streamSpacing;

        public RaindropStreamFactory(IUnoccupiedColumnPool columnPool, int screenWidth, int streamSpacing = 0)
        {
            _columnPool = columnPool;
            _screenWidth = screenWidth;
            _streamSpacing = streamSpacing;
        }

        public RaindropStream create()
        {
            var columnNumber = _columnPool.PickOne();
            var xPosition = CalculateXPosition(columnNumber);
            return new RaindropStream(xPosition, StreamWidth);
        }

        private float ColumnWidth
        {
            get
            {
                return _screenWidth / _columnPool.ColumnCount;
            }
        }

        private float StreamWidth
        {
            get
            {
                return ColumnWidth - _streamSpacing;
            }
        }

        private float CalculateXPosition(int columnNumber)
        {
            return columnNumber * ColumnWidth;
        }
    }

    /**
     * Just a Composite class.
     */
    public class RaindropStreams
    {
        private List<RaindropStream> _streams;

        public RaindropStreams()
        {
            _streams = new List<RaindropStream>();
        }

        public List<RaindropStream> Streams { get { return _streams; } }

        public void Add(RaindropStream stream)
        {
            _streams.Add(stream);
        }

        public void Update()
        {
            foreach (var stream in _streams)
            {
                stream.Update();
            }
        }

        public void Draw()
        {
            foreach (var stream in _streams)
            {
                stream.Draw();
            }
        }
    }
}