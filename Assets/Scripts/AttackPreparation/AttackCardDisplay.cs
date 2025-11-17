using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class AttackCardDisplay : MonoBehaviour
{
    public TroopData troopData;

    public void Setup(TroopData data)
    {
        troopData = data;

        if (troopData == null)
        {
            Debug.LogError("DefenceData is null on this card.");
            return;
        }
    }

    public void OnCardClicked()
    {
        Debug.Log("Selected troop: " + troopData.TroopName);
    }
}