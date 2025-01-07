using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour, ISerializationCallbackReceiver
{
    public int cellSize, playerInitX, playerInitY, width, height;
    [SerializeField] private List<Package> serializable;
    public int[,] grid;
    public GameObject[] intToGameObject;
    public List<GameObject> currObjects;

    [Header("Things Related to Collecting Items")]
    [SerializeField] GameObject player;
    [SerializeField] GameObject popup;

    [Header("Int For Each Type of Block")]
    public int empty = 0;
    public int wall = 1;
    public int river = 2;
    public int fog = 3;
    public int movementCharm = 4;
    public int rotateCharm = 5;
    public int explodeCharm = 6;
    public int jumpCharm = 7;
    public int fogCharm = 8;
    public int teleportCharm = 9;
    public int wallMoveCharm = 10;
    public int pickUpLine = 11;
    public int pinkRose = 12;
    public int chocolate = 13;
    public int yellowFlower = 14;
    public int keyOrShard = 15;
    public int goalCharm = 16;

    [Header("Things Related to Winning")]
    [SerializeField] public GameManager gm;

    [System.Serializable]
    struct Package
    {
        public int Index0;
        public int Index1;
        public int value;
        public Package(int idx0, int idx1, int val)
        {
            Index0 = idx0;
            Index1 = idx1;
            value = val;
        }
    }
    public void OnBeforeSerialize()
    {
        // Convert our unserializable array into a serializable list
        serializable = new List<Package>();
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                serializable.Add(new Package(i, j, grid[i, j]));
            }
        }
    }
    public void OnAfterDeserialize()
    {
        // Convert the serializable list into our unserializable array
        grid = new int[width, height];
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                grid[i, j] = 0;
            }
        }
        foreach (var package in serializable)
        {
            grid[package.Index0, package.Index1] = package.value;
        }
    }
    void Start()
    {
        for(int i = 0; i < grid.GetLength(0); i++)
        {
            for(int j = 0; j < grid.GetLength(1); j++)
            {
                int ind = grid[i, j];
                GameObject g = intToGameObject[grid[i,j]];
                GameObject curr = Instantiate(g);
                curr.transform.position = IndToWorldSpace(i, j);
                if (ind != empty)
                {
                    GameObject baseObj = Instantiate(intToGameObject[0]);
                    baseObj.transform.position = IndToWorldSpace(i, j);
                    currObjects.Add(curr);
                }
                if (ind != empty && ind != wall && ind != river && ind != fog)
                {
                    PickUpAble p = curr.GetComponent<PickUpAble>();
                    p.player = player;
                    if (p.showPopup)
                    {
                        p.popup = popup;
                    }
                    p.posX = i;
                    p.posY = j;
                    p.gm = this;
                }
                if(ind == goalCharm)
                {
                    gm.goalCharm = curr;
                    curr.GetComponent<CircleCollider2D>().enabled = false;
                    curr.GetComponentInChildren<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
                    Debug.Log("grayed out goal charm");
                }
            }
        }
    }

    public Vector2 IndToWorldSpace(int x, int y)
    {
        return new Vector2(transform.position.x + x * cellSize, transform.position.y + y * cellSize);
    }

    public Vector2 WorldSpaceToInd(float x, float y)
    {
        int indX = (int) Mathf.Round((x - transform.position.x) / (float)cellSize);
        int indY = (int) Mathf.Round((y - transform.position.y) / (float)cellSize);
        return new Vector2(indX, indY);

    }

    public void updateGrid()
    {
        foreach (GameObject o in currObjects)
        {
            Destroy(o);
        }
        /*for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                int ind = grid[i, j];
                Debug.Log(ind);
                if(ind != 0)
                {
                    GameObject g = intToGameObject[grid[i, j]];
                    GameObject curr = Instantiate(g);
                    curr.transform.position = IndToWorldSpace(i, j);
                    currObjects.Add(curr);
                }
            }
        }*/
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                int ind = grid[i, j];
                GameObject g = intToGameObject[grid[i, j]];
                GameObject curr = Instantiate(g);
                curr.transform.position = IndToWorldSpace(i, j);
                if (ind != empty)
                {
                    GameObject baseObj = Instantiate(intToGameObject[0]);
                    baseObj.transform.position = IndToWorldSpace(i, j);
                    currObjects.Add(curr);
                }
                if (ind != empty && ind != wall && ind != river && ind != fog)
                {
                    PickUpAble p = curr.GetComponent<PickUpAble>();
                    p.player = player;
                    if (p.showPopup)
                    {
                        p.popup = popup;
                        p.img = curr.GetComponentInChildren<SpriteRenderer>().sprite;
                    }
                    p.posX = i;
                    p.posY = j;
                    p.gm = this;
                }
                if (ind == goalCharm)
                {
                    gm.goalCharm = curr;
                    Debug.Log("can collect: " + gm.canCollect);
                    if (gm.canCollect)
                    {
                        curr.GetComponent<CircleCollider2D>().enabled = true;
                        curr.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1);
                    }
                    else
                    {
                        curr.GetComponent<CircleCollider2D>().enabled = false;
                        curr.GetComponentInChildren<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
                        Debug.Log("update grid gray out goal");
                    }
                }
            }
        }
    }
}


