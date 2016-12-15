using UnityEngine;
using System.Collections;

public class EnemyDoor : MonoBehaviour {

    //Mark this field if you want doors to not exist until the sensor is entered. If not turned on, then the sensor is pointless and is safe to delete.
    [SerializeField] private bool disableDoorsOnStart;
    //Put all doors here.
    [SerializeField] private GameObject[] doors;
    //Put all enemies tied to the door here.
    [SerializeField] private GameObject[] enemies;

    private float checkDelay = 0.5f, checkTimer;

	void Start () {
        if (disableDoorsOnStart)
        {
            for (int i = 0; i < doors.Length; i++)
            {
                doors[i].SetActive(false);
            }
        }
        
        checkTimer = checkDelay;
	}
	
	void Update () {
	    if(checkTimer > 0.0f)
        {
            checkTimer -= Time.deltaTime;
        }
        else
        {
            for(int i=0; i<enemies.Length; i++)
            {
                if (enemies[i] != null)
                    break;
                else if (i == enemies.Length - 1)
                    OpenDoor();
            }
            checkTimer = checkDelay;
        }
	}

    private void OpenDoor()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            Instantiate(Resources.Load(ResourcePaths.TrapDoorFXPrefab), doors[i].gameObject.transform.position, Quaternion.identity);   
        }
        Destroy(gameObject); //should delete all the doors with it
    }

    public void LockDoor()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].SetActive(true);
            Instantiate(Resources.Load(ResourcePaths.TrapDoorFXPrefab), doors[i].gameObject.transform.position, Quaternion.identity);
        }
    }
}
