using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;
    public override void Interact(Player player)
    {

        if (!HasKitchenObject())
        {
            //there's no KitchenObject here
            if (player.HasKitchenObject())
            {
                //player has a KichenObject
                //player.GetKitchenObject().SetKitchenObjectParent(this);
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    //Player's carrying a cuttable object
                    //then we can drop on the cuttingcounter
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                }
                else
                {
                    //we cant drop uncutable object
                }

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
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            //There is a object on the cutting counter, we need to cut it
            //Also , it should can be cut
            KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
            //get the object on the counter then destroy it
            GetKitchenObject().DestroySelf();

            //now we create the slices on the counter
            KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
        }
    }
    private bool HasRecipeWithInput(KitchenObjectSO kitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == kitchenObjectSO)
            {
                //return the output based on recipe ex: cheeseblock -> return cheeseslices
                return true;
            }
        }
        return false;
    }

    //To find output of the object(base on recipe)
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKichenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKichenObjectSO)
            {
                //return the output based on recipe ex: cheeseblock -> return cheeseslices
                return cuttingRecipeSO.output;
            }
        }
        return null;
    }
}
