using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetPrefabPreviewController : MonoBehaviour
{
    [SerializeField] public GameObject prefabPreview;

    public void Show()
    {
        prefabPreview.SetActive(true);
    }
}
