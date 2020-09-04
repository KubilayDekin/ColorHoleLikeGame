using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnChangePosition : MonoBehaviour
{
    public PolygonCollider2D hole2Dcollider; // 2D collider for hole's borders (scaling with initial scale)
    public PolygonCollider2D ground2Dcollider; // 2d collider that covers the platform

    public MeshCollider generatedMeshCollider; // 3d mesh collider. It will be generated from 2Dhole and 2Dground colliders.
    public Collider groundCollider; // basic collider for platform.

    public float initialScale=0.5f; // hole's scale

    Mesh generatedMesh;

    private void Start()
    {
        GameObject[] allObstacles = FindObjectsOfType(typeof(GameObject)) as GameObject[];

        foreach(var obstacle in allObstacles)
        {
            if (obstacle.layer == LayerMask.NameToLayer("Obstacles"))
            {
                Physics.IgnoreCollision(obstacle.GetComponent<Collider>(), generatedMeshCollider, true);
            }
        }
    }

    // When an object enters to hole capsule collider.
    // All objects, will not be in interaction with ground collider since they are still in hole's borders. 
    private void OnTriggerEnter(Collider other)
    {
        Physics.IgnoreCollision(other, groundCollider,true);
        Physics.IgnoreCollision(other, generatedMeshCollider, false);
    }

    // When an object exits from hole capsule collider.
    // All objects, will not be in interaction with generated mesh collider when they left hole's borders.
    private void OnTriggerExit(Collider other)
    {
        Physics.IgnoreCollision(other, groundCollider, false);
        Physics.IgnoreCollision(other, generatedMeshCollider, true);
    }

    //When a transform component changes, this update will make this class calculate the vertices and paths and generate new mesh and collider.
    private void FixedUpdate()
    {
        if (transform.hasChanged == true)
        {
            transform.hasChanged = false;
            hole2Dcollider.transform.position = new Vector2(transform.position.x, transform.position.z);
            hole2Dcollider.transform.localScale = transform.localScale * initialScale;
            MakeHole2D();
            Make3DMesh();
        }
    }

    // Preparing and calculating positions of vertices, setting paths (Need 2 paths for current platform) for mesh that will arrange object-hole interaction.
    private void MakeHole2D()
    {
        Vector2[] pointPositions = hole2Dcollider.GetPath(0);

        for (int i = 0; i<pointPositions.Length; i++)
        {
            pointPositions[i] = hole2Dcollider.transform.TransformPoint(pointPositions[i]);
        }

        ground2Dcollider.pathCount = 2;
        ground2Dcollider.SetPath(1, pointPositions);
    }

    // Generating 3D Mesh and MeshCollider from ground collider.
    private void Make3DMesh()
    {
        if (generatedMesh != null)
        {
            Destroy(generatedMesh);
        }
        generatedMesh = ground2Dcollider.CreateMesh(true, true);
        generatedMeshCollider.sharedMesh = generatedMesh;
    }
}
