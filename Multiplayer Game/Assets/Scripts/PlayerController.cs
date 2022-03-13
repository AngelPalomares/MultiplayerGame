using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform ViewPoint;
    public float MouseSensitivity = 1f;
    private float VerticalRotationStore;
    private Vector2 mouseInput;

    public float MoveSpeed, runspeed;

    private float activeMoveSpeed;

    private Vector3 MoveDirection, Movement;

    public CharacterController charCon;

    private Camera TheCam;

    public float JumpForce;

    public float GravityMod;

    public Transform groundCheckPoint;
    private bool isGrounded;
    public LayerMask groundLayers;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        TheCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * MouseSensitivity;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x,transform.rotation.eulerAngles.z);

        VerticalRotationStore += mouseInput.y;

        VerticalRotationStore = Mathf.Clamp(VerticalRotationStore, -60f, 60f);

        ViewPoint.rotation = Quaternion.Euler(-VerticalRotationStore, ViewPoint.rotation.eulerAngles.y, ViewPoint.eulerAngles.z);

        MoveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

        if(Input.GetKey(KeyCode.LeftShift))
        {
            activeMoveSpeed = runspeed;
        }
        else
        {
            activeMoveSpeed = MoveSpeed;
        }

        float yvel = Movement.y;
        Movement = ((transform.forward * MoveDirection.z) + (transform.right * MoveDirection.x)).normalized * activeMoveSpeed;
        Movement.y = yvel;

        if(charCon.isGrounded)
        {
            Movement.y = 0f;
        }

        isGrounded = Physics.Raycast(groundCheckPoint.position, Vector3.down,.25f,groundLayers);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            Movement.y = JumpForce;
        }

        Movement.y += Physics.gravity.y * Time.deltaTime * GravityMod;

        charCon.Move(Movement * Time.deltaTime);

        if(Input.GetMouseButtonDown(0))
        {
            Shoot();
        }


        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else if(Cursor.lockState == CursorLockMode.None)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    private void Shoot()
    {
        Ray ray = TheCam.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
        ray.origin = TheCam.transform.position;

        if(Physics.Raycast(ray,out RaycastHit hit))
        {
            Debug.Log("we hit" + hit.collider.gameObject.name);
        }

    }

    private void LateUpdate()
    {
        TheCam.transform.position = ViewPoint.position;
        TheCam.transform.rotation = ViewPoint.rotation;
    }
}
