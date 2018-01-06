using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
[RequireComponent(typeof(Character))]

public class PlayerControl : MonoBehaviour
{

    public bool isEnabledToFire1, isEnabledToFire2, isEnabledToFire3; //переменная задается из анимации. нужна для запрета или разрешения "стрельбы"
    private Character character;
    private bool isJumping;
    private bool isRunning;

    private bool isUsing;
    private bool isPicking;
    private bool isDroping;


    private bool isFiring1;
    private bool isFiring2;
    private bool isFiring3;


    //Vector3 mousePositionInRealWorld;
    //Camera cam;
    //Vector2 mousePosition;
    private void Awake()
    {
        character = GetComponent<Character>();
        isEnabledToFire1 = true;
        isEnabledToFire2 = true;
        isEnabledToFire3 = true;

    }


    private void Update()
    {
        if (!isJumping)
        {
            isJumping = CrossPlatformInputManager.GetButtonDown("Jump");
        }


        
        isUsing = CrossPlatformInputManager.GetButtonDown("Use");
        isPicking = CrossPlatformInputManager.GetButtonDown("Pick");
        isDroping = CrossPlatformInputManager.GetButtonDown("Drop");


        isFiring1 = CrossPlatformInputManager.GetButtonDown("Fire1") && isEnabledToFire1;
        isFiring2 = CrossPlatformInputManager.GetButtonDown("Fire2") && isEnabledToFire2;
        isFiring3 = CrossPlatformInputManager.GetButtonDown("Fire3") && isEnabledToFire3;


        character.Act(isUsing, isPicking, isDroping);

        
        character.Fire(isFiring1, isFiring2, isFiring3);

        isEnabledToFire1 = true;
        isEnabledToFire2 = true;
        isEnabledToFire3 = true;


    }


    private void FixedUpdate()
    {

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");
        bool isCrouching = Input.GetKey(KeyCode.S);
        character.Move(h,v, isCrouching, isJumping, isRunning);
        isJumping = false;
    }

}
