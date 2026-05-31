using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {

        if (!HasKitchenObject())
        {
            if (!player.HasKitchenObject())
            {
                //Player is not carrying anything
                //we spawn the object from the counter and give it to player
                KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
                OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
            }

        }
    }



}
