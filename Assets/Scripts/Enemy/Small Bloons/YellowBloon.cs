using System.Collections;
using System.Collections.Generic;
using Tower;
using UnityEngine;

namespace Bloon {
    public class YellowBloon : StandardBloon {

        protected override void Start() {
            bloonEnum = GameControl.BloonSpawner.Bloons.YellowBloon;
            amountOfBloonsToSpawn = 1;
            RBE = 4;
            base.Start();
        }
    }
}

