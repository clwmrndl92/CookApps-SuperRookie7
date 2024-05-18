using System;
using UniRx;
using UnityEditor.UI;
using UnityEngine;

namespace LineUpHeros
{
    public interface IDamagable
    {
        // 프로퍼티
        GameObject gameObjectIDamagable { get; }
        public ReactiveProperty<bool> isDead { get; set; }
        Status status { get; set; }
        // 메소드
        void TakeHeal(int healAmount);
        void TakeDamage(Unit from, int damage);
        void TakeStun(float stunTime);
    }

    public class Status
    {
        public ReactiveProperty<int> tmpHp;
        public ReactiveProperty<int> exp;
        public ReactiveProperty<int> level;

        // Final Stat Property
        public int maxHp => (int)GetFinalStat(_baseHp, _addHp, _addPerHp, _addLevelHp);
        public int atk => (int)GetFinalStat(_baseAtk, _addAtk, _addPerAtk, _addLevelAtk);
        public int atkRange => (int)GetFinalStat(_baseAtkRange, _addAtkRange, _addPerAtkRange, _addLevelAtkRange);
        public float atkCool => (int)GetFinalStat(_baseAtkCool, _addAtkCool, _addPerAtkCool, _addLevelAtkCool);

        // Base Stat, 영구 성장치 적용
        private int _baseHp;
        private int _baseAtk;
        private int _baseAtkRange;
        private float _baseAtkCool;

        // Additional Stat, 아이템, 버프 등 일시적 성장치
        private int _addHp;
        private int _addAtk;
        private int _addAtkRange;
        private float _addAtkCool;

        // Additional Percent Stat, 아이템, 버프 등 일시적 성장치 (퍼센트)
        private int _addPerHp;
        private int _addPerAtk;
        private int _addPerAtkRange;
        private float _addPerAtkCool;

        // 레벨 관련 성장치


        // 다음 레벨이 되기위한 경험치
        public int nextExp
        {
            get
            {
                return level.Value * 2;
            }
        }

        private int _addLevelHp;
        private int _addLevelAtk;
        private int _addLevelAtkRange;
        private float _addLevelAtkCool;

        public Status(StatSettings settings)
        {
            level = new ReactiveProperty<int>(1);
            exp = new ReactiveProperty<int>(0);

            _baseHp = settings.baseHp;
            _baseAtk = settings.baseAtk;
            _baseAtkRange = settings.baseAtkRange;
            _baseAtkCool = settings.baseAtkCool;

            /* TODO: installer에서 캐릭터별 성장치 설정 */
            _addLevelHp = _baseHp / 5;
            _addLevelAtk = _baseAtk / 5;
            _addLevelAtkRange = _baseAtkRange / 5;
            _addLevelAtkCool = _baseAtkCool / 5;

            // UI에 업데이트 될 값
            tmpHp = new ReactiveProperty<int>(maxHp);
            tmpHp.Where(value => value < 0 || value > maxHp)
                .Subscribe(_ =>
                {
                    tmpHp.Value = Mathf.Clamp(tmpHp.Value, 0, maxHp);
                });
        }
        protected float GetFinalStat(float baseStat, float addStat, float addPerStat, float addLevelStat)
        {
            // Final Stat 계산식
            return baseStat + addStat + ((level.Value - 1) * addLevelStat) + ((baseStat + addStat) * (addPerStat / 100));
        }

        // exp 획득
        public void GainExp(int gainExp)
        {
            exp.Value += gainExp;
            if (exp.Value >= nextExp)
            {
                exp.Value -= nextExp;
                ++level.Value;
            }
        }
    }

    // Scriptable Object Installer 세팅 값
    [Serializable]
    public class StatSettings
    {
        public int baseHp;
        public int baseAtk;
        public int baseAtkRange;
        public float baseAtkCool;
    }

}