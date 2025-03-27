using Command.Input;
using System.Collections.Generic;

namespace Command.Actions
{
    public class ActionService
    {
        private Dictionary<CommandType, IAction> actions;

        public ActionService() => CreateActions();

        private void CreateActions()
        {
            actions = new Dictionary<CommandType, IAction>();
            actions.Add(CommandType.Attack, new AttackAction());
            actions.Add(CommandType.Heal, new HealAction());
            actions.Add(CommandType.AttackStance, new AttackStanceAction());
            actions.Add(CommandType.Cleanse, new CleanseAction());
            actions.Add(CommandType.Meditate, new MeditateAction());
            actions.Add(CommandType.BerserkAttack, new BerserkAttackAction());
            actions.Add(CommandType.ThirdEye, new ThirdEyeAction());
        }

        public IAction GetActionByType(CommandType type)
        {
            if (actions.ContainsKey(type))
                return actions[type];
            else
                throw new System.Exception($"No Action found for the type {type} in the dictionary");
        }

        public TargetType GetTargetTypeForAction(CommandType actionType) => actions[actionType].TargetType;
    }
}