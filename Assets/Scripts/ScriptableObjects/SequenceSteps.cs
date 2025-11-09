using UnityEngine;

[CreateAssetMenu(fileName = "SequenceStep", menuName = "Scriptable Objects/Sequence_Step")]
public class SequenceStep : ScriptableObject
{
	public string type = "";
    public virtual void OnEnter(SkirmishContext context) { Debug.Log(this.name + type + "PhaseStarted"); }
	public virtual void OnUpdate(SkirmishContext context) { Debug.Log(this.name + type + "PhaseStarted"); }
	public virtual void OnExit(SkirmishContext context) { Debug.Log(this.name+ type + "PhaseEnded"); }
}
