public class Stone : Block
{
    public override void Initialize()
    {
        blockName = "Stone";
        blockID = 2;
        blockSmoothing = true;
        base.Initialize();
    }
}
