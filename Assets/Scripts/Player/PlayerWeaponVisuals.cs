using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponVisuals : MonoBehaviour
{

    private Player player;
    private Animator anim;
    private bool isEquipingWeapon;



    [SerializeField] private WeaponModel[] weaponModels;
    [SerializeField] private BackupWeaponModel[] backupWeaponModels;


    [Header("Rig")]
    [SerializeField] private float rigWeightIncreaseRate;
    private bool shouldIncrease_RigWeight;
    private Rig rig;

    [Header("Left hand IK")]
    [SerializeField] private float leftHandIkWeightIncreaseRate;
    [SerializeField] private TwoBoneIKConstraint leftHandIk;
    [SerializeField] private Transform leftHandIk_Target;
    private bool shouldIncrease_LeftHandIkWieght;



    private void Start()
    {
        player = GetComponent<Player>();
        anim = GetComponentInChildren<Animator>();
        rig = GetComponentInChildren<Rig>();
        weaponModels = GetComponentsInChildren<WeaponModel>(true);
        backupWeaponModels = GetComponentsInChildren<BackupWeaponModel>(true);
    }

    private void Update()
    {
        UpdateRigWeight();
        UpdateLeftHandIKWeight();
    }


    public void PlayerReloadAnimation()
    {
        if (isEquipingWeapon)
            return;

        float reloadSpeed = player.weapon.CurrentWeapon().reloadSpeed;


        anim.SetFloat("ReloadSpeed", reloadSpeed);
        anim.SetTrigger("Reload");
        ReduceRigWeight();
    }
    public void PlayWeaponEquipAnimation()
    {
        EquipType equipType = CurrentWeaponModel().equipAnimationType;

        float equipmentSpeed = player.weapon.CurrentWeapon().equipmentSpeed;

        leftHandIk.weight = 0;
        ReduceRigWeight();
        anim.SetTrigger("EquipWeapon");
        anim.SetFloat("EquipType", ((float)equipType));
        anim.SetFloat("EquipSpeed", equipmentSpeed);

        SetBusyGrabbingWeaponTo(true);

    }

    public void SetBusyGrabbingWeaponTo(bool busy)
    {
        isEquipingWeapon = busy;

        anim.SetBool("BusyEquipingWeapon", isEquipingWeapon);
    }

    public void SwitchOnCurrentWeaponModel()
    {
        int animationIndex = ((int)CurrentWeaponModel().holdType);

        SwitchOffWeaponModels();
        SwitchOffBackupWeaponModles();

        if (player.weapon.HaveOnlyOneWeapon() == false)
            SwitchOnBackupWeaponModel();
        

        SwitchAnimationLayer(animationIndex);
        CurrentWeaponModel().gameObject.SetActive(true);
        AttachLeftHand();
    }
    public void SwitchOffWeaponModels()
    {
        for (int i = 0; i < weaponModels.Length; i++)
        {
            weaponModels[i].gameObject.SetActive(false);
        }
    }

    private void SwitchOffBackupWeaponModles()
    {
        foreach (BackupWeaponModel backupModel in backupWeaponModels)
        {
            backupModel.gameObject.SetActive(false);
        }
    }

    public void SwitchOnBackupWeaponModel()
    {
        WeaponType weaponType = player.weapon.BackupWeapon().weaponType;

        foreach (BackupWeaponModel backupModel in backupWeaponModels)
        {
            if (backupModel.weaponType == weaponType)
                backupModel.gameObject.SetActive(true);
        }
    }

    private void SwitchAnimationLayer(int layerIndex)
    {
        for (int i = 1; i < anim.layerCount; i++)
        {
            anim.SetLayerWeight(i, 0);
        }

        anim.SetLayerWeight(layerIndex, 1);
    }

    public WeaponModel CurrentWeaponModel()
    {
        WeaponModel weaponModel = null;

        WeaponType weaponType = player.weapon.CurrentWeapon().weaponType;

        for (int i = 0; i < weaponModels.Length; i++)
        {
            if (weaponModels[i].weaponType == weaponType)
                weaponModel = weaponModels[i];
        }

        return weaponModel;
    }

    #region Amination Rigging Methods

    private void AttachLeftHand()
    {
        Transform targetTranform = CurrentWeaponModel().holdPoint;

        leftHandIk_Target.localPosition = targetTranform.localPosition;
        leftHandIk_Target.localRotation = targetTranform.localRotation;

    }
    private void UpdateLeftHandIKWeight()
    {
        if (shouldIncrease_LeftHandIkWieght)
        {
            leftHandIk.weight += leftHandIkWeightIncreaseRate * Time.deltaTime;
            if (leftHandIk.weight >= 1)
                shouldIncrease_LeftHandIkWieght = false;
        }
    }

    private void UpdateRigWeight()
    {
        if (shouldIncrease_RigWeight)
        {
            rig.weight += rigWeightIncreaseRate * Time.deltaTime;

            if (rig.weight >= 1)
                shouldIncrease_RigWeight = false;
        }
    }

    private void ReduceRigWeight()
    {
        rig.weight = .15f;
    }

    public void MaximizeRigWeight() => shouldIncrease_RigWeight = true;
    public void MaximizeLeftHandWeight() => shouldIncrease_LeftHandIkWieght = true;
    #endregion
}


