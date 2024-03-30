using UnityEngine;

[DefaultExecutionOrder(-50)]
public class SetRoot : MonoBehaviour
{
    private void Awake()
    {
        transform.SetParent(null);
    }
}
