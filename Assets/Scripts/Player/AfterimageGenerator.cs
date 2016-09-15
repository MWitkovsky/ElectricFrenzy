using UnityEngine;
using System.Collections.Generic;

public class AfterimageGenerator : MonoBehaviour {

    private GameObject afterimagePrefab, afterimageContainer;

    //Set in editor
    [SerializeField]
    private float bakeTime, fadeSpeed;

    //Not set in editor
    private List<Renderer> afterimages = new List<Renderer>();
    private List<Renderer> afterimagesToRemove = new List<Renderer>();
    private SkinnedMeshRenderer meshRenderer;
    private float bakeTimer;

	void Start () {
        afterimageContainer = GameObject.Find("Afterimages");
        afterimagePrefab = (GameObject)Resources.Load(ResourcePaths.AfterimagePrefab);

        meshRenderer = GetComponent<SkinnedMeshRenderer>();
        bakeTimer = bakeTime;
	}
	
	void Update () { 
        bakeTimer -= Time.deltaTime;

        if(bakeTimer <= 0.0f)
        {
            //Instantiate the new afterimage
            GameObject newAfterimage = (GameObject)Instantiate(afterimagePrefab, transform.position, transform.rotation);
            newAfterimage.transform.Translate(new Vector3(0.0f, 0.0f, 1.0f));
            newAfterimage.transform.parent = afterimageContainer.transform;

            //Place the baked mesh onto it
            Mesh afterimageMesh = new Mesh();
            meshRenderer.BakeMesh(afterimageMesh);
            newAfterimage.GetComponent<MeshFilter>().mesh = afterimageMesh;

            afterimages.Add(newAfterimage.GetComponent<Renderer>());

            bakeTimer = bakeTime;
        }

        foreach (Renderer r in afterimages) {
            Color color = r.material.color;
            color.a -= Time.deltaTime * fadeSpeed;

            if (color.a <= 0.0f)
                afterimagesToRemove.Add(r);
            else
                r.material.color = color;
        }

        //Destroy afterimages marked for deletion
        foreach (Renderer r in afterimagesToRemove)
        {
            afterimages.Remove(r);
            Destroy(r.gameObject);
        }
        afterimagesToRemove.Clear();
	}

    public void DestroyAllAfterimages()
    {
        foreach (Renderer r in afterimages)
        {
            Destroy(r.gameObject);
        }
        afterimages.Clear();
    }
}
