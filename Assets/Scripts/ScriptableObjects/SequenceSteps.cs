using UnityEngine;

[CreateAssetMenu(fileName = "SequenceStep", menuName = "Scriptable Objects/Sequence_Step")]
public class SequenceStep : ScriptableObject
{
    public virtual void OnEnter(SkirmishContext context) {
		// Debug.Log(name + "Enter PhaseStarted"); 
	}
	public virtual void OnUpdate(SkirmishContext context) {
		// Debug.Log(name + "Update PhaseStarted"); 
		
	}
	public virtual void OnExit(SkirmishContext context) {
		// Debug.Log(name + "Exit PhaseEnded"); 
	}
}
