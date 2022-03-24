using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RogueTowerResearch
{
    public class ResearchManager : MonoBehaviour
    {
        private readonly Dictionary<TowerType, TowerFlyweight> TowerFlyweights = new Dictionary<TowerType, TowerFlyweight>();
        private readonly Dictionary<TowerType, Tower> TowerPrefabs = new Dictionary<TowerType, Tower>();
        private readonly Dictionary<int, ResearchGenerator> _researchGenerators = new Dictionary<int, ResearchGenerator>();
        private SpawnManager _spawnManager;
        private CardManager _cardManager;
        private PauseMenu _pauseMenu;

        private void Awake()
        {
            _spawnManager = FindObjectOfType<SpawnManager>();
            _cardManager = FindObjectOfType<CardManager>();
            _pauseMenu = FindObjectOfType<PauseMenu>();
            foreach (var flyweight in FindObjectsOfType<TowerFlyweight>())
            {
                TowerFlyweights[flyweight.GetTowerType()] = flyweight;
            }

            foreach (var prefab in Resources.FindObjectsOfTypeAll<Tower>())
            {
                if (!prefab.name.Contains("(Clone)"))
                {
                    TowerPrefabs[prefab.towerType] = prefab;
                }
            }
        }

        public void AddResearchGenerator(int houseInstanceId, ResearchGenerator researchGenerator)
        {
            _researchGenerators.Add(houseInstanceId, researchGenerator);
        }

        public void SelectResearch(ResearchGenerator researchGenerator)
        {
            if (_cardManager.drawingCards || researchGenerator is null) return;

            var availableCards = _cardManager.GetAvailableCards().ToList();
            var defenderTowerTypes = researchGenerator.GetDefenderTowerTypes();
            var researchableCards = availableCards.FindAll(x => x is TowerUpgradeCard upgradeCard && defenderTowerTypes.Contains(upgradeCard.GetTowerType()));

            if (researchableCards.Count == 0) return;

            var expandsWereShowing = FindObjectOfType<TileSpawnLocation>() != null;

            // Pause just to avoid the weirdness that happens if a wave finishes while we're still selecting research
            _pauseMenu.Pause();
            _pauseMenu.GetPauseMenu().SetActive(false);

            _cardManager.availableCards = researchableCards;
            _cardManager.DrawCards();

            var cards = _cardManager.GetCards();
            var cardHolders = _cardManager.GetCardHolders();

            var restoreOnClicks = new List<Action>();

            for (var i = 0; i < cards.Length; i++)
            {
                if (cards[i] is TowerUpgradeCard card)
                {
                    var cardHolder = cardHolders[i];
                    var button = cardHolder.transform.GetChild(0).GetComponentInChildren<Button>();
                    var originalOnClick = button.onClick;
                    restoreOnClicks.Add(() => button.onClick = originalOnClick);
                    button.onClick = new Button.ButtonClickedEvent();
                    button.onClick.AddListener(() =>
                    {
                        restoreOnClicks.ForEach(restoreOnClick => restoreOnClick());
                        restoreOnClicks.Clear();
                        // Set the research and trigger the house UI to update
                        researchGenerator.SetResearch(card);
                        researchGenerator.House.SpawnUI();
                        // Remove card being researched from the normal pool
                        availableCards.Remove(card);
                        _cardManager.availableCards = availableCards;
                        // Kinda hacky way to hide card selection without actually picking a card
                        _cardManager.drawingCards = false;
                        _cardManager.DrawCards(0);
                        _cardManager.drawingCards = false;
                        if (expandsWereShowing)
                        {
                            _spawnManager.ShowSpawnUIs(true);
                        }
                        _pauseMenu.UnPause();
                    });
                }
            }
        }

        public void CheckTowers(int houseInstanceId)
        {
            _researchGenerators[houseInstanceId].CheckTowers();
        }

        public void SpawnUI(int houseInstanceId)
        {
            _researchGenerators[houseInstanceId].SpawnUI();
        }

        public void UpdateResearch()
        {
            foreach (var researchGenerator in _researchGenerators.Values)
            {
                researchGenerator.UpdateResearch();
            }
        }

        public (float BaseRange, float BonusRange) GetTowerRange(TowerType towerType)
        {
            return (TowerPrefabs[towerType].GetBaseRange(), TowerFlyweights[towerType].bonusRange);
        }
    }
}
