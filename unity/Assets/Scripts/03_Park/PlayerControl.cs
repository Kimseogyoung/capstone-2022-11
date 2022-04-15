using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviourPunCallbacks
{

    // 플레이어 설정
    float moveSpeed = 10f;
    public bool isMoveAble = true;
    public bool isUIActable = true;

    // 상호작용 버튼
    [SerializeField] bool isInteractiveAble = false;
    public GameObject InteractiveButton;
    [SerializeField] Sprite[] buttonImages;

    // 비디오
    public bool isVideoPanelShown = false;
    public GameObject videoPanel;

    // 이모티콘 코루틴 실행여부
    private bool isEmojiRunning = false;
    private Coroutine runningEmojiCorutine;

    // 채팅
    [SerializeField] GameObject ChatPanel;

    // BusketPanel
    [SerializeField] private GameObject buskerPanel;

    // Start is called before the first frame update
    void Start()
    {
        if (this.photonView.IsMine)
        {
            InteractiveButton = FindObjectOfType<Canvas>().transform.Find("InteractiveButton").gameObject;
            videoPanel = FindObjectOfType<Canvas>().transform.Find("smallVideoPanel").gameObject;
            ChatPanel = FindObjectOfType<Canvas>().transform.Find("bigVideoPanel").gameObject.transform.Find("ChatView").gameObject;
            buskerPanel = FindObjectOfType<Canvas>().transform.Find("BuskerVideoPanel").gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoveAble)
        {
            PlyerMove();
        }

    }

    private void PlyerMove()
    {
        if (this.photonView.IsMine)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                gameObject.transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                gameObject.transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                gameObject.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                gameObject.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            }

        }
    }

    public void OnInteractiveButton(int type)
    {
        if (!isInteractiveAble)
        {
            InteractiveButton.GetComponent<Image>().sprite = buttonImages[type];
            InteractiveButton.SetActive(true);
            isInteractiveAble = true;
        }
    }

    public void OffInteractiveButton()
    {
        if (isInteractiveAble)
        {
            InteractiveButton.SetActive(false);
            isInteractiveAble = false;
        }

    }

    public void OnVideoPanel(int mode)
    {
        if (!isVideoPanelShown)
        {
            switch (mode)
            {
                case 0:
                    videoPanel.GetComponent<SmallVideoPanel>();
                    videoPanel.SetActive(true);
                    isVideoPanelShown = true;
                    break;

                case 1:
                    buskerPanel.SetActive(true);
                    buskerPanel.GetComponent<BuskerVideoPanel>().setDevice();
                    break;

                default:
                    break;
            }
        }
    }

    public void OffVideoPanel()
    {
        if (isVideoPanelShown)
        {
            ChatPanel.GetComponent<Chat>().msgList.text = "";
            ChatPanel.GetComponent<Chat>().ifSendMsg.text = "";
            videoPanel.SetActive(false);
            isVideoPanelShown = false;
        }
    }

    // 방송 초기설정
    public void initBusking()
    {
        // 캐릭터 위치 등
    }

    // -------------- 이모지 동기화 관련 함수들 -------------
    public IEnumerator sendEmoji(int emojiNum)
    {
        GameObject bubble = transform.GetChild(1).gameObject;
        bubble.SetActive(true);
        bubble.GetComponent<TextMeshPro>().text = "<sprite=" + emojiNum + ">";
        isEmojiRunning = true;

        yield return new WaitForSeconds(1f);

        bubble.SetActive(false);
        isEmojiRunning = false;

    }

    [PunRPC]
    public void callEmoji(int emojiNum)
    {
        if (isEmojiRunning)
        {
            StopCoroutine(runningEmojiCorutine);
            isEmojiRunning = false;
        }

        runningEmojiCorutine = StartCoroutine(sendEmoji(emojiNum));
    }

    public void rpcEmoji(int emojiNum)
    {
        photonView.RPC("callEmoji", RpcTarget.AllBuffered, emojiNum);
    }
    //----------------------------------------------------------------

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "InteractiveObject")
        {

        }
    }
}
