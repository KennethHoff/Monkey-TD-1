using System.Collections;
using System.Collections.Generic;
using Tower;
using UnityEngine;

namespace Bloon {
    public class GreenBloon : StandardBloon {

        protected override void Start() {
            bloonEnum = GameControl.BloonSpawner.Bloons.GreenBloon;

            /*
            startArmor = 0;
            childrenAmount = 1;
            RBE = 3;
            */
            base.Start();
        }
    }
}

