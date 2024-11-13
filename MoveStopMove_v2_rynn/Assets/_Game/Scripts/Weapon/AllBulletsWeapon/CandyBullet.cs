using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyBullet : BulletsWeapon
{
    Transform CandyBulletTransform;
    private void Start()
    {
        CandyBulletTransform = this.transform;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        positionTarget.y = 0.92f;

        Quaternion lookTarget = Quaternion.LookRotation(fixedDirectToCharacter);

        Vector3 rotation = Quaternion.Lerp(CandyBulletTransform.rotation, lookTarget, Time.deltaTime * 30f).eulerAngles;

        CandyBulletTransform.Translate(fixedDirectToCharacter * Time.deltaTime * speed, Space.World);

        CandyBulletTransform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }
}
