using UnityEngine;

namespace GameSoundQuiz.Data.StaticData
{
    [CreateAssetMenu(fileName = "Application Config", menuName = "Static Data/Application Config")]
    public class ApplicationConfig : ScriptableObject
    {
        public int MaxPlayersInLobby;
        public int MinPlayersInLobby;
    }
}