using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Persistence : MonoBehaviour
{
    public TranslatedContents translatedContents;

    private static Persistence instance;
    private DataLoader dataLoader = new DataLoader();
    private Dictionary<int, AudioClip> audioClips = new Dictionary<int, AudioClip>();
    private List<int> sceneIDs = new List<int>();
    private TextAsset jsonFile;
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            StartCoroutine(dataLoader.DownloadFiles());
        }   
        else
        {
            Destroy(gameObject);
        }
    }

    public static Persistence Instance
    {
        get { return instance; }
    }

    private void Start()
    {
        StartCoroutine(dataLoader.WaitForAllDownloads());
    }

    public void LoadData()
    {
        string fileName = "data.json";
        string path = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            translatedContents = JsonUtility.FromJson<TranslatedContents>(jsonData);
        }

        int topicID = 0;
        foreach (Topic topic in translatedContents.Contents.Topics) 
        {
            foreach (Media media in topic.Media)
            {
                if (media.Name == "Audio")
                {
                    StartCoroutine(LoadAudioClip(topicID, media.Path));
                }
            }
            ++topicID;
        }

        GameObject.FindObjectOfType<LanguageButtons>().CreateLanguageButtons();
    }

    private IEnumerator LoadAudioClip(int ID, string relativePath)
    {
        string path = "file:///" + Path.Combine(Application.persistentDataPath, relativePath);
        
        using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.OGGVORBIS))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(request);
                audioClips[ID] = clip;
            }
        }
    }

    public void Write(string fileName, string text)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);

        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.WriteLine(text);
        }
    }

    public string Read(string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                return reader.ReadLine();
            }
        }
        else
        {
            return "";
        }
    }

    public void GoToScene(int ID)
    {
        sceneIDs.Add(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(ID);
    }

    public void GoBackToPreviousScene()
    {
        int ID = sceneIDs[sceneIDs.Count - 1];
        sceneIDs.RemoveAt(sceneIDs.Count - 1);
        SceneManager.LoadScene(ID);
    }

    public void GetAudioClip(int ID, Action<AudioClip> onClipReady)
    {
        if (audioClips.ContainsKey(ID))
        {
            onClipReady?.Invoke(audioClips[ID]);
        }
        else
        {
            StartCoroutine(WaitForClipDownload(ID, onClipReady));
        }
    }

    private IEnumerator WaitForClipDownload(int ID, Action<AudioClip> onClipReady)
    {
        yield return new WaitUntil(() => audioClips.ContainsKey(ID));
        onClipReady?.Invoke(audioClips[ID]);
    }
}

