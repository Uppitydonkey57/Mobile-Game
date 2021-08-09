using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFreeze : MonoBehaviour
{
    float pendingFreezeDuration;

    bool isFrozen;

    // Update is called once per frame
    void Update()
    {
        if (!isFrozen && pendingFreezeDuration > 0)
        {
            StartCoroutine(DoFreeze(pendingFreezeDuration));
        }
    }

    public void Freeze(float duration)
    {
        pendingFreezeDuration = duration;
    }

    IEnumerator DoFreeze(float duration)
    {
        float original = Time.timeScale;

        Time.timeScale = 0f;

        isFrozen = true;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = original;

        pendingFreezeDuration = 0f;

        isFrozen = false;
    }
}
