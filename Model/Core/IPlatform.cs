using System;
using System.Drawing;

namespace Model.Core
{
    public interface IPlatform
    {
        float X { get; }
        float Y { get; }
        float Width { get; }
        float Height { get; }
        bool IsActive { get; }
        Color Color { get; }
        bool IsBreakable { get; }
        
        void Bounce(Player player);
        
        void Update();
    }
}