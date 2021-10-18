using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class SpotShadowMeshGenerator : MonoBehaviour
{
    private Mesh mesh;

    private Vector3[] vertices;
    private int[] triangles;

    private GameObject currentLight;
    private LightController currentLC;
    [SerializeField] private PlayerController myPlayerController;
    private Transform playerTransform;
    private float playerHeight;
    private float playerHalfWidth;
    private MeshRenderer myMR;

    private Vector3 offset;

    void Start()
    {
        //get references
        currentLight = GameObject.Find("Spot Light");
        currentLC = currentLight.GetComponent<LightController>();

        playerTransform = myPlayerController.gameObject.transform;
        playerHeight = myPlayerController.Height;
        playerHalfWidth = myPlayerController.Width / 2;


        //generate mesh
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        myMR = GetComponent<MeshRenderer>();

    }

    private void Update() 
    {
        //only update mesh when player is under the spot light
        if(myPlayerController.UnderSpotLight)
        {
            myMR.enabled = true;

            CreateShape();
            UpdateMesh();
        }
        else
        {
            myMR.enabled = false;
        }
    }

    private void CreateShape()
    {
        //vertices are in local space
        Vector3 playerLocal0 = new Vector3(- playerHalfWidth, 0, - playerHalfWidth);
        Vector3 playerLocal1 = new Vector3(- playerHalfWidth, 0, + playerHalfWidth);
        Vector3 playerLocal2 = new Vector3(+ playerHalfWidth, 0, + playerHalfWidth);
        Vector3 playerLocal3 = new Vector3(+ playerHalfWidth, 0, - playerHalfWidth);

        Vector3 player0 = new Vector3(playerTransform.position.x - playerHalfWidth, 0,
            playerTransform.position.z - playerHalfWidth);
        Vector3 player1 = new Vector3(playerTransform.position.x - playerHalfWidth, 0,
            playerTransform.position.z + playerHalfWidth);
        Vector3 player2 = new Vector3(playerTransform.position.x + playerHalfWidth, 0,
            playerTransform.position.z + playerHalfWidth);
        Vector3 player3 = new Vector3(playerTransform.position.x + playerHalfWidth, 0,
            playerTransform.position.z - playerHalfWidth);

        Vector3 height = new Vector3(0, playerHeight, 0);

        Debug.Log("player0 = " + playerLocal0);
        Debug.Log("player1 = " + playerLocal1);
        Debug.Log("player2 = " + playerLocal3);
        Debug.Log("player3 = " + playerLocal3);


        vertices = new Vector3[]
        {
            //player bottom
            playerLocal0,
            playerLocal1,
            playerLocal2,
            playerLocal3,

            //shadow bottom
            ShadowCenterPos() + 3 * playerLocal0,
            ShadowCenterPos() + 3 * playerLocal1,
            ShadowCenterPos() + 3 * playerLocal2,
            ShadowCenterPos() + 3 * playerLocal3,

            playerLocal0 + height,
            playerLocal1 + height,
            playerLocal2 + height,
            playerLocal3 + height,

            ShadowCenterPos() + 3 * playerLocal0 + height,
            ShadowCenterPos() + 3 * playerLocal1 + height,
            ShadowCenterPos() + 3 * playerLocal2 + height,
            ShadowCenterPos() + 3 * playerLocal3 + height,
           
        };

        triangles = new int[]
        {
            //bottom
            0, 4, 1,
            1, 4, 5,
            1, 5, 6,
            1, 6, 2, 
            2, 7, 6,
            3, 7, 2,
            0, 4, 7,
            0, 7, 3,

            0, 1, 4,
            1, 5, 4,
            1, 6, 5,
            1, 2, 6,
            2, 6, 7,
            3, 2, 7,
            0, 3, 7,

            0, 1, 3,
            3, 1, 2,
            4, 5, 7,
            7, 5, 6
        };

    }

    private void UpdateMesh()
    {   
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    } 


    private Vector3 ShadowCenterPos()
    {
        ///calculate the center point of the shadow

        Vector2 spotLightPos = new Vector3(currentLight.transform.position.x, currentLight.transform.position.z);
        Vector2 playerPos = new Vector3(playerTransform.position.x, playerTransform.position.z);

        Vector2 relative =  playerPos - spotLightPos;

        float dist = 2 * Vector2.Distance(spotLightPos, playerPos);
        Debug.Log("Dist = " + dist);

        float angle = Vector2.SignedAngle(relative, new Vector2(0, 1));
        Debug.Log("Angle = " + angle);

        Vector3 pos = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle) * dist, 0, Mathf.Cos(Mathf.Deg2Rad * angle) * dist);

        return pos;
    }
  
}
