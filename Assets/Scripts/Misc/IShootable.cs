using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShootable 
{
    bool IsAlive();
    bool DoDamage(float damageAmount);
}
