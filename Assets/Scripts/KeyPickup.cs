using UnityEngine;
using UnityEngine.Events;

public class KeyPickup : MonoBehaviour
{
    [SerializeField] private UnityEvent<GameObject> onPickup;

    private void OnTriggerEnter(Collider other)
    {
        onPickup.Invoke(gameObject);
    }

    public void LogPickupMessage(GameObject obj)
    {
        Debug.Log("Key " + obj.name + " was picked up");
    }
}
