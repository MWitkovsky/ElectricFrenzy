using UnityEngine;
using System.Collections;

public class RechargerHandler : MonoBehaviour {

    [SerializeField]
    private float chargeAmount;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.Player))
        {
            PlayerManager.AddFrenzyCharge(chargeAmount);
            Destroy(gameObject);
        }
    }
}
