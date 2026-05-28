using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParents kitchenObjectParent;

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public void SetKitchenObjectParent(IKitchenObjectParents kichenObjectParent)
    {
        if (this.kitchenObjectParent != null)
        {
            //clear the original counter first
            this.kitchenObjectParent.ClearKitchenObject();
        }

        this.kitchenObjectParent = kichenObjectParent;

        if (kichenObjectParent.HasKitchenObject())
        {
            Debug.LogError("kichenObjectParent already has a KitchenObject!");
        }

        kichenObjectParent.SetKitchenObject(this);


        transform.parent = kichenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectParents GetKitchenObjectParents()
    {
        return kitchenObjectParent;
    }


}
