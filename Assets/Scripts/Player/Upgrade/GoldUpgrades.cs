using UniRx;
using Zenject;

namespace LineUpHeros
{
    public class GoldHPUpgrade : UpgradeInfo
    {

        private int _upgradeValue = 10;
        private int _effect => upgradeNum.Value*_upgradeValue;
        private int _cost => 10 + upgradeNum.Value * 5;

        public GoldHPUpgrade(PlayerInfoController playerInfoController) : base(playerInfoController)
        {
            title = "HP";
            info.Value = $"Max HP\n+{_effect}";
            cost.Value = _cost;
            costType = UpgradeCostType.Gold;
        }

        public override void TryUpgrade()
        {
            if (playerInfoController.UseGold(_cost))
            {
                // 모든 캐릭터 스탯 일괄 상승
                playerInfoController.ApplyStatusUpgradeToAll((character) =>
                {
                    character.status.AddStat((int)Status.EnumStatus.Hp, _upgradeValue, false);
                    character.status.tmpHp.Value += _effect;
                });
                ++upgradeNum.Value;
                info.Value = $"Max HP\n+{_effect}";
                cost.Value = _cost;
            }
        }
    }

    public class GoldATKUpgrade : UpgradeInfo
    {
        
        private int _upgradeValue = 1;
        private int _effect => upgradeNum.Value * _upgradeValue;
        private int _cost => 10 + upgradeNum.Value * 5;

        public GoldATKUpgrade(PlayerInfoController playerInfoController) : base(playerInfoController)
        {
            title = "ATK";
            info.Value = $"ATK\n+{_effect}";
            cost.Value = _cost;
            costType = UpgradeCostType.Gold;
        }

        public override void TryUpgrade()
        {
            if (playerInfoController.UseGold(_cost))
            {
                // 모든 캐릭터 스탯 일괄 상승
                playerInfoController.ApplyStatusUpgradeToAll((character) =>
                {
                    character.status.AddStat((int)Status.EnumStatus.Atk, _upgradeValue, false);
                });
                ++upgradeNum.Value;
                info.Value = $"ATK\n+{_effect}";
                cost.Value = _cost;
            }
        }
    }
}