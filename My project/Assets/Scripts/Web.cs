using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System;
using SimpleJSON;

public class Web : MonoBehaviour
{
    [SerializeField] private TMP_InputField userName, password;
    [SerializeField] private TMP_InputField userNameR, passwordR;
    [SerializeField] private GameObject loginPanel, signPanel, mainPanel, userPanel;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform itemContanier;
    

    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(GetDate());
    }

    public void LoginButton()
    {
        StartCoroutine(Login(userName.text, password.text));
    }

    public void SignUp()
    {
        loginPanel.SetActive(false);
        signPanel.SetActive(true);
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
                if (Int32.Parse(www.downloadHandler.text) != 0)
                {
                    InitializeUI(www.downloadHandler.text);
                    loginPanel.SetActive(false);
                    mainPanel.SetActive(true);
                }
                else
                {
                    Debug.Log("Login failed");
                }
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
                InitializeUI(userName);
            }
        };
    }

    private void InitializeUI(string userId)
    {
        // GetUserData
        StartCoroutine(GetUserData(userId));
        StartCoroutine(GetUserItem(userId));
    }
    
    IEnumerator GetUserData(string userId)
    {
        WWWForm form = new WWWForm();
        form.AddField("userId", userId);

        using (UnityWebRequest request = UnityWebRequest.Post("http://localhost/UnityBackend/GetUserData.php", form))
        {
            yield return request.SendWebRequest();
            if(request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                JSONNode jsonNode = JSON.Parse(request.downloadHandler.text);
                string username = jsonNode.AsObject["username"];
                string level = jsonNode.AsObject["level"];
                string coin = jsonNode.AsObject["coin"];
                userPanel.GetComponent<UserPanel>().Initialize(username, level, coin);
            }
        }
    }

    IEnumerator GetUserItem(string userId)
    {
        WWWForm form = new WWWForm();
        form.AddField("userId", userId);

        using (UnityWebRequest request = UnityWebRequest.Post("http://localhost/UnityBackend/GetUserItem.php", form))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                int result;
                if (Int32.TryParse(request.downloadHandler.text, out result))
                {
                    if (result == 0) yield break;
                }
                Debug.Log(request.downloadHandler.text);
                JSONArray jsonArray = JSON.Parse(request.downloadHandler.text) as JSONArray;
                foreach(JSONNode jsonNode in jsonArray)
                {
                    string itemId = jsonNode.AsObject["itemId"];
                    StartCoroutine(CreateItem(itemId));
                }
            }
        }
    }

    IEnumerator CreateItem(string itemId)
    {
        WWWForm form = new WWWForm();
        form.AddField("itemId", itemId);

        using (UnityWebRequest request = UnityWebRequest.Post("http://localhost/UnityBackend/GetItemData.php", form))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                JSONNode jsonNode = JSON.Parse(request.downloadHandler.text);
                string imageUrl = jsonNode.AsObject["imageUrl"];
                string itemName = jsonNode.AsObject["name"];
                string coin = jsonNode.AsObject["price"];
                string description = jsonNode.AsObject["description"];
                GameObject item = Instantiate(itemPrefab, itemContanier);
                item.GetComponent<item>().Initialize(itemName, coin, description);
            }

        }
    }
}
