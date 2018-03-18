using System.Collections;
using System.Collections.Generic;
using Tower;
using UnityEngine;

namespace Bloon {
    public class RedBloon : StandardBloon {

        protected override void Start() {
            bloonEnum = GameControl.BloonSpawner.Bloons.RedBloon;
            amountOfBloonsToSpawn = 0;
            RBE = 1;
            base.Start();
        }
    }
}

