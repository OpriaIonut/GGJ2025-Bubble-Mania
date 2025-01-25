using System.Collections.Generic;
using UnityEngine;

namespace BubbleMania
{
    [System.Serializable]
    public struct EnemyColorType
    {
        public BubbleType type;
        public Color color;
    }

    public class EnemyFactory : MonoBehaviour
    {
        [SerializeField] private List<EnemyColorType> enemyColor;

        private void Awake()
        {
            Locator.RegisterService(this);
        }

        public Color GetColorByType(BubbleType type)
        {
            for(int index = 0; index < enemyColor.Count; ++index)
            {
                if (enemyColor[index].type == type)
                    return enemyColor[index].color;
            }
            return Color.white;
        }
    }
}