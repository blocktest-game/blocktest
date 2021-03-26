using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassPane : Block {
    public override void Initialize()
    {
        blockName = "Glass Pane";
        blockID = 5;
        blockSmoothing = true;
        base.Initialize();
    }
}
