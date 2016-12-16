using UnityEngine;
using System.Collections;

public class TrojanMain : MonoBehaviour {

    //Status inflicting properties
    [SerializeField] private float statusDuration;
    [SerializeField] private bool poison, paralyze, slowDown;
    private Status status;
    public enum Status { poison, paralyze, slowDown, none };

    //Normal enemy properties
    [SerializeField] private int health, packetYield;
    [SerializeField] private float damageYield;

    void Start () {
        //Ensures only one status is inflicted
        if (poison)
            status = Status.poison;
        else if (paralyze)
            status = Status.paralyze;
        else if (slowDown)
            status = Status.slowDown;
        else
            status = Status.none;
	}

    private void TakeDamage(int damage)
    {
        Instantiate(Resources.Load(ResourcePaths.SmallHitPrefab), transform.position, Quaternion.identity);
        health -= damage;

        if (health <= 0)
        {
            //Death stuff
            Instantiate(Resources.Load(ResourcePaths.HitBurstPrefab), transform.GetChild(0).position, Quaternion.identity);

            for (int i = 0; i < packetYield; i++)
                Instantiate(Resources.Load(ResourcePaths.ReclaimedPacketPrefab), transform.position, Quaternion.identity);

            Destroy(gameObject);
        }

        PlayerManager.ResetAttackCooldown();
    }

    private void InfectPlayer()
    {
        if (status == Status.poison)
            PlayerManager.SetStatus(PlayerInfo.Status.Poison, statusDuration);
        else if (status == Status.paralyze)
            PlayerManager.SetStatus(PlayerInfo.Status.Paralyzed, statusDuration);
        else if (status == Status.slowDown)
            PlayerManager.SetStatus(PlayerInfo.Status.Slowed, statusDuration);

        PlayerManager.Damage(damageYield, true);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.Player))
        {
            if (PlayerManager.IsAttacking())
            {
                TakeDamage(PlayerManager.CalculateDamageDone());
            }
            else
            {
                InfectPlayer();
            }
        }
    }
}
