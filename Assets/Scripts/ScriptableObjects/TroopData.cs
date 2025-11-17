using UnityEngine;

[CreateAssetMenu(fileName = "TroopData", menuName = "Scriptable Objects/TroopData")]
public class TroopData : ScriptableObject
{
    [Header("Basic Stats")]
    public string TroopName;
    public int MaxHealth;
    public int BaseDamage;
    public GameObject thisObjectPlaceholder;
    public GameObject thisObject;
    public GameObject thisObjectCard;
    
    [Header("Phase Actions & Durations")]
    public SequenceStep PrepareAction;
    public float PrepareDuration = 3.0f;
    
    public SequenceStep AbilityAction;
    public float AbilityDuration = 1.0f;
    
    public SequenceStep CoreAction;
    public float CoreDuration = 2.0f;
    
    public SequenceStep DeathAction;
    public float DeathDuration = 1.0f;
}
