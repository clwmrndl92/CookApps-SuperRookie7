using UnityEngine;

namespace LineUpHeros
{
    public abstract class BossState : MonsterState
    {
        protected BossMonster _monster;
        protected BossState(BossMonster boss) : base(boss)
        {
            _monster = boss;
        }
    }
}