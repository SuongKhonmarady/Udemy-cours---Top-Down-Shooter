using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Player player;
    private PlayerControls controls;


    [Header("Aim control")]
    [SerializeField] private Transform aim;

    [SerializeField] private bool isAimingPrecisly;

    [Header("Camera control")]
    [SerializeField] private Transform cameraTarget;
    [Range(.5f,1)]
    [SerializeField] private float minCameraDistance = 1.5f;
    [Range(1,3f)]
    [SerializeField] private float maxCameraDistance = 4;
    [Range(3f, 5f)]
    [SerializeField] private float cameraSensetivity = 5f;


    [Space]

    [SerializeField] private LayerMask aimLayerMask;

    private Vector2 aimInput;
    private RaycastHit lastKnowMouseHit;

    private void Start()
    {
        player = GetComponent<Player>();
        AssignInputEvents();
    }

    private void Update()
    {
        aim.position = GetMouseHitInfo().point;

        if(!isAimingPrecisly)
            aim.position = new Vector3(aim.position.x, aim.position.y + 1, aim.position.z);

        cameraTarget.position = Vector3.Lerp(cameraTarget.position, DesieredCameraPosition(), cameraSensetivity * Time.deltaTime);
    }

    public bool CanAimPrecisly()
    {
        if (isAimingPrecisly)
            return true;
        return false;
    }

    private Vector3 DesieredCameraPosition()
    {

        float actualMaxCameraDistance = player.movement.moveInput.y < -.5f ? minCameraDistance : maxCameraDistance;

        Vector3 desieredCameraPosition = GetMouseHitInfo().point;
        Vector3 aimDirection = (desieredCameraPosition - transform.position).normalized;

        

        float distanceToDesieredPosition = Vector3.Distance(transform.position, desieredCameraPosition);
        float clampedDistance = Mathf.Clamp(distanceToDesieredPosition, minCameraDistance, actualMaxCameraDistance);
        

        desieredCameraPosition = transform.position + aimDirection * clampedDistance;
        desieredCameraPosition.y = transform.position.y + 1;
        

        return desieredCameraPosition;
    }

    public RaycastHit GetMouseHitInfo()
    {
        Ray ray = Camera.main.ScreenPointToRay(aimInput);

        if(Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            lastKnowMouseHit = hitInfo;
            return hitInfo;
        }


        return lastKnowMouseHit;
    }

    private void AssignInputEvents()
    {
        controls = player.controls;

        controls.Character.Aim.performed += context => aimInput = context.ReadValue<Vector2>();
        controls.Character.Aim.canceled += context => aimInput = Vector2.zero;
    }
}
