using UnityEngine;
using TMPro;
public class FractalTree : MonoBehaviour
{
    public int maxDepth = 8;
    public float childDiameterScale = 0.8f; // Child scale factor for the diameter
    public float childHeightScale = 0.8f; // Child scale factor for the height
    public float branchAngle = 66f;
    public int branches = 2;
    private int depth;

    public float rotationSpeed = 20f;

    public bool randomizeAxisRotation = false;


    public Material trunkMaterial; // Material for the trunk
    public Material leafMaterial; // Material for the leaves
    public GameObject branchPrefab; // Prefab for the branches

    void Start()
    {
        if (depth == 0)
        {
            //Debug.Log("root");
            CreateBranches();
        }
    }

    Vector3 CalculateGlobalScale(Transform transform)
    {
        Vector3 globalScale = transform.localScale;
        Transform parent = transform.parent;

        while (parent != null)
        {
            globalScale = Vector3.Scale(globalScale, parent.localScale);
            parent = parent.parent;
        }

        return globalScale;
    }
public void Initialize(int depth, Vector3 position, Quaternion rotation, Material trunkMaterial, Material leafMaterial, int maxDepth, float childDiameterScale, float childHeightScale, float branchAngle, int branches, bool randomizeAxisRotation)
    {
        this.depth = depth;
        this.trunkMaterial = trunkMaterial;
        this.leafMaterial = leafMaterial;
        this.maxDepth = maxDepth;
        this.childDiameterScale = childDiameterScale;
        this.childHeightScale = childHeightScale;
        this.branchAngle = branchAngle;
        this.branches = branches;
        this.randomizeAxisRotation = randomizeAxisRotation;


        transform.position = position + rotation * Vector3.up * CalculateGlobalScale(this.transform).y;
        transform.rotation = rotation;
        

        if (depth < this.maxDepth)
        {
            CreateBranches();
        }
    }

void Update()
    {
        if (depth %2 == 0)
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.Self);
        else
            transform.Rotate(Vector3.up * -2* rotationSpeed * Time.deltaTime, Space.Self);
    }

    
    

    

    private void CreateBranches()
    {
        for (int i = 0; i < branches; i++)
        {
            GameObject childObj = Instantiate(branchPrefab, new Vector3(0,0,0), Quaternion.identity);
            
            childObj.transform.SetParent(this.transform, false);

            if (depth > 0)
            {
                // Destroy(childObj.GetComponent<Collider>());
            }

            Vector3 childScaleVector = new Vector3(childDiameterScale, childHeightScale, childDiameterScale);
            childObj.transform.localScale = childScaleVector;
            float parentHeight = transform.localScale.y;
            childObj.transform.localPosition = new Vector3(0, parentHeight, 0);

            float angle = ((float)i / branches) * 360f;
            childObj.transform.localRotation = Quaternion.Euler(branchAngle, angle, 0);

            // Apply random rotation around the up axis if enabled
            if (randomizeAxisRotation)
            {
                float randomRotation = Random.Range(-15f, 15f); // Random rotation between -15 and 15 degrees
                childObj.transform.Rotate(Vector3.up, randomRotation);
            }

            if (depth == maxDepth - 1)
            {
                // Spawn a leaf
                //Debug.Log("Leaf");
                childObj.GetComponent<Renderer>().material = leafMaterial;
            }
            else
            {
                childObj.GetComponent<Renderer>().material = trunkMaterial;
            }

            if (depth < maxDepth - 1)
            {
                FractalTree childFractal = childObj.AddComponent<FractalTree>();
                childFractal.Initialize(depth + 1, childObj.transform.position, childObj.transform.rotation, trunkMaterial, leafMaterial, maxDepth, childDiameterScale, childHeightScale, branchAngle, branches, randomizeAxisRotation);
            }
        }
        
    }


}