using Player;
using StaticTags;
using System;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public Action OnFoodEaten;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag(Tags.Player))
            return;

        PlayerController snake = other.GetComponent<PlayerController>();

        OnFoodEaten?.Invoke();
        Destroy(gameObject);
    }
}
