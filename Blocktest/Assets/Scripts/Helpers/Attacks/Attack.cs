/// <summary> Stores information on an attacks effect on an attackable object </summary>
public struct Attack
{
    /// <summary> Amount of damage dealt </summary>
    public int damage;

    /// <summary> Type of damage dealt </summary>
    public byte type;

    public Attack(int newDamage, byte newType)
    {
        damage = newDamage;
        type = newType;
    }
}
