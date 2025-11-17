using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq; 

public class SkirmishOverseer : MonoBehaviour
{
    public List<GameObject> initialTroopOrder;

    private Queue<GameObject> _battleQueue;
    private SkirmishContext _battleContext;

    void Start()
    {
        _battleQueue = new Queue<GameObject>();
        foreach (GameObject troop in initialTroopOrder)
        {
            if (troop != null)
            {
                _battleQueue.Enqueue(troop);
            }
        }

        _battleContext = new SkirmishContext(this);
        StartCoroutine(RunFullSequence());
    }

    IEnumerator RunFullSequence()
    {
        Debug.Log("Battle starting...");
        int safetyBreak = 0; 

        while (!_battleContext.IsBattleOver && safetyBreak < 200)
        {
            if (_battleQueue.Count == 0)
            {
                Debug.Log("All troops defeated. Battle over.");
                _battleContext.IsBattleOver = true;
                break;
            }
            if (_battleContext.GetFlag("ARBITRARY_GOAL_REACHED"))
            {
                 Debug.Log("Goal reached. Battle over.");
                 _battleContext.IsBattleOver = true;
                 break;
            }

            GameObject currentStepObject = _battleQueue.Dequeue();
            if (currentStepObject == null) continue; 

            ISequenceObjects currentStep = currentStepObject.GetComponent<ISequenceObjects>();
            SequenceObject runner = currentStepObject.GetComponent<SequenceObject>();

            if (currentStep == null || runner == null)
            {
                Debug.LogWarning($"{currentStepObject.name} is missing scripts, skipping.");
                continue;
            }

            if (runner.IsDefeated)
            {
                Debug.Log($"{currentStepObject.name} is already defeated, skipping.");
                continue; 
            }
            
            Debug.Log($"Turn: {currentStepObject.name}");

            yield return StartCoroutine(currentStep.Execute(this, _battleContext));

            if (!runner.IsDefeated)
            {
                _battleQueue.Enqueue(currentStepObject);
                Debug.Log($"{currentStepObject.name} turn ended, re-queuing.");
            }
            else
            {
                Debug.Log($"{currentStepObject.name} defeated, removing from queue.");
            }

            safetyBreak++;
            if (safetyBreak >= 200) Debug.LogError("Safety break triggered. Check for infinite loop.");
        }

        Debug.Log("Battle finished.");
    }
    
    public SequenceObject GetRandomTarget(SequenceObject self)
    {
        var allTargets = _battleQueue
            .Select(go => go.GetComponent<SequenceObject>())
            .Where(r => r != null && !r.IsDefeated && r != self)
            .ToList();

        if (allTargets.Count > 0)
        {
            return allTargets[Random.Range(0, allTargets.Count)];
        }

        return null; 
    }
}