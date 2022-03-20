using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace RogueTowerResearch
{
    public static class FieldExtensions
    {
        #pragma warning disable Publicizer001
        private static readonly AccessTools.FieldRef<House, List<Tower>> _defendersRef = AccessTools.FieldRefAccess<House, List<Tower>>(nameof(House.defenders));
        private static readonly AccessTools.FieldRef<House, GameObject> _uiObjectRef = AccessTools.FieldRefAccess<House, GameObject>(nameof(House.UIObject));
        private static readonly AccessTools.FieldRef<Tower, float> _baseRangeRef = AccessTools.FieldRefAccess<Tower, float>(nameof(Tower.baseRange));
        private static readonly AccessTools.FieldRef<TowerFlyweight, TowerType> _towerTypeForDamageTrackerRef = AccessTools.FieldRefAccess<TowerFlyweight, TowerType>(nameof(TowerFlyweight.towerTypeForDamageTracker));
        private static readonly AccessTools.FieldRef<TowerUpgradeCard, TowerType> _towerTypeRef = AccessTools.FieldRefAccess<TowerUpgradeCard, TowerType>(nameof(TowerUpgradeCard.towerType));
        private static readonly AccessTools.FieldRef<TowerUpgradeCard, string> _upgradeCardTitleRef = AccessTools.FieldRefAccess<TowerUpgradeCard, string>(nameof(TowerUpgradeCard.title));
        private static readonly AccessTools.FieldRef<SimpleUI, Text> _discriptionTextRef = AccessTools.FieldRefAccess<SimpleUI, Text>(nameof(SimpleUI.discriptionText));
        private static readonly AccessTools.FieldRef<SimpleUI, GameObject> _demolishButtonRef = AccessTools.FieldRefAccess<SimpleUI, GameObject>(nameof(SimpleUI.demolishButton));
        private static readonly AccessTools.FieldRef<PauseMenu, GameObject> _pauseMenuRef = AccessTools.FieldRefAccess<PauseMenu, GameObject>(nameof(PauseMenu.pauseMenu));
        private static readonly AccessTools.FieldRef<CardManager, UpgradeCard[]> _cardsRef = AccessTools.FieldRefAccess<CardManager, UpgradeCard[]>(nameof(CardManager.cards));
        private static readonly AccessTools.FieldRef<CardManager, GameObject[]> _cardHoldersRef = AccessTools.FieldRefAccess<CardManager, GameObject[]>(nameof(CardManager.cardHolders));
        private static readonly AccessTools.FieldRef<CardManager, List<int>> _queuedDrawsRef = AccessTools.FieldRefAccess<CardManager, List<int>>(nameof(CardManager.queuedDraws));
        private static readonly AccessTools.FieldRef<CardManager, List<UpgradeCard>> _availableCardsRef = AccessTools.FieldRefAccess<CardManager, List<UpgradeCard>>(nameof(CardManager.availableCards));
        #pragma warning restore Publicizer001

        public static List<Tower> GetDefenders(this House source) => _defendersRef(source);
        public static GameObject GetUIObject(this House source) => _uiObjectRef(source);
        public static float GetBaseRange(this Tower source) => _baseRangeRef(source);
        public static TowerType GetTowerType(this TowerFlyweight source) => _towerTypeForDamageTrackerRef(source);
        public static TowerType GetTowerType(this TowerUpgradeCard source) => _towerTypeRef(source);
        public static string GetTitle(this TowerUpgradeCard source) => _upgradeCardTitleRef(source);
        public static Text GetDescriptionTextComponent(this SimpleUI source) => _discriptionTextRef(source);
        public static GameObject GetDemolishButton(this SimpleUI source) => _demolishButtonRef(source);
        public static GameObject GetPauseMenu(this PauseMenu source) => _pauseMenuRef(source);
        public static UpgradeCard[] GetCards(this CardManager source) => _cardsRef(source);
        public static GameObject[] GetCardHolders(this CardManager source) => _cardHoldersRef(source);
        public static List<int> GetQueuedDraws(this CardManager source) => _queuedDrawsRef(source);
        public static List<UpgradeCard> GetAvailableCards(this CardManager source) => _availableCardsRef(source);
    }
}
