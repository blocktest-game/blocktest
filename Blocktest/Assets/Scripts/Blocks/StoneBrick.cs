using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneBrick : Block {
    public override void Initialize()
    {
        blockName = "Stone Brick";
        blockID = 7;
        blockSmoothing = true;
        base.Initialize();
    }
}
