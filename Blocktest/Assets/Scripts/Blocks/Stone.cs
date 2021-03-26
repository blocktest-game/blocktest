using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : Block {
    public override void Initialize()
    {
        blockName = "Stone";
        blockID = 2;
        blockSmoothing = true;
        base.Initialize();
    }
}
