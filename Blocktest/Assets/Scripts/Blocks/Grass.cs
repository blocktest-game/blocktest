using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : Block {
    public override void Initialize()
    {
        blockName = "Grass";
        blockID = 1;
        blockSmoothing = true;
        base.Initialize();
    }
}
