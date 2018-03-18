using System.Collections;
using System.Collections.Generic;
using Tower;
using UnityEngine;

namespace Bloon {
    public class YellowBloon : StandardBloon {

        protected override void Start() {
            bloonEnum = GameControl.BloonSpawner.Bloons.YellowBloon;
            bloonChildToSpawn = GameControl.BloonSpawner.Bloons.GreenBloon;
            base.Start();
        }
    }
}

