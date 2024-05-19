
using System;
using Zenject;

namespace LineUpHeros
{
    public class MonsterFactory
    {   
        [Inject(Id = "GoblinFactory")]
        private Monster.Factory _goblinFactory;
        [Inject(Id = "FlyingEyeFactory")]
        private Monster.Factory _flyingEyeFactory;
        
        public Monster Create(MonsterInfo info)
        {
            switch (info.type)
            {
                case EnumMonsterType.Goblin:
                    return _goblinFactory.Create();
                case EnumMonsterType.FlyingEye:
                    return _flyingEyeFactory.Create();
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

            return null;
        }
    }
}