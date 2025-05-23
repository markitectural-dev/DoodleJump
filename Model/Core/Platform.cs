using System;
using System.Drawing;

namespace Model.Core {
    public abstract class Platform : IPlatform {
        public float X { get; protected set; }
        public float Y { get; protected set; }
        public float Width { get; protected set; }
        public float Height { get; protected set; }
        public bool IsActive { get; protected set; } = true;
        public abstract Color Color { get; }
        public abstract bool IsBreakable { get; }

        protected Platform(float x, float y, float width = 60, float height = 10) {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
        public abstract void Bounce(Player player);
    }
}