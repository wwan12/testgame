using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;

public class EnemyController : NetworkBehaviour
{
    GameObject[] playerTargets = null;
    public float ENEMY_SPEED = 4.0f;

	void Start ()
    {
        playerTargets = GameObject.FindGameObjectsWithTag("Player");
	}

	void Update ()
    {
        // TODO: Delete this when spawner is made. 
        playerTargets = GameObject.FindGameObjectsWithTag("Player");

        // Find a player to chase
        foreach (GameObject player in playerTargets)
        {
            // TODO: Only chase the closest player based on distance
            // float distance = Vector3.Distance(transform.position, player.transform.position);

            EngageTarget(player);
        }
    }


    private void EngageTarget(GameObject targetObject)
    {
        Vector3 targetDir = targetObject.transform.position - transform.position;
        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 180);
        transform.Translate(Vector3.up * Time.deltaTime * ENEMY_SPEED);
    }
}
