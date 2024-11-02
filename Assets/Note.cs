using UnityEngine;

public class Note : MonoBehaviour
{
    //[Header("Note for this asset")]
    [TextArea(10, 15)]
    [SerializeField] string note;
}
