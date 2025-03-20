using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Player player;
    private PlayerControls controls;

    [Header("Aim Viusal - Laser")]
    [SerializeField] private LineRenderer aimLaser;


    [Header("Aim control")]
    [SerializeField] private Transform aim;

    [SerializeField] private bool isAimingPrecisly;
    [SerializeField] private bool isLockingToTarget;

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

    private Vector2 mouseInput;
    private RaycastHit lastKnowMouseHit;

    private void Start()
    {
        player = GetComponent<Player>();
        AssignInputEvents();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
            isAimingPrecisly = !isAimingPrecisly;

        if (Input.GetKeyDown(KeyCode.L))
            isLockingToTarget = !isLockingToTarget;

        UpdateAimVisuals();
        UpdateAimposition();
        UpdateCameraPosition();
    }

    private void UpdateAimVisuals()
    {

        Transform gunPoint = player.weapon.GunPoint();
        Vector3 laserDirection = player.weapon.BulletDirection();
        float laserTipLenght = .5f;
        float gunDistance = 4f;


        Vector3 endPoint = gunPoint.position + laserDirection * gunDistance;

        if(Physics.Raycast(gunPoint.position, laserDirection, out RaycastHit hit, gunDistance))
        {
            endPoint = hit.point;
            laserTipLenght = 0;
        }

        aimLaser.SetPosition(0, gunPoint.position);
        aimLaser.SetPosition(1, endPoint);
        aimLaser.SetPosition(2, endPoint + laserDirection * laserTipLenght);
    }
    private void UpdateAimposition()
    {
        Transform target = Target();

        if(target!= null && isLockingToTarget)
        {
            aim.position = target.position;
            return;
        }

        aim.position = GetMouseHitInfo().point;

        if (!isAimingPrecisly)
            aim.position = new Vector3(aim.position.x, aim.position.y + 1, aim.position.z);
    }


    public Transform Target()
    {
        Transform target = null;

        if(GetMouseHitInfo().transform.GetComponent<Target>() != null)
        {
            target = GetMouseHitInfo().transform;
        }

        return target;
    }
    public Transform Aim() => aim;
    public bool CanAimPrecisly() => isAimingPrecisly;
    public RaycastHit GetMouseHitInfo()
    {
        Ray ray = Camera.main.ScreenPointToRay(mouseInput);

        if(Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            lastKnowMouseHit = hitInfo;
            return hitInfo;
        }


        return lastKnowMouseHit;
    }
    #region Camera Region

    private void UpdateCameraPosition()
    {
        cameraTarget.position =
            Vector3.Lerp(cameraTarget.position, DesieredCameraPosition(), cameraSensetivity * Time.deltaTime);
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

    #endregion
    private void AssignInputEvents()
    {
        controls = player.controls;

        controls.Character.Aim.performed += context => mouseInput = context.ReadValue<Vector2>();
        controls.Character.Aim.canceled += context => mouseInput = Vector2.zero;
    }
}
