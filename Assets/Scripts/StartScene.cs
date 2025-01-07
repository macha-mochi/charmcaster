using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyTransition;

public class StartScene : MonoBehaviour
{
    public TransitionSettings transition;

    public void StartGame()
    {
        TransitionManager.Instance().Transition(1, transition, 1);
    }
}
