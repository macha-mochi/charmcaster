using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Vector2 previousRotation = new Vector2(0, 0);
    private Vector2 rotation = new Vector2(0, 0);
    private PlayerMovement player;
    bool isWalking = false;
    bool isJumping = false;
    bool isTeleporting = false;
    bool isClearingFog = false;
    bool isMovingWall = false;
    bool isExplodingWalls = false;

    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        player = gameObject.transform.parent.gameObject.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        rotation = player.rotation;
        if (rotation.x != previousRotation.x || rotation.y != previousRotation.y)
        {
            idle();
        }
        /*
         * when not doing anything: idle in the direction of the current rotation
         * rotate: when the rotation changes idle automatically changes too
         * walk: if in motion, cannot use the charm again until the player gets to the right spot
         * jump: if jumping, cannot do anything until the player gets to the right spot
         * teleport
         * clear fog
         * move wall
         * explode walls
         */
        previousRotation = rotation;
    }
    void idle()
    {
        if(rotation.x == 1 && rotation.y == 0)
        {
            anim.Play("idle_forward_right");
        }else if(rotation.x == 0 && rotation.y == 1)
        {
            anim.Play("idle_back");
        }else if(rotation.x == -1 && rotation.y == 0)
        {
            anim.Play("idle_left");
        }else if(rotation.x == 0 && rotation.y == -1)
        {
            anim.Play("idle_forward_right");
        }
    }
    public void walk(bool start)
    {
        if (start)
        {
            isWalking = true;
            if (rotation.x == 1 && rotation.y == 0)
            {
                anim.Play("walk_forward_right");
            }
            else if (rotation.x == 0 && rotation.y == 1)
            {
                anim.Play("walk_back");
            }
            else if (rotation.x == -1 && rotation.y == 0)
            {
                anim.Play("walk_left");
            }
            else if (rotation.x == 0 && rotation.y == -1)
            {
                anim.Play("walk_forward_right");
            }
        }
        else
        {
            isWalking = false;
            idle();
        }
    }
    public void teleport(bool start)
    {
        if (start)
        {
            isTeleporting = true;
            if (rotation.x == 1 && rotation.y == 0)
            {
                anim.Play("teleport_fade_forward_right");
            }
            else if (rotation.x == 0 && rotation.y == 1)
            {
                anim.Play("teleport_fade_back");
            }
            else if (rotation.x == -1 && rotation.y == 0)
            {
                anim.Play("teleport_fade_left");
            }
            else if (rotation.x == 0 && rotation.y == -1)
            {
                anim.Play("teleport_fade_forward_right");
            }
        }
        else
        {
            isTeleporting = false;
            if (rotation.x == 1 && rotation.y == 0)
            {
                anim.Play("teleport_solidify_forward_right");
            }
            else if (rotation.x == 0 && rotation.y == 1)
            {
                anim.Play("teleport_solidify_back");
            }
            else if (rotation.x == -1 && rotation.y == 0)
            {
                anim.Play("teleport_solidify_left");
            }
            else if (rotation.x == 0 && rotation.y == -1)
            {
                anim.Play("teleport_solidify_forward_right");
            }
        }
    }
    void actuallyTeleport()
    {
        player.updatePos();
        teleport(false);
    }
    public void clearFog()
    {
        //isClearingFog = true; do this later when you get the other vfxes in
        castSpell();
        //the spell animation should loop until the fog tile is hit by the other vfx and cleared, then switch to idle
    }
    public void moveWall()
    {
        castSpell();
        //spell anim loops until wall is in its place (lerp it if time)
    }
    public void explodeWalls()
    {
        castSpell();
        //spell anim loops until explosion vfx clears
    }
    void castSpell()
    {
        if (rotation.x == 1 && rotation.y == 0)
        {
            anim.Play("castspell_forward_right");
        }
        else if (rotation.x == 0 && rotation.y == 1)
        {
            anim.Play("castspell_back");
        }
        else if (rotation.x == -1 && rotation.y == 0)
        {
            anim.Play("castspell_left");
        }
        else if (rotation.x == 0 && rotation.y == -1)
        {
            anim.Play("castspell_forward_right");
        }
    }
}

