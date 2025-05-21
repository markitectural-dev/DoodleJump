using System;

namespace Model.Core
{
    public struct ScoreValue
    {
        public int Value { get; }

        public ScoreValue(int value)
        {
            Value = Math.Max(0, value); 
        }

        public static ScoreValue operator +(ScoreValue a, ScoreValue b)
        {
            return new ScoreValue(a.Value + b.Value);
        }

        public static ScoreValue operator *(ScoreValue score, float multiplier)
        {
            return new ScoreValue((int)(score.Value * multiplier));
        }

        public static implicit operator int(ScoreValue score)
        {
            return score.Value;
        }
    }
}