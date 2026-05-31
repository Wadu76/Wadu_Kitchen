using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

//using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;
public class Player : MonoBehaviour, IKitchenObjectParents
{
    public static Player Instance { get; private set; }
    public event EventHandler<OnSelectedCounterChangedArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private GameInput gameInput;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;// 用于记录目前选中的台
    private KitchenObject kitchenObject;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("there is more than one player instance");
        }
        Instance = this;
    }
    private void Start()
    {
        //其实就给OnInteractAction订阅个处理Interact的函数
        //按下e InteractAction事件会被调用，里面的订阅函数会被Invoke()
        //按下e->Interact_performed被调用->前者中调用（Invoke）OnInteractAction订阅的函数->
        // 即下面的GameInput_OnInteractAction函数被调用->
        // 处理后检测到clearcounter就输出Interacted! 识别+互动成功
        gameInput.OnInteractAction += GameInput_OnInteractAction;

        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        //原本重复的逻辑在InteractHandler里处理后，我们就可以通过selectedCounter是否为空来进行操作
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }

    }
    private void GameInput_OnInteractAlternateAction(object sender, System.EventArgs e)
    {
        //原本重复的逻辑在InteractHandler里处理后，我们就可以通过selectedCounter是否为空来进行操作
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }

    }
    private void Update()
    {
        //处理角色移动&碰撞检测
        HandleMovement();
        //处理碰撞，会把selectedCounter置为 null/非null
        HandleInteractions();
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float interactDistance = 2f;
        //returns a bool 然后也为raycastHit赋值射线看到的RaycastHit结构体，其中有其transform组件等
        //参数：射线起点，射线发射方向movDir，碰撞信息输出，涉嫌发射的最大长度
        //要是射线发射方向一直是movDir，当角色不动的时候movDir为000，此时就不会检测碰撞了
        //因为角色不是一直在动，因此要时刻记录上次移动指向的方向向量，方便检测后续是否有碰到箱子
        if (moveDir != Vector3.zero)
        {
            //上次移动指向的方向向量，方便检测后续是否有碰到箱子,
            lastInteractDir = moveDir;
        }
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance))
        {
            //不用<>以接受不确定具体的组件类型/通过变量传递类型时
            //raycast不是gameobject，因此我们要找对应组件就得获取到gameobj的组件
            //拿到transform就拿到了gameobj的访问全，就能访问其他component了
            //优先拿transform因为每个obj一直都会有
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                //进来了说明有ClearCounter
                if (baseCounter != selectedCounter)
                {
                    //当被选择的柜台变量不是目前所检测到的，就更新为目前检测到的
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                //所检测到的碰撞体没有ClearCounter脚本组件，说明它不是我们要检测的柜台，同样设置selected为空
                SetSelectedCounter(null);
            }
        }
        else
        {
            //没有检测到碰撞，将 “被选择中的柜台”设为空
            SetSelectedCounter(null);
        }
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        // 当方向向量为0时为false，即notwalking
        isWalking = moveDir != Vector3.zero;

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        //bool canMove = !Physics.Raycast(transform.position, moveDir, playerRadius);
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            //上面检测不能move的时候说不定可以x z方向单独移动
            //来尝试只往x方向能否移动
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            // added movdir.x != 0 to avoid while rotating movedir=vec3.zero
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            if (canMove)
            {
                //能在x上移动
                moveDir = moveDirX;
            }
            else
            {
                //尝试z轴移动
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove)
                {
                    //能在Zed上动
                    moveDir = moveDirZ;
                }
                else
                {
                    //咋也动不了，回头吧
                }
            }
        }
        //没有碰到物品
        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        float rotateSpeed = 10f;
        if (moveDir != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        }
        //transform.forward = Vector3.Slerp(transform.forward, movDir, Time.deltaTime * rotateSpeed); 不动的时候movDir为0，会导致rotate赋值为0提醒

        //transform.forward = Vector3.Slerp(transform.forward, lastInteractDir, Time.deltaTime * rotateSpeed);这样会导致靠墙无法rotate因为靠墙我们会改movdir但是不会给lastdir更新值
    }


    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        //this对于本脚本player实例，左边是player内部private变量初始化为传入的参数
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedArgs
        {
            //这里的初始化是将OnselectedCounter中的selectedcounter赋值为传入的参数
            //用于广播给外部
            selectedCounter = selectedCounter,
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
