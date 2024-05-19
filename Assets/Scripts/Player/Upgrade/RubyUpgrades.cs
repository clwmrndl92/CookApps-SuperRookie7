using UniRx;
using Zenject;

namespace LineUpHeros
{
    // ruby 예시
    public class RubyHpUpgrade : UpgradeInfo
    {
        private int _effect => 10 + upgradeNum.Value;
        
        private int _cost => 10 + upgradeNum.Value * 5;
        public RubyHpUpgrade(PlayerInfoController playerInfoController) : base(playerInfoController)
        {
            title = "HP";
            info.Value = $"Max HP\n+{_effect}";
            cost.Value = _cost;
            costType = UpgradeCostType.Ruby;
        }

        public override void TryUpgrade()
        {
            if (playerInfoController.UseRuby(_cost))
            {
                // todo : 루비강화는 캐릭터별 스킬 강화?
                playerInfoController.ApplyStatusUpgradeToAll((character) =>
                {
                    character.status.AddStat((int)Status.EnumStatus.Hp, _effect, false);
                    character.status.tmpHp.Value += _effect;
                });
                ++upgradeNum.Value;
                info.Value = $"Max HP\n+{_effect}";
                cost.Value = _cost;
            }
        }
    }
}