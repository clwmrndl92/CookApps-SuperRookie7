using UniRx;
using Zenject;

namespace LineUpHeros
{
    public class PlayerInfo
    {
        public ReactiveProperty<int> exp = new ReactiveProperty<int>(0);
        public ReactiveProperty<int> level= new ReactiveProperty<int>(1);
        public ReactiveProperty<int> gold= new ReactiveProperty<int>(0);
        
        private TankerCharacter _tanker;
        private ShortRangeDealerCharacter _shortRangeDealer;
        private LongRangeDealerCharacter _longRangeDealer;
        private HealerCharacter _healer;
        
        [Inject]
        public void Construct(TankerCharacter tanker, ShortRangeDealerCharacter shortRangeDealer,
            LongRangeDealerCharacter longRangeDealer, HealerCharacter healer)
        {
            _tanker = tanker;
            _shortRangeDealer = shortRangeDealer;
            _longRangeDealer = longRangeDealer;
            _healer = healer;
        }
        
        // 다음 레벨이 되기위한 경험치
        public int nextExp => level.Value * 2;
        
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
                LevelUp();
            }
        }
        private void GainGold(int gainGold)
        {
            gold.Value += gainGold;
        }

        private void LevelUp()
        {
            ++level.Value;

            CalLevelUpStat(_tanker);
            CalLevelUpStat(_shortRangeDealer);
            CalLevelUpStat(_longRangeDealer);
            CalLevelUpStat(_healer);
        }

        private void CalLevelUpStat(Character character)
        {
            // 레벨업당 기본스탯 10퍼씩 상승
            int indexHp = (int)Status.EnumStatus.Hp;
            int indexAtk = (int)Status.EnumStatus.Atk;
            
            float upHP = character.status.GetBaseStat(indexHp) / 10;
            upHP -= upHP % 10;
            character.status.AddStat(indexHp, upHP, true);
            character.status.tmpHp.Value += (int)upHP;
            character.status.AddStat(indexHp, character.status.GetBaseStat(indexHp) / 10, true);
            character.status.AddStat(indexAtk, character.status.GetBaseStat(indexAtk) / 10, true);
        }

    }
}