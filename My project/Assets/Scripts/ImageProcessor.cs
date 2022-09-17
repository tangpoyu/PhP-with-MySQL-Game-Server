using UnityEngine;
using System.IO;
using SimpleJSON;



public class ImageProcessor : MonoBehaviour
{

    public static ImageProcessor instance;
    private string basePath;
    private string versionJsonPath;
    private JSONObject version;

    private void Awake()
    {
        instance = this;
        basePath = Path.Combine(Application.persistentDataPath, "Images");
        if (!Directory.Exists(basePath))
        {
            Directory.CreateDirectory(basePath);
        }
        versionJsonPath = basePath + "VersionJson";
        if (File.Exists(versionJsonPath))
        {
            string jsonString = File.ReadAllText(versionJsonPath);
            version = JSON.Parse(jsonString) as JSONObject;
        }
        else
        {
            version = new JSONObject();
        }
    }

    public void SaveImage(string fileName, byte[] bytes, int imgVer)
    {
        string filePath = Path.Combine(basePath, fileName);
        File.WriteAllBytes(filePath, bytes);
        UpdateVersionJson(fileName, imgVer);
    }

    public byte[] LoadImage(string fileName, int imgVer)
    {
        string filePath = Path.Combine(basePath, fileName);

        if (!IsImageUpToDate(fileName, imgVer))
        {
            return null;
        }

        if (File.Exists(filePath))
        {
            return File.ReadAllBytes(filePath);
        }
        else
        {
            return null;
        }
    }

    public Sprite BytesToSprite(byte[] bytes)
    {
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(bytes);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        return sprite;
    }

    public void UpdateVersionJson(string name, int ver)
    {
        version[name] = ver;
        File.WriteAllText(versionJsonPath, version.ToString());
    }

    public bool IsImageUpToDate(string name, int ver)
    {
        if(version[name] != null)
        {
            if(version[name].AsInt != ver)
            {
                Debug.Log($"item.{name} Img version : {version[name]} is less than current version : {ver}, need to reinstall.");
                return false;
            }
            else
            {
                Debug.Log($"item.{name} Img version {version[name]} is equal current version {ver}");
                return true;
            }
        }
        Debug.Log($"item.{name} Img doesn't have been installed.");
        return false;
    }
}
