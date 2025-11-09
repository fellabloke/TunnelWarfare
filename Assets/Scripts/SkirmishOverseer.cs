using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;

public class SkirmishOverseer : MonoBehaviour
{
    public bool isRoundOver;
    public Dictionary<string, object> _flags = new Dictionary<string, object>();
    public List<GameObject> sequenceObjectSteps;

    private SkirmishContext skirmishContext;

    private void Start()
    {
        skirmishContext = new SkirmishContext();

        StartCoroutine(RunFullSequence());
    }
    
    IEnumerator RunFullSequence()
    {
        Debug.Log("--- Round Starting ---");

        foreach (GameObject stepObject in sequenceObjectSteps)
        {
            ISequenceObjects currentStep = stepObject.GetComponent<ISequenceObjects>();

            if (currentStep != null)
            {
                Debug.Log($"--- Starting Step: {stepObject.name} ---");
                
                yield return StartCoroutine(currentStep.Execute(this, skirmishContext));

                if (skirmishContext.isRoundOver)
                {
                    Debug.Log("--- Round Over ---");
                    yield break;
                }
            }
            else
            {
                Debug.LogWarning($"GameObject '{stepObject.name}' is missing 'ISequenceStep'");
            }
        }
        
        Debug.Log("--- Entire Sequence Finished ---");
    }
}
