using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private ClearCounter clearCounter;
    [SerializeField] private GameObject visualGameObject;
    //player的单例实在awake弄得，这里要确保单例弄好了再进行事件的订阅因此用start
    //与外部有关就从start开始，内部初始化就awake
    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedArgs e)
    {
        //if the counter player selects is the same one as
        if (e.selectedCounter == clearCounter)
        {
            Show();
        }
        else
        {
            Hide();
        }

    }

    //make the countervisual show
    private void Show()
    {
        visualGameObject.SetActive(true);
    }

    //hide the counter's visual
    private void Hide()
    {
        visualGameObject.SetActive(false);
    }

}
