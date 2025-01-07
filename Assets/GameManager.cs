using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyTransition;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [SerializeField] int numShardsNeeded;
    public int numShardsHave = 0;
    public GameObject goalCharm;
    public bool canCollect = false;

    public TransitionSettings transitionType;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!canCollect && numShardsHave == numShardsNeeded)
        {
            goalCharm.GetComponent<CircleCollider2D>().enabled = true;
            goalCharm.GetComponentInChildren<SpriteRenderer>().color = new Color(255, 255, 255);
            canCollect = true;
        }
    }
    public void win()
    {
        //called when the player collects the goal charm
        TransitionManager.Instance().Transition(SceneManager.GetActiveScene().buildIndex + 1, transitionType, 1);
        //maybe play some vfx?
    }
    public void lose()
    {
        //maybe have a restart UI?
        TransitionManager.Instance().Transition(SceneManager.GetActiveScene().buildIndex, transitionType, 1);
    }

    public void menu()
    {
        TransitionManager.Instance().Transition(0, transitionType, 1);
    }
}
