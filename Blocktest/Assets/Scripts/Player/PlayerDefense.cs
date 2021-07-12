using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefense : MonoBehaviour, Attackable
{
    [SerializeField]
    private int _health = 100;

    public int Health
    {
        get { return _health; }
    }

    public void recieveAttack(Attack hit)
    {
        _health -= hit.damage;
    }

}
