using System.Collections;
using System.Collections.Generic;
using Tower;
using UnityEngine;

namespace Bloon {
    public class PinkBloon : StandardBloon {

        protected override void Start() {
            bloonEnum = GameControl.BloonSpawner.Bloons.PinkBloon;

            /*
            startArmor = 0;
            childrenAmount = 1;
            RBE = 5;
            */
            base.Start();
        }
    }
}

