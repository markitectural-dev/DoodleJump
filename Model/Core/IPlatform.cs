using System;
using System.Drawing;

namespace Model.Core
{
    public interface IPlatform
    {
        float X { get; }
        float Y { get; set; }
        float Width { get; }
        float Height { get; }
        bool IsActive { get; }
        Color Color { get; }
        bool IsBreakable { get; }
        
        // Method for platform behavior when player jumps on it
        void Bounce(Player player);
        
        // Method to update platform state
        void Update();
    }
}