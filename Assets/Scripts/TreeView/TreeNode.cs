using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TreeNode : MonoBehaviour
{

    public TreeNode parentNode;
    public List<TreeNode> childNodes;
    List<LineRenderer> lineRenderers;
    public float borderRadius;
    public GameObject nodePrefab;
    public GameObject linePrefab;


    Vector2 velocity;

    public float clingToParentStrength;
    public float avoidanceStrength;
    public float movementSpeedDamping;
    public float allignToParentPercent;
    public int childIndex;

    Rigidbody2D rb;
    TMP_Text thisText;
    public string currentText;
    public string remainingLetters;
    void Awake()
    {
        childNodes = new List<TreeNode>();
        lineRenderers = new List<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
        thisText = GetComponentInChildren<TMP_Text>();
        LineRenderer[] childLineRenderers = GetComponentsInChildren<LineRenderer>();
        for (int i = 0; i < childLineRenderers.Length; i++)
        {
            Destroy(childLineRenderers[i].gameObject);
        }
    }

    public void Update()
    {
        while (lineRenderers.Count != childNodes.Count)
        {
            if (lineRenderers.Count > childNodes.Count)
            {
                Destroy(lineRenderers[0].gameObject);
                lineRenderers.RemoveAt(0);
            }
            else
            {
                lineRenderers.Add(Instantiate(linePrefab, transform.position, Quaternion.identity, transform).GetComponent<LineRenderer>());
            }
        }


        if (parentNode != null)
        {

            Vector3 parentGravity = (parentNode.transform.position - transform.position);

            parentGravity = parentGravity.normalized * parentGravity.sqrMagnitude * clingToParentStrength;

            velocity += (Vector2)parentGravity;
        }
        else
        {
            rb.position = Vector2.zero;

        }
        TreeNode[] otherNodes = FindObjectsOfType<TreeNode>();//.Where(s => Vector3.Distance(transform.position, s.transform.position) < destinationDistance).ToArray();

        for (int i = 0; i < otherNodes.Length; i++)
        {
            int downstreamRelativeIndex = Mathf.Max((otherNodes[i].childIndex - childIndex) - 1, 0);
            if (downstreamRelativeIndex >= 0 && !childNodes.Contains(otherNodes[i]))
            {
                float downstreamMultiplier = 1 / (downstreamRelativeIndex + 1);

                Vector3 borderGravity = (otherNodes[i].transform.position - transform.position);
                if (borderGravity.sqrMagnitude != 0)
                {
                    borderGravity = -borderGravity.normalized * 1 / borderGravity.sqrMagnitude * avoidanceStrength;
                }
                velocity += (Vector2)borderGravity * downstreamMultiplier;
            }
        }

        velocity *= movementSpeedDamping;

        if (parentNode != null)
        {
            velocity = Vector2.Lerp(velocity, parentNode.velocity, allignToParentPercent);
        }
        Debug.DrawRay(transform.position, velocity, Color.red);

        rb.velocity = velocity;

        if (parentNode == null)
        {
            rb.velocity = Vector2.zero;
            rb.position = Vector2.zero;
        }



        for (int i = 0; i < childNodes.Count; i++)
        {
            lineRenderers[i].startColor = Color.HSVToRGB((childIndex / 10f) % 1f, 1, 1);
            lineRenderers[i].endColor = Color.HSVToRGB((childNodes[i].childIndex / 10f) % 1f, 1, 1);

            lineRenderers[i].SetPosition(0, GetNearestPointOnBorder(childNodes[i].transform.position, borderRadius));
            lineRenderers[i].SetPosition(1, childNodes[i].GetNearestPointOnBorder(transform.position, childNodes[i].borderRadius));

            float distanceToOtherNode = Vector3.Distance(transform.position, childNodes[i].transform.position);
            lineRenderers[i].enabled = distanceToOtherNode > (borderRadius + childNodes[i].borderRadius);
        }
    }

    public Vector3 GetNearestPointOnBorder(Vector3 otherPoint, float borderSize)
    {
        Vector3 pointToOtherPoint = otherPoint - transform.position;
        pointToOtherPoint = pointToOtherPoint.normalized * borderSize;
        return transform.position + pointToOtherPoint;
    }
    public void SpawnChild()
    {
        Vector3 newPositionOffset = (Vector3)(Random.insideUnitCircle.normalized * borderRadius * 2);
        TreeNode spawnedChild = Instantiate(nodePrefab, transform.position + newPositionOffset, Quaternion.identity, transform.parent).GetComponent<TreeNode>();
        spawnedChild.parentNode = this;
        spawnedChild.childIndex = childIndex + 1;
        childNodes.Add(spawnedChild);
    }
    public void RemoveChild(int index)
    {
        childNodes[index].RemoveChildren();
        Destroy(childNodes[index].gameObject);
        childNodes.RemoveAt(index);
        Destroy(lineRenderers[0].gameObject);
        lineRenderers.RemoveAt(index);
    }
    public void RemoveChildren()
    {
        while (childNodes.Count > 0)
        {
            RemoveChild(0);
        }
    }

    public void SetText(string _newText, string _remainingLetters)
    {
        currentText = _newText;
        remainingLetters = _remainingLetters;
        thisText.text = currentText;
        thisText.color = remainingLetters.Length > 0 ? Color.white : Color.green;


    }
    public void SetChildStrings(string[] childStrings, string[] remainingLetters)
    {
        while (childNodes.Count != childStrings.Length)
        {
            if (childNodes.Count > childStrings.Length)
            {
                RemoveChild(0);
            }
            else
            {
                SpawnChild();
            }
        }
        for (int i = 0; i < childStrings.Length; i++)
        {
            childNodes[i].SetText(childStrings[i], remainingLetters[i]);
        }
    }
}
