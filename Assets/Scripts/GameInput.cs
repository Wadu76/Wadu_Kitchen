using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//目前处理的Input有：
//移动输入Move
//互动Interact

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractAction;
    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        //找到对应的movement map并手动启动
        playerInputActions.Player.Enable();

        //performed是unitynew input system自带的event
        //按下按键后会触发
        playerInputActions.Player.Interact.performed += Interact_performed;

    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        //?负责判空 this就是本身 是个sender
        //按下案件后interact_performed被调用，内部调用OnInteractAction订阅的方法
        OnInteractAction?.Invoke(this, EventArgs.Empty);
        Debug.Log(obj);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        //访问的是Player这个input map（player的iinput）
        //然后要的是Playerinput里的moveaction
        //最后我们要读这个move action的vector2的值，我们本来就设置的是以vector2
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        //我们也可以在input system里的 action props里processor里把vector2归一化，那就不需要下面这行了
        inputVector = inputVector.normalized;
        return inputVector;
    }
}
