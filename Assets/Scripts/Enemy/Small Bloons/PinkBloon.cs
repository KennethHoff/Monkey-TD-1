using System.Collections;
using System.Collections.Generic;
using Tower;
using UnityEngine;

namespace Bloon {
    public class PinkBloon : StandardBloon {

        protected override void Start() {
            bloonEnum = GameControl.BloonSpawner.Bloons.PinkBloon;
            bloonChildToSpawn = GameControl.BloonSpawner.Bloons.YellowBloon;
            base.Start();
        }
    }
}

