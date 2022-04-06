using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnExit : StateMachineBehaviour
{
    [SerializeField] GameObject rock;

    public void OnStateExit()
    {
        Destroy(rock);
    }
}
