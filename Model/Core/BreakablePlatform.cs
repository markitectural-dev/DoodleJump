using System;
using System.Drawing;

namespace Model.Core {
    public class BreakablePlatform : Platform {
        public override Color Color => Color.Orange;
        public override bool IsBreakable => true;
        public BreakablePlatform(float x, float y) : base(x, y) {}

        public override void Bounce(Player player) {
            player.Jump();
            IsActive = false; // deactivate platform after use
        }
    }
}