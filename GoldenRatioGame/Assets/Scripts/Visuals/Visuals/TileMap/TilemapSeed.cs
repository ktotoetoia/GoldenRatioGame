using UnityEngine;

namespace IM.Visuals
{
    public class TilemapSeed : MonoBehaviour
    {
        [Tooltip("Change this number on different Tilemaps so identical tile positions generate different sprites.")]
        [SerializeField] private int _seed = 0;
        [SerializeField] private bool _chooseRandom;
        private int _randomSeed;
        private bool _randomChosen;
        
        public int Seed => _chooseRandom
                ? _randomChosen ? _randomSeed : _randomSeed = Random.Range(int.MinValue, int.MaxValue)
                : _seed;
    }
}