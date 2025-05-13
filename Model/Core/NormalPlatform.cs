using System;
using System.Drawing;

namespace Model.Core {
    public class NormalPlatform : Platform {
        public override Color Color => Color.Green;
        public override bool IsBreakable => false;

        public NormalPlatform(float x, float y) : base(x, y) { }

        public override void Bounce(Player player) {
            player.Jump();
        }
    }
}