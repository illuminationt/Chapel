using UnityEngine;

public class CpEnemyJellyfish : CpEnemyBase
{
    protected override void Update()
    {
        base.Update();

        Animator anim = GetComponentInChildren<Animator>();

        if (Input.GetKeyDown(KeyCode.T))
        {
            anim.Play("ChargeST");
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            anim.Play("ChargeLP");
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            anim.Play("ChargeED");
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            anim.Play("Default");
        }
    }
}
