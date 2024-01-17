using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioPlayerController : MonoBehaviour
{
    public AudioSource audioSource;
    public Slider progressBar;
    public TextMeshProUGUI timeText;
    public RectTransform progressBarRectTransform;
    public Button buttonPlayPause;
    public Button buttonNextSong;
    public Sprite playIcon;
    public Sprite pauseIcon; 

    protected bool bIsAudioPaused = false;

    private Image buttonPlayPauseImage;
    private bool bIsSeeking = false;
    

    private void Start()
    {
        progressBar.onValueChanged.AddListener(delegate { OnProgressBarChanged(); });

        buttonPlayPause.onClick.AddListener(() => OnPlayPauseClicked());
        buttonPlayPauseImage = buttonPlayPause.GetComponent<Image>();

        if (buttonNextSong != null)
        {
            buttonNextSong.onClick.AddListener(() => OnNextSongClicked());
        }
    }

    private void Update()
    {
        AutoPlay();

        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            CheckInputForProgressBarRect();
        }
    }

    private void CheckInputForProgressBarRect()
    {
        Vector2 localMousePosition = progressBarRectTransform.InverseTransformPoint(Input.mousePosition);
        if (progressBarRectTransform.rect.Contains(localMousePosition))
        {
            float normalizedPosition = Mathf.InverseLerp(progressBarRectTransform.rect.xMin, progressBarRectTransform.rect.xMax, localMousePosition.x);
            progressBar.value = normalizedPosition * progressBar.maxValue;
            OnProgressBarChanged();
        }
    }

    public void UpdateProgressBar()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            if (!bIsSeeking && audioSource.isPlaying)
            {
                progressBar.value = audioSource.time / audioSource.clip.length;
            }
            timeText.text = $"{FormatTime(audioSource.time)} / {FormatTime(audioSource.clip.length)}";
        }
        else
        {
            if (!bIsSeeking)
            {
                progressBar.value = 0;
            }
        }
    }

    private void OnProgressBarChanged()
    {
        if (audioSource.clip != null && bIsSeeking)
        {
            audioSource.time = progressBar.value * audioSource.clip.length;
        }
    }

    public void OnBeginSeeking()
    {
        bIsSeeking = true;
    }

    public void OnEndSeeking()
    {
        StartCoroutine(EndSeekingCoroutine());
    }

    private IEnumerator EndSeekingCoroutine()
    {
        yield return new WaitForEndOfFrame();

        bIsSeeking = false;

        if (audioSource.clip != null)
        {
            audioSource.time = progressBar.value * audioSource.clip.length;
        }
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = (int)timeInSeconds / 60;
        int seconds = (int)timeInSeconds % 60;
        return $"{minutes:00}:{seconds:00}";
    }

    private void OnPlayPauseClicked()
    {
        if (audioSource.isPlaying)
        {
            bIsAudioPaused = true;
            audioSource.Pause();
            buttonPlayPauseImage.sprite = playIcon;
        }
        else
        {
            bIsAudioPaused = false;
            if (audioSource.clip == null)
            {
                Persistence.Instance.GetAudioClip(0, (AudioClip clip) => { audioSource.clip = clip; });
            }
            audioSource.Play();
            buttonPlayPauseImage.sprite = pauseIcon;
        }
    }

    private void OnNextSongClicked()
    {
        audioSource.time = 0;
        AudioClip tempClip = null;

        if (audioSource.clip != null) 
        {
            do {
                int ID = Random.Range(0, 6);
                Persistence.Instance.GetAudioClip(ID, (AudioClip clip) => { tempClip = clip; });
            } while (tempClip == audioSource.clip);
        }
        else
        {
            OnPlayPauseClicked();
        }

        audioSource.clip = tempClip;

        if (!audioSource.isPlaying)
        {
            OnPlayPauseClicked();
        }
    }

    private void AutoPlay()
    {
        if (!audioSource.isPlaying && !bIsAudioPaused)
        {
            {
                audioSource.time = 0;
                audioSource.Play();
            }
        }
    }
}
