using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo_NPCs : MonoBehaviour
{
    [SerializeField] List<GameObject> NPCList;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        yield return new WaitForSeconds(2f);
        for(int i = 0; i< NPCList.Count; i++)
        {
            NPCList[i].gameObject.SetActive(true);
            NPCList[i].GetComponent<NPCMovementController>().Walk(0, -1);
            yield return new WaitForSeconds(1f);
        }
    }

    private void Update()
    {
        transform.position = GameManager.instance.player.transform.position;
    }
}
