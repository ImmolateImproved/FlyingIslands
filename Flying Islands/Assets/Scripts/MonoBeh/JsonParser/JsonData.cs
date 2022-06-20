using System;

namespace JsonData
{
    [Serializable]
    public class CombatResult
    {
        public Battle[] battles;
    }

    [Serializable]
    public struct PlayerJsonData
    {
        public int id;
        public int coins;
        public int lives;

        public UnitJsonData[] units;
        public UnitJsonData[] market;
    }

    [Serializable]
    public struct Battle
    {
        public int id;
        public Team firstTeam;
        public Team secondTeam;
    }

    [Serializable]
    public struct Team
    {
        public int playerId;
        public UnitJsonData[] units;
    }

    [Serializable]
    public struct UnitJsonData
    {
        public int id;
        public int skinId;

        public int attack;
        public int health;
        public int armor;
    }

}