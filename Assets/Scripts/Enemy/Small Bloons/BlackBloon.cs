using System.Collections;
using System.Collections.Generic;
using Tower;
using UnityEngine;

namespace Bloon {
    public class BlackBloon : StandardBloon {

        protected override void Start() {
            bloonEnum = GameControl.BloonSpawner.Bloons.BlackBloon;

            /*
            startArmor = 0;
            childrenAmount = 2;
            RBE = 11;
            */
            base.Start();
        }
    }
}

