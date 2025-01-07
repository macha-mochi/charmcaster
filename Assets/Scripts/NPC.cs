using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public GameObject player;
    [SerializeField] Image panelImage;
    public GameObject[] charmToGive;
    public bool rizzed = false;

    private GameObject panel;
    public ClickRizzItem selectedRizzItem;
    // Start is called before the first frame update
    void Start()
    {
        panel = panelImage.gameObject.transform.parent.gameObject;
        Debug.Log(panel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!rizzed && collision.gameObject == player)
        {
            Debug.Log(panel);
            panel.SetActive(true);
            panel.GetComponent<NPC_Panel>().n = this;
            panelImage.sprite = charmToGive[0].GetComponentInChildren<SpriteRenderer>().sprite;
        }
    }
}

