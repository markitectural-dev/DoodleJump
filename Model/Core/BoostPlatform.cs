using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Core {
    public class BoostPlatform : Platform {
        public override Color Color => Color.Blue; 
        public override bool IsBreakable => false;  
        public float BoostMultiplier { get; private set; } = 2f; // Jump height multiplier

        public BoostPlatform(float x, float y) : base(x, y) {}
        public override void Bounce(Player player) {
            player.Jump(BoostMultiplier); // jumps higher due to boost multiplier
        }
    }
}
