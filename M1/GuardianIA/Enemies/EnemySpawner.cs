using UnityEngine;

namespace M1.GuardianIA.Enemies
{
    public class EnemySpawner: MonoBehaviour
    {
        public GuardianEnemy prefab;
        public Transform upperLimit, lowerLimit, fortressEntrance, guardian;
        public float spawnRate, spawnRateReductionPercent;
        
        private float currentSpawnRate;
        private float lastSpawnTime;

        void Start()
        {
            currentSpawnRate = spawnRate;
        }
        
        void Update()
        {
            if ((lastSpawnTime += Time.deltaTime) < currentSpawnRate) return;
            
            currentSpawnRate -= (spawnRateReductionPercent * spawnRate);
            lastSpawnTime = 0;
            
            var spawnPosition = Vector2.Lerp(upperLimit.position, lowerLimit.position, Random.value);
            
            Instantiate(prefab, spawnPosition, Quaternion.identity).target = Random.value < .5f ? guardian: fortressEntrance;
            
            currentSpawnRate = Mathf.Max(0.33f, currentSpawnRate);
        }
    }
}