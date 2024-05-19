using System;
using System.Collections.Generic;
using UniRx;
using Zenject;

namespace LineUpHeros
{
    public class PlayerInfoController
    {
        public ReactiveProperty<int> exp = new ReactiveProperty<int>(0);
        public ReactiveProperty<int> level = new ReactiveProperty<int>(1);
        public ReactiveProperty<int> gold = new ReactiveProperty<int>(0);
        public ReactiveProperty<int> ruby = new ReactiveProperty<int>(9999);

        private TankerCharacter _tanker;
        private ShortRangeDealerCharacter _shortRangeDealer;
        private LongRangeDealerCharacter _longRangeDealer;
        private HealerCharacter _healer;

        public List<UpgradeInfo> goldUpgradeList = new List<UpgradeInfo>();
        public List<UpgradeInfo> rubyUpgradeList = new List<UpgradeInfo>();

        private SignalBus _signalBus;

        [Inject]
        public void Construct(TankerCharacter tanker, ShortRangeDealerCharacter shortRangeDealer,
            LongRangeDealerCharacter longRangeDealer, HealerCharacter healer, SignalBus signalBus)
        {
            _tanker = tanker;
            _shortRangeDealer = shortRangeDealer;
            _longRangeDealer = longRangeDealer;
            _healer = healer;
            
            // 골드 업그레이드 항목 추가
            goldUpgradeList.Add(new GoldHPUpgrade(this));
            goldUpgradeList.Add(new GoldATKUpgrade(this));

            // 루비 업그레이드 항목 추가
            rubyUpgradeList.Add(new RubyHpUpgrade(this));
            
            // 몬스터 사망 시그널
            _signalBus = signalBus;
            _signalBus.Subscribe<GameEvent.MonsterDieSignal>(x => OnDieMonster(x.monsterInfo));
        }
        
        // 다음 레벨이 되기위한 경험치
        public int nextExp => level.Value * 2;

        // todo : 몬스터 별로 얻을 경험치, 골드, 스크립터블 오브젝트로 설정 가능하게
        public void OnDieMonster(MonsterInfo info)
        {
            GainExp(info.rewardExp);
            GainGold(info.rewardGold);
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

            // 레벨업당 스탯 상승
            // todo : 캐릭터 별로 따로 상승치 설정?
            ApplyStatusUpgradeToAll(CalLevelUpStat);
        }

        public void ApplyStatusUpgradeToAll(Action<Character> upgradeAction)
        {
            upgradeAction(_tanker);
            upgradeAction(_shortRangeDealer);
            upgradeAction(_longRangeDealer);
            upgradeAction(_healer);
        }

        private void CalLevelUpStat(Character character)
        {
            // 레벨업당 체력, 공격력 base스탯 10퍼씩 상승
            int indexHp = (int)Status.EnumStatus.Hp;
            int indexAtk = (int)Status.EnumStatus.Atk;

            float upHP = character.status.GetBaseStat(indexHp) / 10;
            upHP -= upHP % 10; // 1의자리수 자름
            character.status.AddStat(indexHp, upHP, true);
            character.status.tmpHp.Value += (int)upHP;
            character.status.AddStat(indexHp, character.status.GetBaseStat(indexHp) / 10, true);
            character.status.AddStat(indexAtk, character.status.GetBaseStat(indexAtk) / 10, true);
        }


        public bool UseGold(int useGold)
        {
            if (gold.Value >= useGold)
            {
                gold.Value -= useGold;
                return true;
            }
            return false;
        }

        public bool UseRuby(int useRuby)
        {
            if (ruby.Value >= useRuby)
            {
                ruby.Value -= useRuby;
                return true;
            }
            return false;
        }
    }
}