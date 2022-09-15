using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class Web : MonoBehaviour
{
    [SerializeField] private TMP_InputField userName, password;
    [SerializeField] private TMP_InputField userNameR, passwordR;

    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(GetDate());
    }

    public void LoginButton()
    {
        StartCoroutine(Login(userName.text, password.text));
    }

    public void RigisterButton()
    {
        StartCoroutine(Register(userNameR.text, passwordR.text));
    }

    IEnumerator GetDate()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost/UnityBackend/GetUsers.php"))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                byte[] results = www.downloadHandler.data;
            }
        }
    }

    IEnumerator Login(string userName, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("User", userName);
        form.AddField("Pass", password);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/UnityBackend/Login.php", form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        };
    }

    IEnumerator Register(string userName, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("User", userName);
        form.AddField("Pass", password);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/UnityBackend/Register.php", form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        };
    }
}
