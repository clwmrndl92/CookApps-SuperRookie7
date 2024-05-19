using UniRx;
using Zenject;

namespace LineUpHeros
{
    // 재화 종류
    public enum UpgradeCostType
    {
        Gold, Ruby
    }
    // 강화 항목 클래스
    public abstract class UpgradeInfo
    {
        protected PlayerInfoController playerInfoController;
        public string title = "";
        public ReactiveProperty<string> info = new ReactiveProperty<string>("");
        public ReactiveProperty<int> upgradeNum = new ReactiveProperty<int>(0);
        public ReactiveProperty<int> cost = new ReactiveProperty<int>(0);
        public UpgradeCostType costType;

        public abstract void TryUpgrade();
        protected UpgradeInfo(PlayerInfoController infoController)
        {
            playerInfoController = infoController;
        }
    }
}