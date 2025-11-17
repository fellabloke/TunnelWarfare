using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class SequenceObject : MonoBehaviour, ISequenceObjects
{
    [Header("Troop Template")]
    public TroopData TroopData;

    public int CurrentHealth { get; private set; }
    public bool IsDefeated { get; private set; } = false;

    private SkirmishContext _context;
    private bool _isActionInterrupted = false;
    private SequenceStep _interruptAction = null; 

    public void TakeDamage(int amount)
    {
        if (IsDefeated) return; 

        CurrentHealth -= amount;
        Debug.Log($"{TroopData.TroopName} took {amount} damage, {CurrentHealth} HP remaining.");

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            IsDefeated = true;
            Debug.Log($"{TroopData.TroopName} defeated.");
        }
    }

    public IEnumerator Execute(SkirmishOverseer manager, SkirmishContext context)
    {
        if (TroopData == null)
        {
            Debug.LogError($"{TroopData.TroopName} is missing TroopData. Cannot execute turn.", this);
            yield break;
        }

        _context = context;

        if (CurrentHealth == 0 && !IsDefeated) 
        {
            CurrentHealth = TroopData.MaxHealth;
        }

        if (IsDefeated)
        {
             Debug.Log($"{name} is defeated, skipping turn.");
             yield break;
        }
        
        context.SetFlag("current_step_runner", this);
        context.SetFlag("current_troop_data", TroopData);
        
        yield return StartCoroutine(RunPhase(TroopData.PrepareAction, TroopData.PrepareDuration));
        
        if (!IsDefeated) 
            yield return StartCoroutine(RunPhase(TroopData.AbilityAction, TroopData.AbilityDuration));
        
        if (!IsDefeated)
            yield return StartCoroutine(RunPhase(TroopData.CoreAction, TroopData.CoreDuration));
        
        if (!IsDefeated)
            yield return StartCoroutine(RunPhase(TroopData.DeathAction, TroopData.DeathDuration));
        
        context.RemoveFlag("current_step_runner");
        context.RemoveFlag("current_troop_data");
    }

    private IEnumerator RunPhase(SequenceStep action, float maxDuration) 
    {
        _isActionInterrupted = false;
        _interruptAction = null;

        yield return StartCoroutine(RunActionWithVariableTime(action, maxDuration));
        
        if (_isActionInterrupted && _interruptAction != null)
        {
            Debug.LogWarning($"Phase interrupted. Running {_interruptAction.name}.");
            yield return StartCoroutine(RunActionWithVariableTime(_interruptAction, 1.5f));
        }
    }

    private IEnumerator RunActionWithVariableTime(SequenceStep action, float maxDuration) 
    {
        if (action == null) yield break;
        float timer = 0f;
        
        action.OnEnter(_context);
        while (timer < maxDuration)
        {
            if (IsDefeated || _isActionInterrupted) break; 
            
            action.OnUpdate(_context);
            timer += Time.deltaTime;
            yield return null;
        }
        action.OnExit(_context);
    }

    // void OnTriggerEnter(Collider other)
    // {
    //     TriggerData triggerData = other.GetComponent<TriggerData>();
    //     if (triggerData != null)
    //     {
    //         Debug.Log($"Triggered by {other.name}.");
    //         _interruptAction = triggerData.ActionToRunOnTrigger as SequenceSteps;
    //         _isActionInterrupted = true;
    //     }
    // }
}