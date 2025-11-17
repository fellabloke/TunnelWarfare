using UnityEngine;

public class AttackRearrange : MonoBehaviour
{
    private Vector3 placementOriginalPosition;

    AttackSlot originSlot;
    AttackSlot targetSlot;

    [SerializeField] private GameObject placementBeingDragged;
    [SerializeField] private GameObject placementToSwap;

    public LayerMask reorderLayer;

    public int currentSelection;
    private float overlapRadius = 2f;
    private bool IsOverValidRearrange;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 500f, reorderLayer))
            {
                originSlot = hit.collider.GetComponent<AttackSlot>();

                if (originSlot != null && originSlot.attackInSlot != null)
                {
                    currentSelection = originSlot.slotIndex;
                    placementBeingDragged = originSlot.attackInSlot;

                    placementOriginalPosition = originSlot.transform.position;
                    placementBeingDragged.GetComponent<Collider>().enabled = false;
                }
                else
                {
                    originSlot = null;
                }
            }
        }

        if (placementBeingDragged != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 500f, reorderLayer))
            {
                placementBeingDragged.transform.position = hit.point;

                Collider[] hits = Physics.OverlapSphere(placementBeingDragged.transform.position, overlapRadius, reorderLayer);
                if (hits.Length > 0)
                {
                    targetSlot = hits[0].GetComponent<AttackSlot>();
                    if (targetSlot != null && targetSlot != originSlot)
                    {
                        IsOverValidRearrange = true;
                        placementBeingDragged.transform.position = targetSlot.transform.position;
                        placementToSwap = targetSlot.attackInSlot;
                    }

                }
                else
                {
                    IsOverValidRearrange = false;
                    targetSlot = null;
                }

            }
            else
            {
                IsOverValidRearrange = false;
                targetSlot = null;
            }
        }

        if (Input.GetMouseButtonUp(0) && placementBeingDragged != null)
        {
            if (IsOverValidRearrange && targetSlot != null)
            {
                originSlot.attackInSlot = placementToSwap;
                targetSlot.attackInSlot = placementBeingDragged;

                placementBeingDragged.transform.position = targetSlot.transform.position;
                if (placementToSwap != null)
                {
                    placementToSwap.transform.position = originSlot.transform.position;
                }
            }
            else
            {
                placementBeingDragged.transform.position = placementOriginalPosition;
            }
            Cleanup(); 
        }
    }
    void Cleanup()
    {
        if (placementBeingDragged != null)
        {
            placementBeingDragged.GetComponent<Collider>().enabled = true;
        }

        placementBeingDragged = null;
        placementToSwap = null;
        originSlot = null;
        targetSlot = null;
        IsOverValidRearrange = false;
    }
}
