using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletsWeapon : MonoBehaviour
{
    public float timer;

    public float speed;

    public Vector3 positionTarget;

    public Vector3 charOwnerPos;

    public Vector3 fixedDirectToCharacter;

    public CharacterManager characterOwner;

    public Vector3 posSpawnBullet;

    private void Update()
    {

        UpdateState();

        autoDespawnIfOutOfRange();
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    public virtual void UpdateState()
    {
        
    }


    public void autoDespawnIfOutOfRange()
    {

        if (Vector3.Distance(charOwnerPos, transform.position) > characterOwner.range)
        {
            gameObject.SetActive(false);
        }
    }
    public void setOwnerPos(Vector3 _charOwnerPos)
    {
        charOwnerPos = _charOwnerPos;

        fixedDirectToCharacter = (positionTarget - charOwnerPos).normalized;
    }
    public void setTargetPosition(Vector3 _targetPos)
    {
        positionTarget = _targetPos;
    }

    public void setOwnerChar(CharacterManager _characterOwner)
    {
        characterOwner = _characterOwner;
    }
}
