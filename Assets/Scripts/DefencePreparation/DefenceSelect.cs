using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEngine.XR;

public class DefenceSelect : MonoBehaviour
{
    [Header("UI Slots")]
    public GameObject[] drawSlotContainers;

    [Header("Defence Data")]
    public DefenceData[] defencePool;
    public List<GameObject> defencesDrawn;
    public List<GameObject> defencesPlaced;

    [Tooltip("Prefabs for card & placeholder")]
    public GameObject cardUIPrefab;

    [Header("UI Buttons")]
    public Button refresh;
    public Button lockIn;

    [Header("Canvas Elements")]
    public Camera defenceCamera;
    public static event Action defencesRefreshed;

    [Header("Drag Logic")]
    [SerializeField] private GameObject cardBeingDragged;
    [SerializeField] private GameObject defenceToBePlacedPlaceholder;
    public LayerMask groundLayer;
    public LayerMask dropLayer;
    public LayerMask cardLayer;

    private GameObject instantiatedPlaceholder;
    private Vector3 cardOriginalPosition;
    private bool isOverValidDropZone;
    private float dragDistance;

    [Header("Placement")]
    [SerializeField] private GameObject defenceToBePlaced;

    private float overlapRadius = 2f;

    DefenceCardDisplay cardDisplay;
    DefenceData data;
    DefenceSlot defenceSlot;
    GameFlowManager gameFlowManager;

    void Start()
    {
        gameFlowManager = GameFlowManager.Instance;

        if (refresh != null)
        {
            refresh.onClick.AddListener(RefreshDraw);
        }
        if (lockIn != null)
        {
            lockIn.onClick.AddListener(LockInDraw);
        }

        DrawNewDefences();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = defenceCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 500f, cardLayer))
            {
                cardBeingDragged = hit.collider.gameObject;
                cardOriginalPosition = hit.transform.position;
                dragDistance = Vector3.Distance(defenceCamera.transform.position, cardBeingDragged.transform.position);

                cardBeingDragged.GetComponent<Collider>().enabled = false;
                
                cardDisplay = cardBeingDragged.GetComponent<DefenceCardDisplay>();
                data = cardDisplay.defenceData;
                defenceToBePlacedPlaceholder = data.thisObjectPlaceholder;
                instantiatedPlaceholder = Instantiate(defenceToBePlacedPlaceholder, cardBeingDragged.transform);
            }
        }

        if (cardBeingDragged != null)
        {
            Ray ray = defenceCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 500f, groundLayer))
            {
                cardBeingDragged.transform.position = hit.point;

                Collider[] hits = Physics.OverlapSphere(cardBeingDragged.transform.position, overlapRadius, dropLayer);
                if (hits.Length > 0)
                {
                    Collider slotCollider = hits[0];
                    DefenceSlot slotScript = slotCollider.GetComponent<DefenceSlot>();
                    if (slotScript != null && slotScript.defenceInSlot == null)
                    {
                        isOverValidDropZone = true;
                        cardBeingDragged.transform.position = hits[0].transform.position;
                        defenceSlot = slotScript;
                    }
                    else
                    {
                        isOverValidDropZone = false;
                        defenceSlot = null;
                    }
                    
                }
                else
                {
                    isOverValidDropZone = false;
                    defenceSlot = null;
                }

            }
            else
            {
                isOverValidDropZone = false;
                defenceSlot = null;
            }
        }

        if (Input.GetMouseButtonUp(0) && cardBeingDragged != null)
        {
            if (isOverValidDropZone && defenceSlot != null)
            {
                defenceToBePlaced = data.thisObject;
                GameObject newDefence = Instantiate(defenceToBePlaced, cardBeingDragged.transform.position, Quaternion.identity);

                defencesPlaced.Add(newDefence);

                defenceSlot.defenceInSlot = newDefence;

                Destroy(cardBeingDragged);
                
            }
            else
            {
                cardBeingDragged.transform.position = cardOriginalPosition;
            }

            Cleanup(); 
        }
    }

    #region Card UI
    public void RefreshDraw()
    {
        ClearDrawSlots();
        DrawNewDefences();
        defencesRefreshed?.Invoke();
    }

    public void LockInDraw()
    {
        gameFlowManager.ChangeState(GameState.AttackPrep);
    }

    void ClearDrawSlots()
    {
        cardUIPrefab = null;
        foreach (GameObject slot in drawSlotContainers)
        {
            foreach (Transform child in slot.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    void DrawNewDefences()
    {
        if (defencePool.Length == 0)
        {
            Debug.LogError("Troop Pool is empty! Can't draw any troops.");
            return;
        }

        foreach (GameObject slot in drawSlotContainers)
        {
            DefenceData randomDefenceData = defencePool[UnityEngine.Random.Range(0, defencePool.Length)];

            cardUIPrefab = randomDefenceData.thisObjectCard;
            GameObject newCard = Instantiate(cardUIPrefab, slot.transform);
            defencesDrawn.Add(newCard);

            DefenceCardDisplay cardDisplay = newCard.GetComponent<DefenceCardDisplay>();
            if (cardDisplay != null)
            {
                cardDisplay.Setup(randomDefenceData);
            }
            else
            {
                Debug.LogWarning("Your cardUIPrefab is missing the 'DefenceCardDisplay' script!");
            }
        }
    }
    #endregion

    #region Dragging

    void Cleanup()
    {
        if (instantiatedPlaceholder != null)
        {
            Destroy(instantiatedPlaceholder);
            instantiatedPlaceholder = null;
        }

        if (cardBeingDragged != null)
        {
            cardBeingDragged.GetComponent<Collider>().enabled = true;
        }
        
        cardBeingDragged = null;
        data = null;
        cardDisplay = null;
        isOverValidDropZone = false;
        defenceSlot = null;
    }
    
    #endregion
}