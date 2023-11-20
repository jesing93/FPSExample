using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    [Header("Camera")]
    [SerializeField] private float mouseSensibility;
    [SerializeField] private float maxViewX;
    [SerializeField] private float minViewX;
    private float rotationX;

    [Header("Data")]
    [SerializeField] private int currentAmmo;
    [SerializeField] private int maxAmmo;
    [SerializeField] private float currentLife;
    [SerializeField] private float maxLife;
    private float currentStamina = 100f;
    private float maxStamina = 100f;
    private float staminaRecoverRate = 10f;
    private float staminaConsumptionRate = 20f;
    private float staminaRecoveryDelay = 2f;
    private float lastTimeEmptyStamina;
    private bool isSprinting;
    private bool isSprintPressed;
    private int score;

    //Components
    private Camera pCamera;
    private Rigidbody rb;
    private WeaponController weaponController;

    //Singletone
    public static PlayerManager instance;

    private void Awake()
    {
        //Singletone
        instance = this;

        //Components
        rb = GetComponent<Rigidbody>();
        pCamera = Camera.main;
        weaponController = GetComponent<WeaponController>();
        //weaponController.IsPlayer = true;

        //Hide the cursor
        Cursor.lockState = CursorLockMode.Locked;

        //Initialize HUD
        HudController.instance.UpdateAmmoStorage(currentAmmo + " / " + maxAmmo);
    }

    private void Update()
    {
        if (currentLife > 0)
        {
            CameraView();
            //Jump
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }

            //Fire
            if (Input.GetButtonDown("Fire1"))
            {
                if (weaponController.CanShoot())
                {
                    weaponController.Shoot();
                }
            }

            //Reload
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (currentAmmo > 0)
                {
                    int ammoSpent = weaponController.Reload(currentAmmo);
                    if (ammoSpent > 0)
                    {
                        currentAmmo -= ammoSpent;
                        HudController.instance.UpdateAmmoStorage(currentAmmo + " / " + maxAmmo);
                    }
                }
            }

            //Stamina management
            Sprint();
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    /// <summary>
    /// Movement action
    /// </summary>
    private void MovePlayer()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 direction;

        if (isSprinting && currentStamina > 0)
        {
            direction = (transform.right * x + transform.forward * z).normalized * speed * 2f;
        }
        else
        {
            direction = (transform.right * x + transform.forward * z).normalized * speed;
        }
        
        direction.y = rb.velocity.y;

        rb.velocity = direction;
    }

    /// <summary>
    /// Jump action
    /// </summary>
    private void Jump()
    {
        //Cast a ray down from player position
        Ray ray = new Ray(transform.position, Vector3.down);
        
        //If collides, can jump
        if (Physics.Raycast(ray, 1.1f))
        {
            rb.velocity = new Vector3 (rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }

    /// <summary>
    /// Manage the sprint and stamina
    /// </summary>
    private void Sprint()
    {
        isSprintPressed = Input.GetKey(KeyCode.LeftShift);
        if (isSprintPressed)
        {
            //If can sprint and not sprinting
            if (currentStamina > 0 && !isSprinting) // && lastTimeEmptyStamina + staminaRecoveryDelay < Time.time
            {
                isSprinting = true;
            }
        }
        else if (isSprinting)
        {
            //Stop sprint
            isSprinting = false;
        }

        if (isSprinting)
        {
            //If trying to sprint and able to do it
            if (currentStamina > 0)
            {
                currentStamina = Mathf.Clamp(currentStamina - staminaConsumptionRate * Time.deltaTime, 0, maxStamina);
                HudController.instance.UpdateStamina(currentStamina);
            }
            else
            {
                isSprinting = false;
                //lastTimeEmptyStamina = Time.time;
                //StartCoroutine(HudController.instance.StaminaExhausted(4, .2f));
            }
        }
        else if (currentStamina < maxStamina && !isSprintPressed)
        {
            //Stamina regeneration
            currentStamina = Mathf.Clamp(currentStamina + staminaRecoverRate * Time.deltaTime, 0, maxStamina);
            HudController.instance.UpdateStamina(currentStamina);
        }
    }

    /// <summary>
    /// The controller of the camera movement (Uses mouse sensibility)
    /// </summary>
    private void CameraView()
    {
        //Get from Mouse Input X and Y axis
        float y = Input.GetAxis("Mouse X") * mouseSensibility;
        rotationX += Input.GetAxis("Mouse Y") * mouseSensibility;

        //Cut x rotation with min and max limits
        rotationX = Mathf.Clamp(rotationX, minViewX, maxViewX);

        //Camera rotation
        pCamera.transform.localRotation = Quaternion.Euler(-rotationX, 0, 0);

        //Rotate the player
        transform.eulerAngles += Vector3.up * y;
    }

    public void ReceiveDamage(float damage, bool damageOwner)
    {
        currentLife = Mathf.Clamp(currentLife - damage, 0, maxLife);
        HudController.instance.UpdateHealth(currentLife);
        if (currentLife == 0)
        {
            HudController.instance.OpenEndPanel();
        }
    }

    public void ReceiveAmmo(int ammo)
    {
        currentAmmo = Math.Clamp((currentAmmo + ammo), 0, maxAmmo);
        HudController.instance.UpdateAmmoStorage(currentAmmo + " / " + maxAmmo);
    }

    public void ReceiveHealth(int health)
    {
        currentLife = Mathf.Clamp((currentLife + health), 0, maxLife);
        HudController.instance.UpdateHealth(currentLife);
    }

    public void AddScore(int bonusScore)
    {
        score += bonusScore;
        HudController.instance.UpdateScore(score);
    }
}
