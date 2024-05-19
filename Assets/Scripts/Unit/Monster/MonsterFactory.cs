
using System;
using Zenject;

namespace LineUpHeros
{
    public class MonsterFactory
    {   
        [Inject]
        private GameController _gameController;
        [Inject(Id = "GoblinFactory")]
        private Monster.Factory _goblinFactory;
        [Inject(Id = "FlyingEyeFactory")]
        private Monster.Factory _flyingEyeFactory;
        
        public Monster Create(MonsterInfo info)
        {
            Monster monster = null;
            switch (info.type)
            {
                case EnumMonsterType.Goblin:
                    monster = _goblinFactory.Create();
                    break;
                case EnumMonsterType.FlyingEye:
                    monster = _flyingEyeFactory.Create();
                    break;
                case EnumMonsterType.Mushroom:
                    break;
                case EnumMonsterType.Skeleton:
                    break;
                case EnumMonsterType.KingGoblin:
                    break;
                case EnumMonsterType.KingFlyingEye:
                    break;
                case EnumMonsterType.KingMushroom:
                    break;
                case EnumMonsterType.KingSkeleton:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(info.type), info.type, null);
            }

            if (monster != null)
            {
                ApplyMonsterStatChanges(monster);
            }
            return monster;
        }

        private void ApplyMonsterStatChanges(Monster monster)
        {
            float monsterStageGrowthRate = _gameController.currentStage.Value * 10;
            monster.status.SetPerStat((int)Status.EnumStatus.Hp, monsterStageGrowthRate);
            monster.status.SetPerStat((int)Status.EnumStatus.Atk, monsterStageGrowthRate);
        }
    }
}