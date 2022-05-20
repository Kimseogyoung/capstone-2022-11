using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

[System.Serializable]
public class SongSlot : MonoBehaviour, IPointerDownHandler
{
    
    [SerializeField]
    protected Music music;
    public Image image;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI artistText;
    public Toggle toggle;
    public Button delBtn;
    protected Image backImage;
    public TextMeshProUGUI timeText;

    private bool select = false;

    public delegate void DeleteHandler(SongSlot ss);
    public event DeleteHandler OnDeleteButtonClick;

    public delegate void ClickHandler(SongSlot ss);
    public event ClickHandler OnClickSlot;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnClickSlot?.Invoke(this);
    }
    private void Awake()
    {
        backImage = GetComponent<Image>();
    }
    void Start()
    {
        if(toggle!=null)
            toggle.isOn = false;
        if (delBtn != null)
        {
            delBtn.gameObject.SetActive(false);
            delBtn.onClick.AddListener(delegate{ OnDeleteButtonClick?.Invoke(this); });
        }
            
    }
    public bool isSelected
    {
        get { return select; }
        set
        {
            select = value;
            if (select)
            {
                SetImage(new Color(1.0f, 0.7f, 0.7f));
            }
            else
            {
                SetImage(new Color(1, 1, 1));
            }


        }
    }
    public void SetTime()
    {
        if (timeText != null)
        {
            timeText.text = (int)music.time/60 +":"+(int)music.time % 60;
        }
    }
    public Music GetMusic()
    {
        return music;
    }
    public void SetMusic(Music _music)
    {
        music = _music;
        titleText.text = music.title;

        artistText.text = music.GetArtistName();
        LoadImage(music.imageLocate);
        if (toggle != null)
            toggle.isOn = false;
        SetTime();
    }
    private void LoadImage(string filePath)
    {
        if (filePath == null || filePath == "")
        {
            image.sprite = Resources.Load<Sprite>("Image/none"); 
        }
        else
        {
            StartCoroutine(GetTexture(filePath));
        }
    }
    public void SetImage(Color color)
    {
        backImage.color = color;
    }
    IEnumerator GetTexture(string _path)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://"+_path);
        yield return www.SendWebRequest();
        
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("GetTextrue : "+www.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Rect rect = new Rect(0, 0, texture.width, texture.height);
            image.sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
