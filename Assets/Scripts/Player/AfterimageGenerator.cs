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
    private SkinnedMeshRenderer meshRenderer, meshRenderer2, meshRenderer3, meshRenderer4;
    private Transform modelTransform;
    private float bakeTimer;

	void Start () {
        afterimageContainer = GameObject.Find("Afterimages");
        afterimagePrefab = (GameObject)Resources.Load(ResourcePaths.AfterimagePrefab);

        meshRenderer = GameObject.Find("PlugMesh_002").GetComponent<SkinnedMeshRenderer>();
        meshRenderer2 = GameObject.Find("Eyes").GetComponent<SkinnedMeshRenderer>();
        meshRenderer3 = GameObject.Find("EarLeft").GetComponent<SkinnedMeshRenderer>();
        meshRenderer4 = GameObject.Find("EarRight").GetComponent<SkinnedMeshRenderer>();

        bakeTimer = bakeTime;
	}
	
	void Update () { 
        bakeTimer -= Time.deltaTime;

        if(bakeTimer <= 0.0f)
        {
            BakeMesh(meshRenderer);
            BakeMesh(meshRenderer2);
            BakeMesh(meshRenderer3);
            BakeMesh(meshRenderer4);

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

    private void BakeMesh(SkinnedMeshRenderer meshRenderer)
    {
        //Instantiate the new afterimage
        GameObject newAfterimage = (GameObject)Instantiate(afterimagePrefab, modelTransform.position, modelTransform.rotation);
        if(meshRenderer == this.meshRenderer || meshRenderer == meshRenderer3 || meshRenderer == meshRenderer4)
            newAfterimage.transform.position -= modelTransform.forward / 4.5f;

        newAfterimage.transform.parent = afterimageContainer.transform;
        newAfterimage.tag = "Untagged";

        //Place the baked mesh onto it
        Mesh afterimageMesh = new Mesh();
        meshRenderer.BakeMesh(afterimageMesh);
        newAfterimage.GetComponent<MeshFilter>().mesh = afterimageMesh;

        afterimages.Add(newAfterimage.GetComponent<Renderer>());
    }

    public void SetModelTransform(Transform modelTransform)
    {
        this.modelTransform = modelTransform;
    }
}
