using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : Block {
    public override void Initialize()
    {
        blockName = "Wood";
        blockID = 3;
        blockSmoothing = true;
        base.Initialize();
    }
}
