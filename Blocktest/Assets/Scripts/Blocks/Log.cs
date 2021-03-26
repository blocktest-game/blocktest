using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : Block {
    public override void Initialize()
    {
        blockName = "Log";
        blockID = 6;
        blockSmoothing = true;
        base.Initialize();
    }
}
