using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;



    //function works after player interacts a counter by pressing e
    //in clear counter we'll only be able to put things on it
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


}
