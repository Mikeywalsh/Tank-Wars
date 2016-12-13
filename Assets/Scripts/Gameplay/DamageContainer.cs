using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A struct used to pass information when dealing damage, such as the source of the damage and the weapon used
/// </summary>
public struct DamageContainer
{
    /// <summary>
    /// The tank dealing the damage, null if environmental damage
    /// </summary>
    public Tank Source;
    /// <summary>
    /// The weapon being used to deal the damage
    /// </summary>
    public Equipment Weapon;
    /// <summary>
    /// The amount of damage being dealt
    /// </summary>
    public int DamageAmount;

    public DamageContainer(Tank source, Equipment weapon)
    {
        Source = source;
        Weapon = weapon;
        DamageAmount = Random.Range(weapon.MinDamage(), weapon.MaxDamage() + 1);
    }
}
