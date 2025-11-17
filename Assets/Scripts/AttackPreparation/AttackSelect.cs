using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class AttackSelect : MonoBehaviour
{
    [Header("UI Slots")]
    public GameObject[] drawSlotContainers;

    [Header("Troop Data")]
    public TroopData[] troopPool;
    public List<GameObject> troopsDrawn;
    public List<GameObject> troopsPlaced;

    [Tooltip("Prefabs for card & placeholder")]
    public GameObject cardUIPrefab;

    [Header("UI Buttons")]
    public Button refresh;

    [Header("Canvas Elements")]
    public Camera troopCamera;
    public static event Action troopsRefreshed;

    [Header("Drag Logic")]
    [SerializeField] private GameObject cardBeingDragged;
    [SerializeField] private GameObject troopToBePlacedPlaceholder;
    public LayerMask groundLayer;
    public LayerMask dropLayer;
    public LayerMask cardLayer;

    private GameObject instantiatedPlaceholder;
    private Vector3 cardOriginalPosition;
    private bool isOverValidDropZone;
    private float dragDistance;

    [Header("Placement")]
    [SerializeField] private GameObject troopToBePlaced;

    private float overlapRadius = 2f;

    AttackCardDisplay cardDisplay;
    TroopData data;
    AttackSlot troopSlot;

    void Start()
    {
        if (refresh != null)
        {
            refresh.onClick.AddListener(RefreshDraw);
        }

        DrawNewtroops();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = troopCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 500f, cardLayer))
            {
                cardBeingDragged = hit.collider.gameObject;
                cardOriginalPosition = hit.transform.position;
                dragDistance = Vector3.Distance(troopCamera.transform.position, cardBeingDragged.transform.position);

                cardBeingDragged.GetComponent<Collider>().enabled = false;
                
                cardDisplay = cardBeingDragged.GetComponent<AttackCardDisplay>();
                data = cardDisplay.troopData;
                troopToBePlacedPlaceholder = data.thisObjectPlaceholder;
                instantiatedPlaceholder = Instantiate(troopToBePlacedPlaceholder, cardBeingDragged.transform);
            }
        }

        if (cardBeingDragged != null)
        {
            Ray ray = troopCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 500f, groundLayer))
            {
                cardBeingDragged.transform.position = hit.point;

                Collider[] hits = Physics.OverlapSphere(cardBeingDragged.transform.position, overlapRadius, dropLayer);
                if (hits.Length > 0)
                {
                    Collider slotCollider = hits[0];
                    AttackSlot slotScript = slotCollider.GetComponent<AttackSlot>();
                    if (slotScript != null && slotScript.attackInSlot == null)
                    {
                        isOverValidDropZone = true;
                        cardBeingDragged.transform.position = hits[0].transform.position;
                        troopSlot = slotScript;
                    }
                    else
                    {
                        isOverValidDropZone = false;
                        troopSlot = null;
                    }
                    
                }
                else
                {
                    isOverValidDropZone = false;
                    troopSlot = null;
                }

            }
            else
            {
                isOverValidDropZone = false;
                troopSlot = null;
            }
        }

        if (Input.GetMouseButtonUp(0) && cardBeingDragged != null)
        {
            if (isOverValidDropZone && troopSlot != null)
            {
                troopToBePlaced = data.thisObject;
                GameObject newtroop = Instantiate(troopToBePlaced, cardBeingDragged.transform.position, Quaternion.identity);

                troopsPlaced.Add(newtroop);

                troopSlot.attackInSlot = newtroop;

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
        DrawNewtroops();
        troopsRefreshed?.Invoke();
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

    void DrawNewtroops()
    {
        if (troopPool.Length == 0)
        {
            Debug.LogError("Troop Pool is empty! Can't draw any troops.");
            return;
        }

        foreach (GameObject slot in drawSlotContainers)
        {
            TroopData randomtroopData = troopPool[UnityEngine.Random.Range(0, troopPool.Length)];

            cardUIPrefab = randomtroopData.thisObjectCard;
            GameObject newCard = Instantiate(cardUIPrefab, slot.transform);
            troopsDrawn.Add(newCard);

            AttackCardDisplay cardDisplay = newCard.GetComponent<AttackCardDisplay>();
            if (cardDisplay != null)
            {
                cardDisplay.Setup(randomtroopData);
            }
            else
            {
                Debug.LogWarning("Your cardUIPrefab is missing the 'troopCardDisplay' script!");
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
        troopSlot = null;
    }
    
    #endregion
}