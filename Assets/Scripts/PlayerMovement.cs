using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    private int cellSize;
    private int posX, posY;
    private int height, width;
    public Vector2 rotation = new Vector2(0, -1);
    [SerializeField] public GridManager gm;
    private Inventory inv;
    private AnimationController ac;
    [SerializeField] int numFramesForWalk;
    [SerializeField] int numFramesForDash;
    private int elapsedFrames;

    private bool isWalking = false;
    private bool isDashing = false;
    private bool rotateCharmActivated = false;
    private bool fogCharmActivated = false;
    private bool teleportCharmActivated = false; 
    private void Start()
    {
        cellSize = gm.cellSize;
        posX = gm.playerInitX;
        posY = gm.playerInitY;
        width = gm.grid.GetLength(0);
        height = gm.grid.GetLength(1);
        inv = gameObject.GetComponent<Inventory>();
        updatePos(0, 0);
        ac = GetComponentInChildren<AnimationController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow) || 
            Input.GetKeyDown(KeyCode.RightArrow) || 
            Input.GetKeyDown(KeyCode.UpArrow) || 
            Input.GetKeyDown(KeyCode.DownArrow) || 
            Input.GetKeyDown(KeyCode.W) || 
            Input.GetKeyDown(KeyCode.A) || 
            Input.GetKeyDown(KeyCode.S) || 
            Input.GetKeyDown(KeyCode.D))
        {
            if (rotateCharmActivated)
            {
                rotate();
                rotateCharmActivated = false;
            }
            else if(inv.movementCharmsLeft > 0)
            {
                walk();
            }
        }
        if (isWalking)
        {
            walk();
        }
        if (isDashing)
        {
            jump();
        }
        if(fogCharmActivated && GameObject.FindGameObjectsWithTag("Fog").Length != 0){
            clearFog();
        }
        if (teleportCharmActivated)
        {
            teleport();
        }
    }
    public Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }
    private void updatePos(int priorX, int priorY)
    {
        gm.grid[priorX, priorY] %= 1000; 
        transform.position = gm.IndToWorldSpace(posX, posY);
        //this was causing index out of bound so im going to comment it out, if something needs to know if player is on it, just give the xPos and yPos?
        //gm.grid[posX, posY] += 1000; // divide by 1000 on an index to check if player is on it and mod 1000 to see what kind of thing it is (I'm assuming we won't have more than 999 states haha)
    }
    public void updatePos()
    {
        transform.position = gm.IndToWorldSpace(posX, posY);
    }
    public void teleport()
    {
        Debug.Log("teleported");
        if (!teleportCharmActivated)
        {
            teleportCharmActivated = true;
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 pos = GetWorldPositionOnPlane(Input.mousePosition, 0);
                Vector2 posInd = gm.WorldSpaceToInd(pos.x, pos.y);
                int priorX = posX;
                int priorY = posY;
                posX = (int)posInd.x;
                posY = (int)posInd.y;
                ac.teleport(true); //pos will be updated at the end of the fade animation
                teleportCharmActivated = false;
            }
        }
    }

    public void walk()
    {
        if (!isWalking)
        {
            Debug.Log("set up stuff");
            elapsedFrames = 0;
            //set up stuff
            //update posX and posY to the new spot
            //but dont update transform yet, lerp the actual transform
            int inputX = (int)Input.GetAxisRaw("Horizontal");
            int inputY = (int)Input.GetAxisRaw("Vertical");
            if (inputX != 0 && inputY == 0 || inputX == 0 && inputY != 0)
            {
                int newXPos = posX + inputX;
                int newYPos = posY + inputY;
                if (inputX != 0 && newXPos >= 0 && newXPos < width)
                {
                    if (gm.grid[newXPos, posY] != gm.wall && gm.grid[newXPos, posY] != gm.river)
                    {
                        posX += inputX;
                        rotation = new Vector2(inputX, 0);
                        inv.removeCharm("movement");
                        inv.movementCharmsLeft--;
                        isWalking = true;
                        ac.walk(true);
                    }
                }
                if (inputY != 0 && newYPos >= 0 && newYPos < height)
                {
                    if (gm.grid[posX, newYPos] != gm.wall && gm.grid[posX, newYPos] != gm.river)
                    {
                        posY += inputY;
                        rotation = new Vector2(0, inputY);
                        inv.removeCharm("movement");
                        inv.movementCharmsLeft--;
                        isWalking = true;
                        ac.walk(true);
                    }
                }
            }
        }
        else
        {
            //just lerp
            Debug.Log("lerping walk");
            elapsedFrames++;
            elapsedFrames %= (numFramesForWalk + 1);
            Vector2 dest = gm.IndToWorldSpace(posX, posY);
            Vector3 oldPos = gm.IndToWorldSpace(posX - (int)rotation.x, posY - (int)rotation.y);
            Vector3 newPos = new Vector3(dest.x, dest.y, 0);
            Vector3 lerpPos = Vector3.Lerp(oldPos, newPos, (float)(elapsedFrames)/numFramesForWalk);
            transform.position = lerpPos;
            if(Math.Abs(transform.position.x - dest.x) < 0.001 && Math.Abs(transform.position.y - dest.y) < 0.001)
            {
                transform.position = newPos;
                isWalking = false;
                ac.walk(false);
            }
        }
    }

    public void rotate()
    {
        Debug.Log("rotated");
        int inputX = (int)Input.GetAxisRaw("Horizontal");
        int inputY = (int)Input.GetAxisRaw("Vertical");
        if (inputX != 0 && inputY == 0 || inputX == 0 && inputY != 0) rotation = new Vector2(inputX, inputY);
        //transform.right = rotation;
    }

    public void jump()
    {
        Debug.Log("jumped. rotation " + rotation.x + " " + rotation.y);
        if (!isDashing)
        {
            elapsedFrames = 0;
            int xNew = posX + (int)rotation.x * 2;
            int yNew = posY + (int)rotation.y * 2;
            int xCheck = posX + (int)rotation.x;
            int yCheck = posY + (int)rotation.y;
            if (xNew >= 0 && xNew < width && yNew >= 0 && yNew < height && gm.grid[xCheck, yCheck] == gm.wall && gm.grid[xNew, yNew] != gm.wall && gm.grid[xNew, yNew] != gm.river)
            {
                int priorX = posX;
                int priorY = posY;
                posX = xNew;
                posY = yNew;
                Debug.Log(priorX + " " + priorY + " " + posX + " " + posY);
                isDashing = true;
                ac.walk(true);
            }
        }
        else
        {
            //lerp
            Debug.Log("lerping dash");
            elapsedFrames++;
            elapsedFrames %= (numFramesForDash + 1);
            Vector2 dest = gm.IndToWorldSpace(posX, posY);
            Debug.Log("pos: " + posX + " " + posY);
            Vector3 oldPos = gm.IndToWorldSpace(posX - (int)rotation.x*2, posY - (int)rotation.y*2);
            Vector3 newPos = new Vector3(dest.x, dest.y, 0);
            Vector3 lerpPos = Vector3.Lerp(oldPos, newPos, (float)(elapsedFrames) / numFramesForDash);
            transform.position = lerpPos;
            Debug.Log(oldPos + "  " + newPos + " " + lerpPos);
            if (Math.Abs(transform.position.x - dest.x) < 0.001 && Math.Abs(transform.position.y - dest.y) < 0.001)
            {
                transform.position = newPos;
                isDashing = false;
                ac.walk(false);
            }
        }
    }

    public void clearFog(){
        Debug.Log("clear fog");
        if (!fogCharmActivated)
        {
            fogCharmActivated = true;
        }
        else {
            if (Input.GetMouseButtonDown(0))
            {
                Collider2D overlap = Physics2D.OverlapCircle(GetWorldPositionOnPlane(Input.mousePosition, 0), 0.5f);
                //Debug.Log(overlap);
                if (overlap != null && overlap.CompareTag("Fog"))
                {
                    ac.clearFog();
                    overlap.GetComponent<ParticleSystem>().Stop();
                    fogCharmActivated = false;
                }

            }
        }
    }

    public void wallMove()
    {
        Debug.Log("move wall");
        int moveX = posX + (int)rotation.x;
        int moveY = posY + (int)rotation.y;
        if(moveX >= 0 && moveX < width && moveY >= 0 && moveY < height && gm.grid[moveX, moveY] == gm.wall)
        {
            int finalX = moveX + (int)rotation.x;
            int finalY = moveY + (int)rotation.y;
            if (finalX >= 0 && finalX < width && finalY >= 0 && finalY < height && gm.grid[finalX, finalY] != gm.wall)
            {
                ac.moveWall();
                gm.grid[moveX, moveY] = 0;
                gm.grid[finalX, finalY] = gm.wall;
                gm.updateGrid();
            }
        }
    }

    public void explode()
    {
        Debug.Log("explode walls");
        int currX, currY;
        currX = posX + 1;
        currY = posY;
        if (currX >= 0 && currX < width && currY >= 0 && currY < height && gm.grid[currX, currY] == gm.wall)
        {
            gm.grid[currX, currY] = 0;
        }
        currX = posX - 1;
        currY = posY;
        if (currX >= 0 && currX < width && currY >= 0 && currY < height && gm.grid[currX, currY] == gm.wall)
        {
            gm.grid[currX, currY] = 0;
        }
        currX = posX;
        currY = posY + 1;
        if (currX >= 0 && currX < width && currY >= 0 && currY < height && gm.grid[currX, currY] == gm.wall)
        {
            gm.grid[currX, currY] = 0;
        }
        currX = posX;
        currY = posY - 1;
        if (currX >= 0 && currX < width && currY >= 0 && currY < height && gm.grid[currX, currY] == gm.wall)
        {
            gm.grid[currX, currY] = 0;
        }
        currX = posX + 1;
        currY = posY + 1;
        if (currX >= 0 && currX < width && currY >= 0 && currY < height && gm.grid[currX, currY] == gm.wall)
        {
            gm.grid[currX, currY] = 0;
        }
        currX = posX - 1;
        currY = posY - 1;
        if (currX >= 0 && currX < width && currY >= 0 && currY < height && gm.grid[currX, currY] == gm.wall)
        {
            gm.grid[currX, currY] = 0;
        }
        currX = posX - 1;
        currY = posY + 1;
        if (currX >= 0 && currX < width && currY >= 0 && currY < height && gm.grid[currX, currY] == gm.wall)
        {
            gm.grid[currX, currY] = 0;
        }
        currX = posX + 1;
        currY = posY - 1;
        if (currX >= 0 && currX < width && currY >= 0 && currY < height && gm.grid[currX, currY] == gm.wall)
        {
            gm.grid[currX, currY] = 0;
        }
        ac.explodeWalls();
        gm.updateGrid();
    }
    public void SetRotateActivated(bool b)
    {
        rotateCharmActivated = b;
    }
}
