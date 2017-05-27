using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TestOne : MonoBehaviour
{
    private Text tex;

    void Start()
    {
        tex = this.transform.FindChild("Text").GetComponent<Text>();
        this.StartCoroutine("IE_Test");
    }

    private IEnumerator IE_Test()
    {
        string[] str = {"aa","bb","cc"};
        int idx = 0;
        while (true)
        {
            yield return new WaitForSeconds(1f);
            tex.text = str[idx];
            idx = (idx + 1) % str.Length;
        }
    }

    void Update()
    {
    }
}