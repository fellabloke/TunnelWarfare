using UnityEngine;

public class MainMenuState : BaseGameState
{
    public override GameState StateType => GameState.MainMenu;

    public MainMenuState(GameFlowManager manager) : base(manager) { }

    void OnEnable()
    {
        Debug.Log("Entering Menu State");
    }

    void Update()
    {
        Debug.Log("Updating Menu State");
    }
    void OnDisable()
    {

        Debug.Log("Exiting Menu State");
    }

}
