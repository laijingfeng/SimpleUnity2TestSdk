using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public Button btnDoLogin;
    public Button btnDoSwitchAccount;
    public Button btnTest;
    public Text tex;

    void Start()
    {
        btnDoLogin.onClick.AddListener(() =>
        {
            UnityCallAnroid("DoLogin", false);
        });

        btnDoSwitchAccount.onClick.AddListener(() =>
        {
            UnityCallAnroid("DoSwitchAccount", false);
        });

        btnTest.onClick.AddListener(() =>
        {
            btnTest.gameObject.AddComponent<TestOne>();
        });
    }

    private void DoLoginCallBack(string account)
    {
        tex.text += "\n" + account;
    }

    void Update()
    {
    }

    private void UnityCallAnroid(string methodName, bool isStatic = false, params object[] args)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                if (isStatic)
                {
                    jo.CallStatic(methodName, args);
                }
                else
                {
                    jo.Call(methodName, args);
                }
            }
        }
#endif
    }

    private T UnityCallAnroid<T>(string methodName, bool isStatic = false, params object[] args)
    {
        T ret = default(T);

#if UNITY_ANDROID && !UNITY_EDITOR
        using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                if (isStatic)
                {
                    ret = jo.CallStatic<T>(methodName, args);
                }
                else
                {
                    ret = jo.Call<T>(methodName, args);
                }
            }
        }
#endif
        return ret;
    }
}