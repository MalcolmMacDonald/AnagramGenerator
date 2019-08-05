using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInputController : MonoBehaviour
{
    TreeNode initialNode;
    TreeCamera thisCamera;


    public TreeGenerator thisTreeGenerator;
    public GameObject inputTextPrefab;

    bool inputingText
    {
        get
        {
            return initialNode == thisCamera.activeNode;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        thisCamera = GetComponent<TreeCamera>();

        initialNode = thisCamera.activeNode;

        TMP_InputField inputField = Instantiate(inputTextPrefab, initialNode.transform.position, initialNode.transform.rotation, initialNode.transform).GetComponent<TMP_InputField>();
        inputField.onValueChanged.AddListener((newText) =>
        {
            initialNode.SetChildStrings(thisTreeGenerator.GetChildWordsFromString(newText));
        });
    }

}
