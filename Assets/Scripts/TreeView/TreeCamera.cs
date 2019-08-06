using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class TreeCamera : MonoBehaviour
{
    public TreeNode activeNode;
    public float movementSpeed;

    Vector2 currentVelocity;
    Camera thisCamera;
    Canvas currentCanvas;
    Rigidbody2D rb;
    Bounds thisBounds;

    public float sizeMovementSpeed;
    public float maxSizeChangeRate;
    float currentSizeChangeRate;

    public float wordFocusPointPadding;

    PlayerInputController thisInputController;
    // Start is called before the first frame update
    void Awake()
    {
        thisBounds = new Bounds();
        thisCamera = GetComponent<Camera>();
        currentCanvas = FindObjectOfType<Canvas>();
        thisInputController = FindObjectOfType<PlayerInputController>();
        rb = GetComponent<Rigidbody2D>();
        activeNode = NearestNode(Vector2.zero);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorldPosition = thisCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = currentCanvas.transform.position.z;

        if (Input.GetMouseButtonDown(0))
        {

            activeNode = NearestNode(mouseWorldPosition);

        }
        if (Input.GetMouseButtonDown(1))
        {
            TreeNode selectedNode = NearestNode(mouseWorldPosition);
            if (selectedNode.parentNode != null)
            {
                thisInputController.AddChildNodes(selectedNode);
            }
        }

        GetNodeChildBounds();

        Vector3 destinationPostion = activeNode.transform.position;
        destinationPostion.z = transform.position.z;


        float destinationSize = Mathf.Lerp(thisBounds.extents.y, thisBounds.extents.x / thisCamera.aspect, 0.5f);
        destinationSize = Mathf.Max(destinationSize, 1);
        currentSizeChangeRate = destinationSize - thisCamera.orthographicSize;
        currentSizeChangeRate *= sizeMovementSpeed;
        currentSizeChangeRate = Mathf.Clamp(currentSizeChangeRate, -maxSizeChangeRate, maxSizeChangeRate);

        thisCamera.orthographicSize += currentSizeChangeRate * Time.deltaTime;


        currentVelocity = (Vector2)(destinationPostion - transform.position);
        currentVelocity *= movementSpeed;
        rb.velocity = currentVelocity;

    }

    TreeNode NearestNode(Vector2 position)
    {
        TreeNode[] allNodes = FindObjectsOfType<TreeNode>();
        if (allNodes.Length == 0)
        {
            return null;
        }
        return allNodes.OrderBy(s => Vector2.Distance(s.transform.position, position)).First();
    }


    void GetNodeChildBounds()
    {

        List<Vector3> containedPoints = new List<Vector3>();
        containedPoints.Add(activeNode.transform.position);
        if (activeNode.parentNode != null)
        {
            containedPoints.Add(activeNode.parentNode.transform.position);
        }
        containedPoints.AddRange(activeNode.childNodes.Select(s => s.transform.position));

        Vector2 maxPosition = Vector2.zero;
        Vector2 minPosition = Vector2.zero;
        Vector3 currentCenter = transform.position;
        currentCenter.z = 0;
        for (int i = 0; i < containedPoints.Count; i++)
        {
            Vector2 pointTowardContainedPoint = (containedPoints[i] - currentCenter);
            pointTowardContainedPoint = pointTowardContainedPoint.normalized * (pointTowardContainedPoint.magnitude * wordFocusPointPadding);
            Debug.DrawRay(currentCenter, pointTowardContainedPoint);
            maxPosition = Vector3.Max(pointTowardContainedPoint, maxPosition);
            minPosition = Vector3.Min(pointTowardContainedPoint, minPosition);
        }
        thisBounds.center = currentCenter;
        thisBounds.extents = new Vector3(Mathf.Max(maxPosition.x, -minPosition.x), Mathf.Max(maxPosition.y, -minPosition.y), 0);
    }

    void OnDrawGizmos()
    {
        if (thisBounds == null)
        {
            return;
        }
        Gizmos.DrawWireCube(thisBounds.center, thisBounds.size);
    }
}
