using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipType { SideEquipAnimation, BackEquipAnimation };
public enum HoldTyoe { CommonHold = 1 , LowHold , HighHlod };

public class WeaponModel : MonoBehaviour
{
    public WeaponType weaponType;
    public EquipType equipAnimationType;
    public HoldTyoe holdType;

    public Transform gunPoint;
    public Transform holdPoint;

}
