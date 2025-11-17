using UnityEngine;

public class SceneInitialiser : MonoBehaviour
{
    public GameState sceneState;

    void Start()
    {
       if (GameFlowManager.Instance != null)
        {
            GameFlowManager.Instance.ChangeState(sceneState);
        } 
    }
}
