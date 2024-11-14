using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletsWeapon : MonoBehaviour
{
    [Header ("Settings")]
    public float timer;

    public float speed;

    [Header("Target")]
    public Vector3 positionTarget; // point target

    public Vector3 charOwnerPos; // Char Owner Position

    protected Vector3 fixedDirectToCharacter; 

    public CharacterManager characterOwner; // Character == Owner for Object Weapon

    private Vector3 posSpawnBullet; 

    private void Update()
    {

        UpdateState();

        AutoDespawnIfOutOfRange();
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    public virtual void UpdateState()
    {
        
    }

    // BULLET OUT OF RANGE == DESPAWN
    public void AutoDespawnIfOutOfRange()
    {

        if (Vector3.Distance(charOwnerPos, transform.position) > characterOwner.range)
        {
            gameObject.SetActive(false);
        }
    }

    // Set OWN POS
    public void SetOwnerPos(Vector3 _charOwnerPos)
    {
        // Starting from (Char)
        charOwnerPos = _charOwnerPos;

        fixedDirectToCharacter = (positionTarget - charOwnerPos).normalized;
    }
    // Set Target Position => BULLET TO TARGET
    public void SetTargetPosition(Vector3 _targetPos)
    {
        positionTarget = _targetPos;
    }

    // SET POSITION FOR CHAR TO KNOW THE TARGET
    public void SetOwnerChar(CharacterManager _characterOwner)
    {
        characterOwner = _characterOwner;
    }
}
