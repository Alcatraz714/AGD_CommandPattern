using Command.Actions;
using System.Collections.Generic;
using UnityEngine;

namespace Command.Player
{
    [CreateAssetMenu(fileName = "UnitScriptableObject", menuName = "ScriptableObjects/UnitScriptableObject")]
    public class UnitScriptableObject : ScriptableObject
    {
        public int UnitID;
        public UnitType UnitType;
        public UnitView UnitPrefab;
        public int MaxHealth;
        public int Power;
        public List<CommandType> executableCommands;
        public Vector3 EnemyBattlePositionOffset;
        public float MovementSpeed;
    }
}