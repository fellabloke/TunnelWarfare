using System;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    public BaseGameState currentState;
    public static GameFlowManager Instance { get; private set; }

    private Dictionary<GameState, BaseGameState> allStates;
    [SerializeField] private BaseGameState[] statesToRegister;

    public event Action<GameState> stateChanged;

    public GameState CurrentStateType
    {
        get
        {
            if (currentState == null) return GameState.None;
            return currentState.StateType;
        }
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        allStates = new Dictionary<GameState, BaseGameState>();
        foreach (BaseGameState state in statesToRegister)
        {
            if (state == null) continue;

            state.Initialise(this);
            allStates.Add(state.StateType, state);
            state.gameObject.SetActive(false); 
        }
    }

    public void ChangeState(GameState newState)
    {
        if (!allStates.ContainsKey(newState))
        {
            Debug.LogError($"State {newState} is not in the dictionary!");
            return;
        }   
        if (currentState != null)
        {
            currentState.gameObject.SetActive(false);
        }

        currentState = allStates[newState];
        currentState.gameObject.SetActive(true);
        stateChanged?.Invoke(newState);
    }
}
