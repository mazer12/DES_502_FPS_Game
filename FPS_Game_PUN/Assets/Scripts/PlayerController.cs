using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerController : MonoBehaviour
public class PlayerController : MonoBehaviourPunCallbacks, IDamagable
{

    [SerializeField] GameObject cameraHolder;

    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, maxAnimSpeed, jumpForce, smoothTime;

<<<<<<< Updated upstream
    public Animator anim;
    private bool isJumping;
    private bool isGrounded;
=======
    [SerializeField] Item[] itemList;

    int currentItemIndex;
    int previousItemIndex = -1;
    bool gunEquiped = false; //Initially no gun equipped, so player cant shoot bullet. 

>>>>>>> Stashed changes
    float verticalLookRotation;
    bool grounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;

    Rigidbody rb;

    PhotonView PV;

    const float maxHealth = 100f;
    float currHealth = maxHealth;

    PlayerManager playerManager;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();

        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    void Start()
    {
        if (!PV.IsMine)
        if (PV.IsMine)
        {
            return;
        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
        }
    
    }

    int mod(int k, int n) { return ((k %= n) < 0) ? k + n : k; }

    void Update()
    {
        if (!PV.IsMine)
            return;

        Look();
        Move();
        Jump();
<<<<<<< Updated upstream
        if (grounded)
        {
            anim.SetBool("isGrounded", true);
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", false);
        }
        else
        {
            anim.SetBool("isGrounded", false);
            anim.SetBool("isFalling", true);
=======

       

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
        {
            int temp = mod((currentItemIndex + 1),  2);
            EquipItem(temp);
            gunEquiped = true;
        }
        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        {
            int temp = mod((currentItemIndex - 1), 2);
            EquipItem(temp);
            gunEquiped= true;
        }

        if (Input.GetMouseButtonDown(0) && gunEquiped) 
        {
            itemList[currentItemIndex].Use();
>>>>>>> Stashed changes
        }
    }

    void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);

        float inputMagnitude = Mathf.Clamp01(moveDir.magnitude) * walkSpeed;
        float speed = inputMagnitude * walkSpeed;
        if (Input.GetKey(KeyCode.LeftShift)|| Input.GetKey(KeyCode.RightShift))
        {
            inputMagnitude *= 2;
        }
        anim.SetFloat("Input Magnitude", inputMagnitude, 0.1f, Time.deltaTime); ;

        if (moveDir != Vector3.zero)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
        
    }

    void Jump()
    {
        if (Input.GetButton("Jump") && grounded)
        {
            anim.SetBool("isJumping", true);
            rb.AddForce(transform.up * jumpForce);
        }
    }

    public void SetGroundedState(bool _grounded)
    {
        grounded = _grounded;
    }


    void EquipItem(int index)
    {
        if(index == previousItemIndex)
        {
            return;
        }

        currentItemIndex = index;

        itemList[currentItemIndex].itemGameObj.SetActive(true);

        if(previousItemIndex != -1)
        {
            itemList[previousItemIndex].itemGameObj.SetActive(false);
        }

        previousItemIndex = currentItemIndex;

        if (PV.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("currentItemIndex", currentItemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(!PV.IsMine && targetPlayer == PV.Owner)
        {
            EquipItem((int)changedProps["currentItemIndex"]);
        }
    }

    void FixedUpdate()
    {
        if (!PV.IsMine)
            return;

        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);

        
    }

    public void TakeDamage(float damage)
    {
        PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    public void RPC_TakeDamage(float damage)
    {
        if(!PV.IsMine)
            return;

        currHealth -= damage;

        if(currHealth <= 0.0f)
        {
            Die();
        }
    }

    void Die()
    {
        playerManager.Die();
    }
}