using TMPro;
using UnityEngine;

namespace BubbleMania
{
    public class EnemyKillCounter : MonoBehaviour
    {
        private TextMeshProUGUI killedEnemies;

        private int enemiesKilled = 0;

        private void Awake()
        {
            Locator.RegisterService(this);
            killedEnemies = GetComponent<TextMeshProUGUI>();
            killedEnemies.text = "Enemies killed: " + enemiesKilled;
        }

        public void RegisterEnemyDeath()
        {
            enemiesKilled++;
            killedEnemies.text = "Enemies killed: " + enemiesKilled;
        }
    }
}