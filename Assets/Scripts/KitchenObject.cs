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


    public void DestroySelf()
    {
        //make the object on the counter it's child
        kitchenObjectParent.ClearKitchenObject();
        //then we destroy it
        Destroy(gameObject);
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParents kitchenObjectParents)
    {
        //now we create the slices on the counter
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        //set the counter as parent 
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParent(kitchenObjectParents);
        return kitchenObject;
    }
}
