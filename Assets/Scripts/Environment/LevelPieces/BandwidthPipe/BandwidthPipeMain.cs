using UnityEngine;
using System.Collections;

public class BandwidthPipeMain : MonoBehaviour {

    [SerializeField]
    private float speed;
    [SerializeField]
    private Transform[] intermediaryPoints;

    private Transform entrance, exit, target, player;
    private int index;

	void Start () {
	    foreach (Transform t in transform)
        {
            if (t.name == "Entrance")
                entrance = t;
            else if (t.name == "Exit")
                exit = t;
        }

        entrance.GetComponent<BandwidthPipeEntranceHandler>().SetMain(this);

        target = entrance;
        index = -1;
	}

	void FixedUpdate () {
        if (player)
        {
            Vector3 nextMove = (target.position - player.position).normalized * Time.fixedDeltaTime * speed;

            if (Vector3.Distance(player.position, target.position) > Vector3.Distance((player.position + nextMove), target.position))
            {
                player.Translate((target.position - player.position).normalized * Time.fixedDeltaTime * speed);
            }                
            else
            {
                player.position = target.position;
                ChooseNextTarget();
            }
        }
	}

    private void ChooseNextTarget()
    {
        if (target != exit)
        {
            if (++index < intermediaryPoints.Length)
                target = intermediaryPoints[index];
            else
                target = exit;
        }
        else
        {
            index = -1;
            target = entrance;
            player = null;
            PlayerManager.SetTeleporting(false);
        }
    }

    public void SetPlayer(Transform player)
    {
        this.player = player;
        PlayerManager.SetTeleporting(true);
    }
}
