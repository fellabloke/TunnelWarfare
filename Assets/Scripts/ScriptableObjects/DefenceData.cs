using UnityEngine;

[CreateAssetMenu(fileName = "DefenceData", menuName = "Scriptable Objects/DefenceData")]
public class DefenceData : ScriptableObject
{
    [Header("Basic Stats")]
    public string DefenceName;
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
