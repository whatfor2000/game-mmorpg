using UnityEngine;
public class Archer : CharacterClass
{
    private void Awake()
    {
        className = "Archer";
        moveSpeedModifier = 1.2f;
        attackSpeedModifier = 1f;
    }

    public override void PerformSpecialAbility()
    {
        Debug.Log("Archer fires a volley of arrows!");
    }
}