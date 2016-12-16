using UnityEngine;
using System.Collections;

public class PortalMain : MonoBehaviour {

    [SerializeField] private float flattenTime;
    [SerializeField] private AudioClip goalReachedSound;

    private AudioSource source;
    private float originalXScale, flattenTimer;
    private bool entered;

    void Start()
    {
        source = GetComponent<AudioSource>();
        originalXScale = transform.localScale.x;
        flattenTimer = flattenTime;
    }

	void Update () {
        if (!entered)
        {
            transform.Rotate(new Vector3(0.0f, 9.5f * Time.deltaTime, 0.0f));
        }
        else
        {
            flattenTimer -= Time.deltaTime;
            if (flattenTimer > 0.0f)
                transform.localScale = new Vector3(Mathf.Lerp(originalXScale, 0.0f, 1.0f - (flattenTimer / flattenTime)), transform.localScale.y, transform.localScale.z);
            else
                Destroy(gameObject);

            transform.Rotate(new Vector3(0.0f, 360.0f * Time.deltaTime, 0.0f));
        }
	}

    void OnTriggerEnter2D(Collider2D obj)
    {
        if(obj.gameObject.CompareTag(TagManager.Player))
        {
            entered = true;
            GameManager.BeginVictoryScreen();
            GetComponent<Collider2D>().enabled = false;

            for (int i=0; i<100; i++)
                Instantiate(Resources.Load(ResourcePaths.HackFXPrefab), transform.position, transform.localRotation);
            Destroy(PlayerManager.GetPlayer());
            source.PlayOneShot(goalReachedSound);
        }
    }
}
