using UnityEngine;

public class Warrior : CharacterClass
{
    private void Awake()
    {
        className = "Warrior";
        moveSpeedModifier = 0.8f;
        attackSpeedModifier = 1.2f;
    }

    public override void PerformSpecialAbility()
    {
        Debug.Log("Warrior performs a powerful slash!");
    }
}
