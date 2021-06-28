/// <summary> Handles incoming attacks </summary>
/// <remarks> You can test for this with <code>gameObject.GetComponent<Attackable>()</code> </remarks>
public interface Attackable
{
    /// <summary> Handles being attacked </summary>
    public void recieveAttack(Attack hit);
}
