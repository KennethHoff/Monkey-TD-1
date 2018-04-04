using System.Collections;
using System.Collections.Generic;
using Tower;
using UnityEngine;

namespace Bloon {
    public class BlueBloon : StandardBloon {

        protected override void Start() {
            bloonEnum = GameControl.BloonSpawner.Bloons.BlueBloon;

            /*
            startArmor = 0;
            childrenAmount = 1;
            RBE = 2;
            */
            base.Start();
        }
    }
}

