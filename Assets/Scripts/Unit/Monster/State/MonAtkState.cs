namespace LineUpHeros
{
    // 일반 공격 스테이트
    public class MonAtkState : MonsterState
    {
        public MonAtkState(Monster monster) : base(monster)
        {
            _monster = monster;
        }

        public override void OnEnterState()
        {
            _monster.ChangeAnimationState(EnumState.Character.ATK);
        }

        public override void OnUpdateState()
        {
        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnExitState()
        {
        }

        public override void CheckChangeState()
        {
            throw new System.NotImplementedException();
        }
    }
}