using UnityEngine;
using Unity.Cinemachine;
using System;
public class Cinemachine : MonoBehaviour
{
    public CinemachineCamera attackPrepCam;
    public CinemachineCamera defencePrepCam;
    private CinemachineCamera currentActiveCam;

    private const int priorityOn = 10;
    private const int priorityOff = 0;

    GameFlowManager gameFlowManager;

    void Awake()
    {
        gameFlowManager = GameFlowManager.Instance;

        if (defencePrepCam != null) defencePrepCam.Priority = priorityOff;
        if (attackPrepCam != null) attackPrepCam.Priority = priorityOff;
    }
    void OnEnable()
    {
        gameFlowManager.stateChanged += GameStates;
    }

    void GameStates(GameState state)
    {
        if (currentActiveCam != null)
        {
            currentActiveCam.Priority = priorityOff;
        }

        CinemachineCamera newCamToActivate = null;
        switch (state)
        {
            case GameState.DefencePrep:
                newCamToActivate = defencePrepCam;
                break;
            case GameState.AttackPrep:
                newCamToActivate = attackPrepCam;
                break;
        }

        if (newCamToActivate != null)
        {
            newCamToActivate.Priority = priorityOn;
        }
        
        currentActiveCam = newCamToActivate;

    }

    void Cleanup()
    {
        attackPrepCam.Priority = priorityOff;
        defencePrepCam.Priority = priorityOff;
    }
}
