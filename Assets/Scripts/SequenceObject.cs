using UnityEngine;
using System.Collections;

public class SequenceObject : MonoBehaviour, ISequenceObjects
{
    public TroopData TroopData; 
	
	public float prepareDuration = 3.0f;
	public float abilityDuration = 1.0f;
	public float coreDuration = 2.0f;
	public float deathDuration = 1.0f;
	
	private SkirmishContext _context;
	private bool _isActionInterrupted = false;
    private SequenceStep _interruptAction = null;

    public IEnumerator Execute(SkirmishOverseer manager, SkirmishContext context)
	{
		_context = context;
		
		yield return StartCoroutine(RunPhase(TroopData.PrepareAction, prepareDuration));
		
		yield return StartCoroutine(RunPhase(TroopData.AbilityAction, abilityDuration));
		
		yield return StartCoroutine(RunPhase(TroopData.CoreAction, coreDuration));
		
		yield return StartCoroutine(RunPhase(TroopData.DeathAction, deathDuration));
		
		Debug.Log($"--- Step {name} Finished ---");
	}
	
	private IEnumerator RunPhase(SequenceStep action, float maxDuration)
	{
		_isActionInterrupted = false; 
		_interruptAction = null;
		
		yield return StartCoroutine(RunActionWithVariableTime(action, maxDuration));
		
		if (_isActionInterrupted && _interruptAction != null)
		{
			Debug.LogWarning($"Phase was interrupted. Running interrupt action: {_interruptAction.name}");
			
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
			if (_isActionInterrupted)
			{
				break;
			}
			
			action.OnUpdate(_context);
			
			timer += Time.deltaTime;
			yield return null;
		}
		
		action.OnExit(_context);
	}
	
	// void OnTriggerEnter(Collider other)
	// {
	// 	TriggerData triggerData = other.GetComponent<TriggerData>();
	// 	if (triggerData != null)
	// 	{
	// 		Debug.Log($"TRIGGER HIT: Collided with {other.name}!");
	// 		_interruptAction = triggerData.ActionToRunOnTrigger;
	// 		_isActionInterrupted = true;
	// 	}
	// }
}
