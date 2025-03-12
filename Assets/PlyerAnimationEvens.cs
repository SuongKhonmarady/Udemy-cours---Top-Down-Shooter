using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlyerAnimationEven : MonoBehaviour
{
    private WeaponVisualController visualController;

    private void Start()
    {
        visualController = GetComponentInParent<WeaponVisualController>();
    }

    public void ReloadIsOver()
    {
        visualController.ReturnRigWeigthToOne();

        //refill-bullets
    }
}
