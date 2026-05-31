using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{

    [SerializeField] private KitchenObjectSO cutKitchenObjectSO;
    public override void Interact(Player player)
    {

        if (!HasKitchenObject())
        {
            //there's no KitchenObject here
            if (player.HasKitchenObject())
            {
                //player has a KichenObject
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                //player has nothing
            }
        }
        else
        {
            //there's a KitchenObject here
            if (player.HasKitchenObject())
            {
                //player is carrying something
            }
            else
            {
                //player is not carrying anything,then give the object to player
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject())
        {
            //There is a object on the cutting counter, we need to cut it
            //get the object on the counter then destroy it
            GetKitchenObject().DestroySelf();

            //now we create the slices on the counter
            KitchenObject.SpawnKitchenObject(cutKitchenObjectSO, this);
        }
    }
}
