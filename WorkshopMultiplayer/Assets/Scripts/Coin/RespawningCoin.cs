using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawningCoin : Coin
{

    public event Action<RespawningCoin> OnCollected;

    private Vector3 previousPosition;

    private void Update()
    {
         if (previousPosition != transform.position)
        {
            show(true);
        }

         previousPosition = transform.position;
    }

    public override int Collect()
    {
        if(!IsServer)
        {
            show(false); 
            return 0;
        }

        if(alreadyCollected) { return 0; }

        alreadyCollected = true;

        OnCollected?.Invoke(this);

        return coinValue;
    }

    public void Reset()
    {
        alreadyCollected = false;
    }

}
