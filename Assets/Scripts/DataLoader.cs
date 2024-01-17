using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class NameDetailEntry
{
    public string hr;
    public string en;
    public string de;

    public string GetText(string languageId)
    {
        switch (languageId)
        {
            case "hr":
                return hr;
            case "en":
                return en;
            case "de":
                return de;
            default:
                return null;
        }
    }
}

[Serializable]
public class Photo
{
    public string Path;
}

[Serializable]
public class Media
{
    public string Name;
    public string Path;
    public List<Photo> Photos;
}

[Serializable]
public class Details
{
    public string id;
    public string text;
}

[Serializable]
public class Topic
{
    public NameDetailEntry Name;
    public NameDetailEntry Details; 
    public List<Media> Media;
}

[Serializable]
public class Language
{
    public string id;
    public string name;
}

[Serializable]
public class LanguageContent
{
    public List<Topic> Topics;
}

[Serializable]
public class TranslatedContents
{
    public List<Language> languages;
    public LanguageContent Contents;
}

public class DataLoader
{
    private int downloadCounter = 0;
    private string[] fileUrls = new string[]
    {
        "https://raw.githubusercontent.com/crvenkapavica/TopicExplorerData/main/data.json",
        
        "https://raw.githubusercontent.com/crvenkapavica/TopicExplorerData/main/files/images/simulation1.png",
        "https://raw.githubusercontent.com/crvenkapavica/TopicExplorerData/main/files/images/simulation2.png",
        "https://raw.githubusercontent.com/crvenkapavica/TopicExplorerData/main/files/images/simulation3.png",
        "https://raw.githubusercontent.com/crvenkapavica/TopicExplorerData/main/files/images/bird.png",
        "https://raw.githubusercontent.com/crvenkapavica/TopicExplorerData/main/files/images/feather.png",
        "https://raw.githubusercontent.com/crvenkapavica/TopicExplorerData/main/files/images/stripes.png",
        "https://raw.githubusercontent.com/crvenkapavica/TopicExplorerData/main/files/images/pyramid1.png",
        "https://raw.githubusercontent.com/crvenkapavica/TopicExplorerData/main/files/images/pyramid2.png",
        "https://raw.githubusercontent.com/crvenkapavica/TopicExplorerData/main/files/images/pyramid3.png",
        "https://raw.githubusercontent.com/crvenkapavica/TopicExplorerData/main/files/images/question1.png",
        "https://raw.githubusercontent.com/crvenkapavica/TopicExplorerData/main/files/images/question2.png",
        "https://raw.githubusercontent.com/crvenkapavica/TopicExplorerData/main/files/images/question3.png",
        "https://raw.githubusercontent.com/crvenkapavica/TopicExplorerData/main/files/images/cicada1.png",
        "https://raw.githubusercontent.com/crvenkapavica/TopicExplorerData/main/files/images/cicada2.png",
        "https://raw.githubusercontent.com/crvenkapavica/TopicExplorerData/main/files/images/cicada3.png",
        "https://raw.githubusercontent.com/crvenkapavica/TopicExplorerData/main/files/images/reinkarnacija1.png",
        "https://raw.githubusercontent.com/crvenkapavica/TopicExplorerData/main/files/images/reinkarnacija2.png",
        "https://raw.githubusercontent.com/crvenkapavica/TopicExplorerData/main/files/images/reinkarnacija3.png",

        "https://raw.githubusercontent.com/crvenkapavica/TopicExplorerData/main/files/audio/audio1.ogg",
        "https://raw.githubusercontent.com/crvenkapavica/TopicExplorerData/main/files/audio/audio2.ogg",
        "https://raw.githubusercontent.com/crvenkapavica/TopicExplorerData/main/files/audio/audio3.ogg",
        "https://raw.githubusercontent.com/crvenkapavica/TopicExplorerData/main/files/audio/audio4.ogg",
        "https://raw.githubusercontent.com/crvenkapavica/TopicExplorerData/main/files/audio/audio5.ogg",
        "https://raw.githubusercontent.com/crvenkapavica/TopicExplorerData/main/files/audio/audio6.ogg"
    };
    

    public IEnumerator DownloadFiles()
    {
        foreach (var url in fileUrls)
        {
            yield return DownloadAndSave(url, Path.GetFileName(url));
        }
    }

    private IEnumerator DownloadAndSave(string fileUrl, string fileName)
    {
        downloadCounter++;
        string savePath = Path.Combine(Application.persistentDataPath, fileName);

        using (UnityWebRequest webRequest = UnityWebRequest.Get(fileUrl))
        {
           // webRequest.downloadHandler = new DownloadHandlerFile(savePath);
            yield return webRequest.SendWebRequest();

            byte[] fileData = webRequest.downloadHandler.data;
            File.WriteAllBytes(savePath, fileData);
        }
        downloadCounter--;
    }

    public IEnumerator WaitForAllDownloads()
    {
        yield return new WaitUntil(() => downloadCounter == 0);
        Persistence.Instance.LoadData();
    }
}