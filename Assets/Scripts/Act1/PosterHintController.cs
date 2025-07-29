using UnityEngine;
using TMPro;

public class PosterHintController : MonoBehaviour
{
    public static PosterHintController I;

    public GameObject hintTextObject; // Drag your TMP object here

    private void Awake()
    {
        I = this;
    }

    public void HideHint()
    {
        hintTextObject.SetActive(false);
    }
}
