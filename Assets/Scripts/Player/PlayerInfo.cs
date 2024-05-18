using UniRx;
using Zenject;

namespace LineUpHeros
{
    public class PlayerInfo
    {
        public ReactiveProperty<int> exp = new ReactiveProperty<int>(0);
        public ReactiveProperty<int> level= new ReactiveProperty<int>(1);
        public ReactiveProperty<int> gold= new ReactiveProperty<int>(0);
        
        // 다음 레벨이 되기위한 경험치
        public int nextExp => level.Value * 2;

        public PlayerInfo()
        {
            
        }
        
        public void GainMonsterReward(int gainExp, int gainGold)
        {
            GainExp(gainExp);
            GainGold(gainGold);
        }
        // exp 획득
        private void GainExp(int gainExp)
        {
            exp.Value += gainExp;
            if (exp.Value >= nextExp)
            {
                exp.Value -= nextExp;
                ++level.Value;
            }
        }
        private void GainGold(int gainGold)
        {
            gold.Value += gainGold;
        }

    }
}