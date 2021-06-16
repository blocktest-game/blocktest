/// <summary> Handles incoming attacks </summary>
/// <remarks> You can test for this with <code>if(class is Attackable)</code> </remarks>
interface Attackable
{
    /// <summary> Handles being attacked </summary>
    public void recieveAttack(Attack hit);
}
