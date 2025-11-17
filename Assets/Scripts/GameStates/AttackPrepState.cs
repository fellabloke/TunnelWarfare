using Unity.Cinemachine;
using UnityEngine;

public class AttackPrepState : BaseGameState
{
    public override GameState StateType => GameState.AttackPrep;
    public AttackPrepState(GameFlowManager manager) : base(manager) { }

    [Header("Items To Create")]
    public GameObject attackObjectToInstantiate;
    private GameObject currentAttackObject;
    public GameObject initialUI;

    void OnEnable()
    {
        InitialiseAttackState();

        Debug.Log("Entering Attack State");
    }

    void Update()
    {
        Debug.Log("Updating Attack State");
    }

    void OnDisable()
    {
        Debug.Log("Exiting Attack State");
    }

    void InitialiseAttackState()
    {
        currentAttackObject = Instantiate(attackObjectToInstantiate);
        AttackUISelector initialUIMarker = currentAttackObject.GetComponentInChildren<AttackUISelector>();
        initialUI = initialUIMarker.gameObject;
    }
    
}
