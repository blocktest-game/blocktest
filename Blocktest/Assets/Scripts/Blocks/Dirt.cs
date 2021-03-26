using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dirt : Block {
    public override void Initialize()
    {
        blockName = "Dirt";
        blockID = 0;
        blockSmoothing = true;
        base.Initialize();
    }
}
