using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CloudShare : MonoBehaviour
{
    public string url = "请上传UpLoad.zip，在此处填写UnityUpload.php的完整HTTP地址";//服务器地址
    public string QrAPI = "https://cli.im/api/qrcode/code?text=";//草料API用于将网址转换为二维码
    public Button SaveBtn;
    public Button QrBtn;
    public RawImage m_ScreenCapture;
    public RawImage m_QrCode;
    public GameObject UI;
    string uploadName;
    string path;
    string downloadName;
    Texture2D m_downloadTex;
    void Start()
    {
        SaveBtn.onClick.AddListener(delegate ()
        {
            StartCoroutine(UploadScreen());
        });
        QrBtn.onClick.AddListener(delegate ()
        {
            StartCoroutine(Qrcodse());
        });
    }
    public Texture2D Screenshot()//截屏
    {
        int width = Screen.width;
        int height = Screen.height;

        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0, false);
        tex.Apply();

        m_ScreenCapture.texture = tex;

        return tex;
    }
    IEnumerator UploadScreen()//上传图片
    {
        UI.SetActive(false);
        yield return new WaitForEndOfFrame();

        byte[] bytes = Screenshot().EncodeToPNG();
        UI.SetActive(true);
        WWWForm form = new WWWForm();
        form.AddField("Name", UploadNameStr());
        form.AddBinaryData("post", bytes);

        WWW www = new WWW(url, form);
        StartCoroutine(PostData(www));

        System.IO.File.WriteAllBytes(path, bytes);
    }

    public string UploadNameStr()//定义上传图片的名称
    {
        uploadName = System.DateTime.Now.ToString("yyyyMMddHHmmssffff").ToString();
        path = Application.dataPath + "/Gallery/" + uploadName + ".png";//并未对本地生成的图片做任何管理

        downloadName = QrAPI + "在此处填写upload文件夹的完整HTTP地址" + uploadName + ".png";//引用草料API
        Debug.Log(uploadName);
        return uploadName;
    }
    IEnumerator PostData(WWW www)//接受服务器返回的信息
    {
        yield return www;
        Debug.Log(www.text);
    }
    IEnumerator Qrcodse()//将服务器地址通过草料API转换成为二维码
    {
        using WWW w = new WWW(downloadName);
        yield return w;
        print(w.text);
        //获取'src=" //' 后所有的数据
        string s = w.text.Substring(w.text.IndexOf("<img src=") + 12, w.text.Length - (w.text.IndexOf("<img src=") + 12));
        print(s);
        //截取src="" 内部的链接地址，不包括'//'
        string result = s.Substring(0, s.IndexOf("\""));
        print(result);
        StartCoroutine(ReturnQR(result));
    }
    IEnumerator ReturnQR(string re)//返回二维码
    {
        WWW ww = new WWW(re);
        yield return ww;
        m_downloadTex = ww.texture;
        m_QrCode.texture = m_downloadTex;

        byte[] bytes = m_downloadTex.EncodeToPNG();

        WWWForm form = new WWWForm();
        form.AddField("Name", UploadNameStr());
        form.AddBinaryData("post", bytes);

        WWW www = new WWW(url, form);
        StartCoroutine(PostData(www));

        System.IO.File.WriteAllBytes(path, bytes);
    } 
}