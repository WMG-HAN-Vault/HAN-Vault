using UnityEngine;
using TMPro;
public class CanvasController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TextMeshProUGUI HanKey;

    void Start()
    {
        HanKey.text = " ";
    }
    
    // Update is called once per frame
    void Update()
    {
        GameObject[] HanKeys = GameObject.FindGameObjectsWithTag("HanTag");
        Debug.Log(HanKeys.Length);
        if (HanKeys.Length == 0)
        {
            HanKey.text = "Han Key";
        }
    }
}
