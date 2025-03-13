using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Player player;
    private PlayerControls controls;


    [Header("Aim info")]
    [Range(.5f,1)]
    [SerializeField] private float minCameraDistance = 1.5f;
    [Range(1,3f)]
    [SerializeField] private float maxCameraDistance = 4;

    [Range(3f, 5f)]
    [SerializeField] private float aimSensetivity = 5f;

    [SerializeField] private Transform aim;
    [SerializeField] private LayerMask aimLayerMask;

    private Vector2 aimInput;

    private void Start()
    {
        player = GetComponent<Player>();
        AssignInputEvents();
    }

    private void Update()
    {
        aim.position = Vector3.Lerp(aim.position, DesieredAimPosition(), aimSensetivity * Time.deltaTime);
    }

    private Vector3 DesieredAimPosition()
    {

        float actualMaxCameraDistance = player.movement.moveInput.y < -.5f ? minCameraDistance : maxCameraDistance;

        Vector3 desieredAimPosition = GetMousePosition();
        Vector3 aimDirection = (desieredAimPosition - transform.position).normalized;

        

        float distanceToDesieredPosition = Vector3.Distance(transform.position, desieredAimPosition);
        float clampedDistance = Mathf.Clamp(distanceToDesieredPosition, minCameraDistance, actualMaxCameraDistance);
        

        desieredAimPosition = transform.position + aimDirection * clampedDistance;
        desieredAimPosition.y = transform.position.y + 1;
        

        return desieredAimPosition;
    }

    public Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(aimInput);

        if(Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            return hitInfo.point;
        }


        return Vector3.zero;
    }

    private void AssignInputEvents()
    {
        controls = player.controls;

        controls.Character.Aim.performed += context => aimInput = context.ReadValue<Vector2>();
        controls.Character.Aim.canceled += context => aimInput = Vector2.zero;
    }
}
