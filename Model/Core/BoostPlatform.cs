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
        public float BoostMultiplier { get; private set; } = 2f; 

        public BoostPlatform(float x, float y) : base(x, y, 60, 25) {}
        public override void Bounce(Player player) {
            player.Jump(BoostMultiplier); 
        }
    }
}
