using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CuttingRecipeSO : ScriptableObject
{
    //the object before cut
    public KitchenObjectSO input;
    //the object after we cut
    public KitchenObjectSO output;
}
