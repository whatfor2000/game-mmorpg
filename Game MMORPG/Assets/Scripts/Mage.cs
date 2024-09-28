using UnityEngine;

public class Mage : CharacterClass
{
    private void Awake()
    {
        className = "Mage";
        moveSpeedModifier = 1f;
        attackSpeedModifier = 0.8f;
    }

    public override void PerformSpecialAbility()
    {
        Debug.Log("Mage casts a powerful spell!");
    }
}
