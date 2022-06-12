﻿
namespace DigitalRain
{
    public readonly struct ColumnId
    {
        private readonly int _id;
        public ColumnId(int id)
        {
            _id = id;
        }

        public override string ToString()
        {
            return $"{base.ToString()}({_id})";
        }
    }
}
