using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("HUD Attributes")]
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject charmInventory;
    [SerializeField] GameObject rizzItemInventory;
    [SerializeField] Inventory inv;
    [SerializeField] GameObject charmPrefab;
    [SerializeField] GameObject rizzItemPrefab;
    [SerializeField] GameObject arrowUI;
    [SerializeField] RectTransform[] charmPositions;
    [SerializeField] RectTransform[] rizzItemPositions;
    [SerializeField] PlayerMovement player; //game manager is accesed with player.gm.gm hehe

    [Header("Item Popup Attributes")]
    [SerializeField] TextMeshProUGUI popupName;
    [SerializeField] TextMeshProUGUI popupDescript;
    [SerializeField] Image popupImage;

    [Header("NPC Stuff")]
    [SerializeField] GameObject npc_panel;

    [Header("Tutorial Panel Stuff")]
    [SerializeField] Image currentCharmImage;
    [SerializeField] TextMeshProUGUI charmName;
    [SerializeField] TextMeshProUGUI instructions;
    [SerializeField] GameObject mc;

    private List<GameObject> currCharms = new List<GameObject>();
    private List<GameObject> currRizzItems = new List<GameObject>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            resetTutorialUI();
        }
        else Destroy(this);
    }
    public void resetTutorialUI()
    {
        //basically set it back to movement
        updateTutorialUI(mc.GetComponentInChildren<SpriteRenderer>().sprite, mc.GetComponent<PickUpAble>().itemName, mc.GetComponent<PickUpAble>().description);
    }
    public void updateTutorialUI(Sprite img, string name, string instr)
    {
        currentCharmImage.sprite = img;
        charmName.SetText(name);
        instructions.SetText(instr);
    }

    private void Update()
    {
        updateDirectionUI();
    }

    public void onReset()
    {
        player.gm.gm.lose();
    }
    public void onBack()
    {
        player.gm.gm.menu();
    }
    public void updateCharmUI()
    {
        foreach(GameObject o in currCharms)
        {
            Destroy(o);
        }
        for(int i = 0; i < inv.charms.Count; i++)
        {
            Inventory.CharmData c = inv.charms[i];
            GameObject o = Instantiate(charmPrefab, canvas.transform);
            o.GetComponent<Image>().sprite = c.image;
            TextMeshProUGUI[] children = o.GetComponentsInChildren<TextMeshProUGUI>();
            foreach(TextMeshProUGUI r in children)
            {
                if(r.gameObject.name == "name")
                {
                    r.SetText(c.itemName);
                }else if(r.gameObject.name == "amt")
                {
                    r.SetText("x" + c.amt);
                }
            }
            ClickCharm cc = o.GetComponent<ClickCharm>();
            cc.type = c.type;
            cc._name = c.itemName;
            cc.desc = c.description;
            cc.player = player;
            o.GetComponent<RectTransform>().localPosition = charmPositions[i].localPosition;
            currCharms.Add(o);
        }
    }
    public void updateRizzItemUI()
    {
        foreach (GameObject o in currRizzItems)
        {
            Destroy(o);
        }
        for (int i = 0; i < inv.rizzItems.Count; i++)
        {
            Inventory.RizzItemData c = inv.rizzItems[i];
            Debug.Log("instantiated");
            GameObject o = Instantiate(rizzItemPrefab, canvas.transform);
            o.GetComponent<Image>().sprite = c.image;
            TextMeshProUGUI[] children = o.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI r in children)
            {
                if (r.gameObject.name == "name")
                {
                    r.SetText(c.itemName);
                }
                else if (r.gameObject.name == "amt")
                {
                    r.SetText("x" + c.amt);
                }
            }
            o.GetComponent<RectTransform>().localPosition = rizzItemPositions[i].localPosition;
            ClickRizzItem cr = o.GetComponent<ClickRizzItem>();
            cr._name = c.itemName;
            cr._desc = c.description;
            cr.npc_panel = npc_panel;
            currRizzItems.Add(o);
        }
    }

    public void updateDirectionUI()
    {
        if (player.rotation.x == 0 && player.rotation.y == 1)
        {
            StartCoroutine(SlowRotate(arrowUI.GetComponent<RectTransform>().rotation, Quaternion.Euler(0, 0, 0), 0.1f));
        }
        if (player.rotation.x == 0 && player.rotation.y == -1)
        {
            StartCoroutine(SlowRotate(arrowUI.GetComponent<RectTransform>().rotation, Quaternion.Euler(0, 0, 180), 0.1f));
        }
        if (player.rotation.x == 1 && player.rotation.y == 0)
        {
            StartCoroutine(SlowRotate(arrowUI.GetComponent<RectTransform>().rotation, Quaternion.Euler(0, 0, 270), 0.1f));
        }
        if (player.rotation.x == -1 && player.rotation.y == 0)
        {
            StartCoroutine(SlowRotate(arrowUI.GetComponent<RectTransform>().rotation, Quaternion.Euler(0, 0, 90), 0.1f));
        }
    }
    IEnumerator SlowRotate(Quaternion source, Quaternion target, float overTime)
    {
        float startTime = Time.time;
        while (Time.time < (startTime + overTime))
        {
            float t = (Time.time - startTime) / overTime;
            arrowUI.GetComponent<RectTransform>().rotation = Quaternion.Lerp(source, target, t); 
            yield return null;
        }
        arrowUI.GetComponent<RectTransform>().rotation = target;
    }
    public void closePopup()
    {
        //assuming the close button is a child of a panel called "Panel"
        /*
        RectTransform[] parents = GetComponentsInParent<RectTransform>(true);
        foreach (RectTransform c in parents)
        {
            if (c.gameObject.name == "Panel")
            {
                c.gameObject.SetActive(false);
                break;
            }
        }
        */
        popupName.transform.parent.gameObject.SetActive(false);
    }
    public void SetPopupAttributues(string _name, string _descript, Sprite _img)
    {
        popupName.text = _name;
        popupDescript.text = _descript;
        popupImage.sprite = _img;
    }
}
