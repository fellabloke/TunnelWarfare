using UnityEngine;

public abstract class BaseGameState : MonoBehaviour 
{
    protected GameFlowManager _manager;

    public abstract GameState StateType { get; }

    public BaseGameState(GameFlowManager manager)
    {
        _manager = manager;
    }

   public virtual void Initialise(GameFlowManager manager) { _manager = manager; } 
}
