using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInputController : MonoBehaviour
{
    TreeNode initialNode;
    TreeCamera thisCamera;
    public static PlayerInputController instance;


    public GameObject inputTextPrefab;

    bool inputingText
    {
        get
        {
            return initialNode == thisCamera.activeNode;
        }
    }

    public string rootText;
    void Start()
    {
        instance = this;
        thisCamera = GetComponent<TreeCamera>();

        initialNode = thisCamera.activeNode;

        TMP_InputField inputField = Instantiate(inputTextPrefab, initialNode.transform.position, initialNode.transform.rotation, initialNode.transform).GetComponent<TMP_InputField>();
        inputField.onEndEdit.AddListener((newText) =>
        {
            rootText = newText;
            string[] childStrings = TreeGenerator.instance.GetChildWordsFromString(rootText);
            string[] remainingLetters = TreeGenerator.instance.GetRemainingLetters(rootText, childStrings);
            initialNode.RemoveChildren();
            initialNode.SetChildStrings(childStrings, remainingLetters);
        });
    }
    public void AddChildNodes(TreeNode thisNode)
    {
        string[] childStrings = TreeGenerator.instance.GetChildWordsFromString(thisNode.remainingLetters);
        string[] remainingLetters = TreeGenerator.instance.GetRemainingLetters(thisNode.remainingLetters, childStrings);
        thisNode.SetChildStrings(childStrings, remainingLetters);
    }

}
