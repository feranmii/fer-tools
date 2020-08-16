using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Min(0.05f)] public float openDuration = .25f;

    // [Foldout("Key")] public bool requiresKey = true;

    public bool disableOnOpen;
    public DoorKey unlockKey;

    [ReadOnly] public bool isOpen;
    [ReadOnly] public bool isLocked;
    private Tweener openTweener;
    private Tweener closeTweener;


    public Action failedUnlockAttempt;
    public Action successfulUnlock;


    private BoxCollider collider;


    private void Awake()
    {
        collider = GetComponent<BoxCollider>();

        Lock();


        openTweener = transform.DOLocalRotate(new Vector3(0, 90, 0), openDuration)
            .Pause()
            .SetAutoKill(false);

        closeTweener = transform.DOLocalRotate(new Vector3(0, 0, 0), openDuration)
            .Pause()
            .SetAutoKill(false);
    }

    public void Open()
    {
        if (isLocked || isOpen) return;

        isOpen = true;
        if (disableOnOpen) collider.enabled = false;

        closeTweener.Pause();
        openTweener.Restart();
    }

    public void Close()
    {
        if (!isOpen)
            return;

        isOpen = false;

        openTweener.Pause();
        closeTweener.Restart();
    }

    private void Lock()
    {
        isLocked = true;
    }

    public void Unlock(DoorKey doorKey)
    {
        if (doorKey == unlockKey)
        {
            print(doorKey);
            successfulUnlock?.Invoke();

            doorKey.Use();
            isLocked = false;
        }
        else
        {
            failedUnlockAttempt?.Invoke();

            isLocked = true;
            print("Failed To Unlock");
        }
    }
}