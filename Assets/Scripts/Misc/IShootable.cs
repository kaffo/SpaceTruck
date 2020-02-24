using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShootable 
{
    bool IsAlive();
    void DoDamage(float damageAmount);
}
