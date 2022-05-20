using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class User
{
    public string id;
    public string password;
    public string email;
    public string nickname;
    public int character;
    public int followNum;
    public int followerNum;
    public List<string> preferredGenres=new List<string>();
    public List<string> follow = new List<string>();
    public List<string> listName = new List<string>();


    public void SetUser(string id,string email,string nickname,int character, List<string> preferredGenres=null, List<string> followIds = null)
    {
        this.id = id;
        this.email = email;
        this.nickname = nickname;
        this.character = character;
        this.preferredGenres = preferredGenres;
        this.follow = followIds;


        if (preferredGenres == null)
            this.preferredGenres = new List<string>();
        if (followIds == null)
            this.follow = new List<string>();
        
    }

    public void Clear()
    {
        this.id = "";
        this.email = "";
        this.nickname = "";
        this.character = 0;
        this.preferredGenres.Clear();
        this.follow.Clear();
    }
    public string GetName()
    {
        return nickname+"("+id+")";
    }
}
public class UserData : Singleton<UserData>
{

    public User user;
    public string Token;

    public float[] sets;//오디오, 전체화면 유무, 해상도
    public delegate void FollowHandler();
    public event FollowHandler OnChangeFollow;

    public delegate void FollowStateHandler(string id);
    public event FollowStateHandler OnDeleteFollow;
    public event FollowStateHandler OnAddFollow;
    public string id
    {
        get { return user.id; }    // _data 반환
        set { user.id = value; }   // value 키워드 사용
    }

    // Start is called before the first frame update
    void Awake()
    {
        sets = new float[3];
        sets[0] = 0.8f;
        if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    public void ChangeFollow(List<string> strs,string id, bool isDelete = false)
    {
        user.follow = strs;
        user.followNum = user.follow.Count;
        OnChangeFollow?.Invoke();
        if (isDelete == true)
        {//삭제
            OnDeleteFollow?.Invoke(id);
        }
        else
        {//팔로우
            OnAddFollow?.Invoke(id);
        }
    }
    public void Clear()
    {
        Token = "";
        user.Clear();
    }
}
