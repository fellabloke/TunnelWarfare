using UnityEngine;

public class DefencePrepState : BaseGameState
{
    public override GameState StateType => GameState.DefencePrep;
    public DefencePrepState(GameFlowManager manager) : base(manager) { }

    [Header("Items To Create")]
    public GameObject baseToInstantiate;
    private GameObject currentBase;
    // public GameObject temp;

    public GameObject initialUI;
    // private GameObject[] cardPlaceSlots;
    
    void OnEnable()
    {
        InitialiseDefence();

        Debug.Log("Entering Defence State");
    }

    void Update()
    {
        Debug.Log("Updating Defence State");
    }
    void OnDisable()
    {
        initialUI.SetActive(false);

        Debug.Log("Exiting Defence State");
    }

    void InitialiseDefence()
    {
        currentBase = Instantiate(baseToInstantiate);
        DefenceUISelector initialUIMarker = currentBase.GetComponentInChildren<DefenceUISelector>();
        initialUI = initialUIMarker.gameObject;
    }
}
