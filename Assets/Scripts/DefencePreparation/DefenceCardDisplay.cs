using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class DefenceCardDisplay : MonoBehaviour
{
    public DefenceData defenceData;

    public void Setup(DefenceData data)
    {
        defenceData = data;

        if (defenceData == null)
        {
            Debug.LogError("DefenceData is null on this card.");
            return;
        }
    }

    public void OnCardClicked()
    {
        Debug.Log("Selected troop: " + defenceData.DefenceName);
    }
}