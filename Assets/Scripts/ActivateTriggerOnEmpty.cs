using UnityEngine;

public class ActivateTriggerOnEmpty : MonoBehaviour
{
    [Header("Configurare")]
    [Tooltip("Trage aici obiectul Părinte care conține copiii ce vor fi distruși.")]
    public Transform obiectParinteSpecific;

    [Tooltip("Trage aici obiectul (sau SphereCollider-ul) care trebuie să devină Trigger.")]
    public SphereCollider sferaTinta;

    private bool actiuneExecutata = false;

    void Update()
    {

        if (actiuneExecutata) return;

        if (obiectParinteSpecific != null)
        {
            if (obiectParinteSpecific.childCount == 0)
            {
                ActiveazaTrigger();
            }
        }
    }

    void ActiveazaTrigger()
    {
        if (sferaTinta != null)
        {
            sferaTinta.isTrigger = true;
            //Debug.Log("Copiii părintelui specific au fost distruși. Sfera a devenit Trigger!");
        }
        else
        {
            //Debug.LogWarning("Nu ai asignat 'Sfera Tinta' în Inspector!");
        }

        actiuneExecutata = true;
    }
}