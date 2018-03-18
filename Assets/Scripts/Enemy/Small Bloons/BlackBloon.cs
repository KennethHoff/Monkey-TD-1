using System.Collections;
using System.Collections.Generic;
using Tower;
using UnityEngine;

namespace Bloon {
    public class BlackBloon : StandardBloon {

        protected override void Start() {
            bloonEnum = GameControl.BloonSpawner.Bloons.BlackBloon;
            bloonChildToSpawn = GameControl.BloonSpawner.Bloons.PinkBloon;
            amountOfBloonsToSpawn = 2;
            base.Start();
        }
    }
}

