using UnityEngine;

public abstract class CharacterClass : MonoBehaviour
{
    public string className;
    public float moveSpeedModifier = 1f;
    public float attackSpeedModifier = 1f;

    public abstract void PerformSpecialAbility();
}




