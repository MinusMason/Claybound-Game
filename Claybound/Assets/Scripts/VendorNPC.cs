using UnityEngine;

public class VendorNPC : MonoBehaviour
{
    private bool playerInRange = false;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (VendorUI.Instance != null)
            {
                if (VendorUI.Instance.IsOpen)
                    VendorUI.Instance.Close();
                else
                    VendorUI.Instance.Open();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            DiceRollUI.Instance?.ShowPrompt("Press E  —  Open Vendor");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            DiceRollUI.Instance?.HidePrompt();
            VendorUI.Instance?.Close();
        }
    }
}
