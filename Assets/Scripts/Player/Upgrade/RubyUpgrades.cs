using UniRx;
using Zenject;

namespace LineUpHeros
{
    public class RubySkillUpgrade : UpgradeInfo
    {
        private float _upgradeValue = 0.5f;
        private float _effect => upgradeNum.Value*_upgradeValue;
        private int _cost => 1 + upgradeNum.Value * 1;

        private EnumCharacter _characterType;
        public RubySkillUpgrade(PlayerInfoController playerInfoController, string charName, EnumCharacter characterType) : base(playerInfoController)
        {
            title = charName;
            info.Value = $"Skill Up!\n(ATK Multiplier)\n+{_effect}";
            cost.Value = _cost;
            costType = UpgradeCostType.Ruby;
            _characterType = characterType;
        }

        public override void TryUpgrade()
        {
            if (playerInfoController.UseGold(_cost))
            {
                // 스킬 데미지 업그레이드
                playerInfoController.ApplyStatusUpgrade((character) =>
                {
                    character.status.AddStat((int)CharacterStatus.EnumCharacterStatus.SkillDamageMul, _upgradeValue, false);
                }, _characterType);
                ++upgradeNum.Value;
                info.Value = $"Skill Up!\n(ATK Multiplier)\n+{_effect}";
                cost.Value = _cost;
            }
        }
    }
    
}