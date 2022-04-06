using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticeController : MonoBehaviour
{
    [SerializeField] public Image itemSprite;
    [SerializeField] Image background;

    public void SetLocation(Vector3 noticePosition)
    {
        itemSprite.transform.position = GetComponent<Camera>().WorldToScreenPoint(noticePosition);
        background.transform.position = GetComponent<Camera>().WorldToScreenPoint(noticePosition);
    }

}
