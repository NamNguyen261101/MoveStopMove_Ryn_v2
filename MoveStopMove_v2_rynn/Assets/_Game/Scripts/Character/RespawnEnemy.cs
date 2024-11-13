using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class RespawnEnemy : MonoBehaviour
{
    public List<Enemy> EnemyList = new List<Enemy>();

    public List<GameObject> SpawnPos = new List<GameObject>();

    public NameCharacter EnemyName;

    public const string RespawnTag = "Respawn";

    // Update is called once per frame
    void Start()
    {
        InvokeRepeating(RespawnTag, 0, 0.05f);
    }

    public void Respawn()
    {
        if (GameManager.Instance.EnemyCount <= 0)
        {
            return;
        }

        for (int i = 0; i < SpawnPos.Count; i++)
        {
            if (EnemyList[i].gameObject.activeInHierarchy == false)
            {
                EnemyList[i].transform.position = SpawnPos[Random.Range(Random.Range(0, 10), i)].transform.position;

                GameManager.Instance.EnemyCount -= 1;

                Spawn(EnemyList[i]);
            }
        }
    }

    public void Spawn(Enemy _enemy)
    {
        _enemy.gameObject.SetActive(true);

        _enemy.isDead = false;

        _enemy.enabled = true;

        _enemy.Score = 0;

        _enemy.ScoreText.text = _enemy.Score.ToString();

        _enemy.Name.text = "" + (NameCharacter)Random.Range(0, 9);

        _enemy.agent.enabled = true;

        _enemy._collider.enabled = true;

        _enemy.rb.detectCollisions = true;

        if (!GameManager.Instance._listCharacter.Contains(_enemy.GetComponent<CharacterManager>()))
        {
            GameManager.Instance._listCharacter.Add(_enemy.GetComponent<CharacterManager>());
        }
        _enemy.ChangeState(new PatrolSM());
    }
}
