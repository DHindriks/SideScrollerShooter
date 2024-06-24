using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    enum heightLayers
    {
        L1,
        L2,
        L3
    };

    [SerializeField] Rigidbody rb;
    [SerializeField] int speed;
    [SerializeField] GameObject CollExplosionPrefab;

    [SerializeField] GameObject GameOverScreen;
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] TextMeshProUGUI Scoretext;
    [SerializeField] ParticleSystem DeathEffect;

    heightLayers Currentlayer = heightLayers.L2;
    bool SwitchingLayers = false;
    float LayerSwitchTimeS = .75f;

    Vector3 fp;   //First touch position
    Vector3 lp;   //Last touch position
    float dragDistance = 100;  //minimum distance for a swipe to be registered
    float TouchTimeStamp;

    [SerializeField]bool AllowControls = true;
    public List<WeaponBase> Weapons;

    GameManager gameManager;
    ShipData shipData;

    public Transform BackPlane;
    public Camera MainCam;
    public Transform CrosshairHolder;

    public int Health = 3; //amount of hits the player can survive

    void Start()
    {
        SetControls(false);
        gameManager = GameManager.Instance;
        GetSkinAndWeapons();
    }

    void GetSkinAndWeapons()
    {
        //remove any other objects
        foreach(Transform obj in transform.GetChild(0))
        {
            Destroy(obj.gameObject);
        }
        //get the skin, set data ref
        GameObject skin = Instantiate(gameManager.CurrentShip.gameObject, transform.GetChild(0));
        shipData = skin.GetComponent<ShipData>();
        skin.transform.position = transform.GetChild(0).position;

        //remove any other objects from weapon holder
        foreach (Transform obj in transform.GetChild(1))
        {
            Destroy(obj.gameObject);
        }

        for(int i = 0; i < shipData.WeaponSlots.Count; i++)
        {
            GameObject wpn = Instantiate(gameManager.weapons[i].gameObject);
            wpn.transform.SetParent(shipData.WeaponSlots[i]);
            wpn.transform.position = shipData.WeaponSlots[i].position;
        }
        SetControls(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyProjectile")
        {
            Health--;

            GameObject collpart = Instantiate(CollExplosionPrefab);
            collpart.transform.position = other.transform.position;
            Destroy(other.gameObject);

            // if health 0, game over
            if (Health <= 0)
            {
                SetControls(false);
                DeathEffect.Play();
                Invoke("OpenGameOverscreen", 4);
            }
        }

    }

    void OpenGameOverscreen()
    {
        Scoretext.text = "Score: " + scoreManager.Score;
        GameOverScreen.SetActive(true);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void SetControls(bool enabled)
    {
        AllowControls = enabled;
        foreach (WeaponBase weapon in Weapons)
        {
            weapon.AllowControls = enabled;
        }
    }

    void Update()
    {
        if (AllowControls)
        {

            //transform.position += transform.forward * 10 * Time.deltaTime;
            rb.AddForce((transform.forward * speed) * Time.deltaTime);

            if (Input.touchCount >= 1)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    fp = touch.position;
                    lp = touch.position;
                    TouchTimeStamp = Time.time;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    lp = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended && (TouchTimeStamp + 1) > Time.time)
                {
                    lp = touch.position;

                    //Check if drag distance is greater than 20% of the screen height
                    if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                    {//It's a drag
                     //check if the drag is vertical or horizontal
                        if (Mathf.Abs(lp.x - fp.x) < Mathf.Abs(lp.y - fp.y))
                        {
                            if (lp.y > fp.y)  //If the movement was up
                            {   //Up swipe
                                Debug.Log("Up Swipe");
                                LayerUp();
                            }
                            else
                            {   //Down swipe
                                Debug.Log("Down Swipe");
                                LayerDown();
                            }
                        }
                    }
                }
            }
        }
    }

    public void LayerUp()
    {
        switch (Currentlayer)
        {
            case heightLayers.L1:
                SwitchHeight(heightLayers.L2);
                break;
            case heightLayers.L2:
                SwitchHeight(heightLayers.L3);
                break;
            case heightLayers.L3:
                Debug.LogWarning("cannot go Higher");
                break;
        }
    }

    public void LayerDown()
    {
        switch (Currentlayer)
        {
            case heightLayers.L3:
                SwitchHeight(heightLayers.L2);
                break;
            case heightLayers.L2:
                SwitchHeight(heightLayers.L1);
                break;
            case heightLayers.L1:
                Debug.LogWarning("cannot go Lower");
                break;
        }
    }

    void SwitchHeight(heightLayers NewLayer)
    {
        if (SwitchingLayers)
        {
            return;
        }
        Currentlayer = NewLayer;
        IEnumerator Move;
        switch (Currentlayer)
        {
            case heightLayers.L1:
                Move = MoveLayers(15);
                StartCoroutine(Move);
                break;
            case heightLayers.L2:
                Move = MoveLayers(20);
                StartCoroutine(Move); 
                break;
            case heightLayers.L3:
                Move = MoveLayers(25);
                StartCoroutine(Move);
                break;
        }
    }

    IEnumerator MoveLayers(float NewHeight)
    {
        SwitchingLayers = true;
        float EndTime = Time.time + LayerSwitchTimeS;
        float CurrentHeight = transform.position.y;
        float CurrentProgress;
        float StartTime = Time.time;
        float CurrentY;
        while (Time.time < EndTime)
        {
            CurrentProgress = Mathf.InverseLerp(StartTime, EndTime, Time.time);
            CurrentY = Mathf.Lerp(CurrentHeight, NewHeight, CurrentProgress);
            transform.position = new Vector3(transform.position.x, CurrentY, transform.position.z);
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, NewHeight, transform.position.z);
        SwitchingLayers = false;
        yield return null;
    }
}
