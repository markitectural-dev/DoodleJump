using System;
using System.Drawing;

namespace Model.Core {
    public class TrickPlatform : Platform {
        // Properties
        public override Color Color => Color.Red;
        public override bool IsBreakable => true;
        public TrickPlatform(float x, float y) : base(x, y) {}

        public override void Bounce(Player player) {
            IsActive = false; // no bouncing; the platform deactivates after passing through
        }
    }
}