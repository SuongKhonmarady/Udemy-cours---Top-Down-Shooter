using UnityEngine;

public enum WeaponType
{
    Pistol,
    Revolver,
    AutoRifle,
    Shotgun,
    Rifle
}


[System.Serializable] // Makes class visible in the inspector.
public class Weapon
{
    public WeaponType weaponType;


    public int bulletsInMagazine;
    public int magazineCapacity;
    public int totalReserveAmmo;

    [Range(1,3)]
    public float reloadSpeed = 1; //  how fast charcter reload weapon
    [Range(1,3)]
    public float equipmentSpeed = 1; // how fast charcter equip weapon

    public bool CanShoot()
    {
        return HaveEnoughBullets();
    }

    private bool HaveEnoughBullets()
    {
        if (bulletsInMagazine > 0)
        {
            bulletsInMagazine--;
            return true;
        }

        return false;
    }

    public bool CanReload()
    {
        if (bulletsInMagazine == magazineCapacity)
            return false;


        if (totalReserveAmmo > 0)
        {
            return true;
        }
        return false;
    }

    public void RefillBullets()
    {
        //totalReserveAmmo += bulletsInMagazine; 
        // this will return bullets in magazine to total amount of bullets


        int bulletesToReload = magazineCapacity;

        if (bulletesToReload > totalReserveAmmo)
            bulletesToReload = totalReserveAmmo;


        totalReserveAmmo -= bulletesToReload;
        bulletsInMagazine = bulletesToReload;

        if (totalReserveAmmo < 0)
            totalReserveAmmo = 0;
    }
}
