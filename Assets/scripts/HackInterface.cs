using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackInterface : MonoBehaviour, IActivatable
{
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private string tooltipMessage = "Kutvliegen";

    private bool isActivated = false;

    public string TooltipMessage
    { get { return tooltipMessage; } }

    public void Activate()
    {
        if (isActivated || target == null)
            return;
        
        IHackable hackable = target.GetComponent<IHackable>();

        if (hackable != null)
            hackable.Hack();

        StartCoroutine(ActivateCoroutine());

        isActivated = true;
    }

    private IEnumerator ActivateCoroutine()
    {
        Material material = GetComponentInChildren<Renderer>().material;
        Color colorA = Color.white;
        Color colorB = Color.green;

        float t = 0f;

        while (t < 1f)
        {
            material.color = Color.Lerp(colorA, colorB, (1 - Mathf.Cos(2 * Mathf.PI * t)) / 2);

            t += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        material.color = colorA;
    }
}
