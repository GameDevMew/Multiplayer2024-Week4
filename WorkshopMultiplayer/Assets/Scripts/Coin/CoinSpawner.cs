using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CoinSpawner : NetworkBehaviour
{

    [SerializeField] private RespawningCoin coinPrefab;
    [SerializeField] private int maxCoin = 50;
    [SerializeField] private int coinValue = 10;
    [SerializeField] private Vector2 xSpawnRang;
    [SerializeField] private Vector2 ySpawnRang;
    [SerializeField] private LayerMask layerMask;

    private Collider2D[] coinBuffer = new Collider2D[1];

    private float coinRadius;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) { return; }
        coinRadius = coinPrefab.GetComponent<CircleCollider2D>().radius;

        for(int i = 0; i < maxCoin ; i++) 
        {
            SpawnCoin();
        }
    
    }


    private void SpawnCoin()
    {
        RespawningCoin coinInstance = Instantiate(
            coinPrefab, 
            GetSpawnPoint(), 
            Quaternion.identity);

        coinInstance.SetValue(coinValue);
        coinInstance.GetComponent<NetworkObject>().Spawn();

        coinInstance.OnCollected += HandleCoinCollected;
    }

    private void HandleCoinCollected(RespawningCoin coin)
    {
        coin.transform.position = GetSpawnPoint();
        coin.Reset();
    }

    private Vector2 GetSpawnPoint()
    {
        float x = 0;
        float y = 0;

        while (true)
        {
            x = Random.Range(xSpawnRang.x, xSpawnRang.y);
            y = Random.Range(ySpawnRang.x, ySpawnRang.y);
            Vector2 spawnPoint = new Vector2(x, y); 
            ContactFilter2D contactFilter2D = new ContactFilter2D();
            contactFilter2D.layerMask = layerMask ;
            int numColliders = Physics2D.OverlapCircle(spawnPoint, coinRadius, contactFilter2D, coinBuffer);
            if(numColliders == 0)
            {
                return spawnPoint;
            }

        }
    }
}
