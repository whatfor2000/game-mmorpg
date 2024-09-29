using UnityEngine;

public class Warrior : CharacterClass
{
    private void Awake()
    {
        // Initialize the class attributes for Warrior
        InitializeClassAttributes("Warrior", 0.8f, 1.2f);
    }

    public override void PerformSpecialAbility()
    {
        Debug.Log("Warrior performs a powerful slash!");
        // Implement ability logic here

        // Call to synchronize the class if needed
        SynchronizeClassChange();
    }
}
