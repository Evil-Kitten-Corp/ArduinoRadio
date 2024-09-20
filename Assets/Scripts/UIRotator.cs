using UnityEngine;

public class UIRotator : MonoBehaviour
{
    public float rotationSpeed;

    private void Update()
    {
        transform.Rotate (0f, 0f, Time.deltaTime * rotationSpeed);
    }
}
