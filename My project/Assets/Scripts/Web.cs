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

    private string userId;

    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(GetDate());
    }

    public void LoginButton()
    {
        if (userName.text == "")
        {
            Debug.Log("userName inputfield is null");
            return;
        }

        if (password.text == "")
        {
            Debug.Log("password inputfield is null");
            return;
        }

        StartCoroutine(Login(userName.text, password.text));
    }

    public void SignUp()
    {
        loginPanel.SetActive(false);
        signPanel.SetActive(true);
    }

    public void RigisterButton()
    {
        if(userNameR.text == "")
        {
            Debug.Log("userName inputfield is null");
            return;
        }

        if(passwordR.text == "")
        {
            Debug.Log("password inputfield is null");
            return;
        }

        StartCoroutine(Register(userNameR.text, passwordR.text));
    }

    public void SellItem(string itemId)
    {
        StartCoroutine(SellItemCoroutine(itemId));
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
                switch (Int32.Parse(www.downloadHandler.text))
                {
                    case -2:
                        Debug.Log("input of password error.");
                        break;

                    case -3:
                        Debug.Log("no username matches.");
                        break;

                    case -4:
                        Debug.Log("database error.");
                        break;

                    default:
                        userId = www.downloadHandler.text;
                        UpdateUI(www.downloadHandler.text);
                        loginPanel.SetActive(false);
                        mainPanel.SetActive(true);
                        break;
                };
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
                switch (www.downloadHandler.text)
                {
                    case "-1" :
                        Debug.Log("Backend error.");
                        break;

                    case "-2":
                        Debug.Log("This username is already taken.");
                        break;

                    default:
                        Debug.Log($"Build {userName} account.");
                        UpdateUI(www.downloadHandler.text);
                        signPanel.SetActive(false);
                        mainPanel.SetActive(true);
                        break;
                }
            }
        };
    }

    private void UpdateUI(string userId)
    {
        Debug.Log("Update UI.");
        // GetUserData
        foreach(Transform transform in itemContanier)
        {
            Destroy(transform.gameObject);
        }

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
                int imgVer = jsonNode.AsObject["imgVer"].AsInt;
                string imageUrl = jsonNode.AsObject["imageUrl"];
                string itemName = jsonNode.AsObject["name"];
                string coin = jsonNode.AsObject["price"];
                string description = jsonNode.AsObject["description"];
                GameObject item = Instantiate(itemPrefab, itemContanier);
                item.GetComponent<item>().Initialize(itemId,itemName, coin, description);
                byte[] imageBytes = ImageProcessor.instance.LoadImage(itemId,imgVer);
                if(imageBytes == null) StartCoroutine(GetItemIcon(itemId, item, imgVer));
                else
                {
                    Debug.Log("item " + itemId + " icon already has.");
                    Sprite sprite = ImageProcessor.instance.BytesToSprite(imageBytes);
                    item.GetComponent<item>().SetImage(sprite);
                }
            }

        }
    }

    IEnumerator SellItemCoroutine(string itemId)
    {
        WWWForm form = new WWWForm();
        form.AddField("itemId", itemId);
        form.AddField("userId", userId);

        using (UnityWebRequest request = UnityWebRequest.Post("http://localhost/UnityBackend/SellItem.php", form))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("Sell Sucess.");
                UpdateUI(userId);
            }
        }
    }

    IEnumerator GetItemIcon(string itemId, GameObject item, int imgVer)
    {
        WWWForm form = new WWWForm();
        form.AddField("itemId", itemId);
        using(UnityWebRequest request = UnityWebRequest.Post("http://localhost/UnityBackend/GetItemIcon.php", form))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log("Download item icon failed : " + request.error + ", itemId :" + itemId);
            }
            else
            {
                Debug.Log("Download item icon successfully. itemId : " + itemId);
                byte[] bytes = request.downloadHandler.data;
                ImageProcessor.instance.SaveImage(itemId, bytes, imgVer);
                Sprite sprite = ImageProcessor.instance.BytesToSprite(bytes);
                item.GetComponent<item>().SetImage(sprite);
            }
        }
    }
}
