using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RogueTowerResearch
{
    public class ResearchGenerator : MonoBehaviour
    {
        public const int COST_OF_RESEARCH = 45;

        public House House;
        private IncomeGenerator _incomeGenerator;
        private CardManager _cardManager;
        private ResearchManager _researchManager;
        private TowerUpgradeCard _upgradeCard;
        private TowerType? _towerBeingResearched;
        private int _researchPoints;

        private void Awake()
        {
            House = GetComponentInParent<House>();
            _incomeGenerator = GetComponentInParent<IncomeGenerator>();
            _cardManager = FindObjectOfType<CardManager>();
            _researchManager = FindObjectOfType<ResearchManager>();
        }

        public HashSet<TowerType> GetDefenderTowerTypes()
        {
            return new HashSet<TowerType>(House.GetDefenders().ConvertAll(x => x.towerType));
        }

        public IEnumerable<Tower> GetDefendersBeingResearched()
        {
            return House.GetDefenders().Where(x => x.towerType == _towerBeingResearched);
        }

        public void SetResearch(TowerUpgradeCard upgradeCard)
        {
            if (_towerBeingResearched.HasValue)
            {
                var bonusRange = _researchManager.TowerFlyweights[_towerBeingResearched.Value].bonusRange;
                foreach (var defender in GetDefendersBeingResearched())
                {
                    defender.baseRange *= 2;
                    defender.range = defender.GetBaseRange() + bonusRange;
                }
            }
            _researchPoints = 0;
            _upgradeCard = upgradeCard;
            _towerBeingResearched = _upgradeCard?.GetTowerType();
            _incomeGenerator.incomeTimesLevel = _upgradeCard is null ? House.GetDefenders().Count : 0;
            if (_towerBeingResearched.HasValue)
            {
                foreach (var defender in GetDefendersBeingResearched())
                {
                    defender.baseRange /= 2;
                    defender.range /= 2;
                }
            }
        }

        public void UpdateResearch()
        {
            _researchPoints += GetResearchPointsThisTurn();

            if (_researchPoints >= COST_OF_RESEARCH)
            {
                _upgradeCard.Upgrade();
                _cardManager.GetAvailableCards().AddRange(_upgradeCard.unlocks);
                SetResearch(null);
            }
        }

        public void CheckTowers()
        {
            if (_upgradeCard is null) return;
            _incomeGenerator.incomeTimesLevel = 0;
        }

        private int GetResearchPointsThisTurn()
        {
            if (_upgradeCard is null) return 0;
            return GetDefendersBeingResearched().Sum(x => x.level);
        }

        private void SelectResearch()
        {
            _researchManager.SelectResearch(this);
        }

        private void CancelResearch(SimpleUI houseUI)
        {
            _cardManager.GetAvailableCards().Add(_upgradeCard);
            SetResearch(null);
            Destroy(houseUI);
            House.SpawnUI();
        }

        public void SpawnUI()
        {
            var houseUI = FindObjectsOfType<SimpleUI>().Where(x => x.name == $"{House.GetUIObject().name}(Clone)").FirstOrDefault();

            if (houseUI is null) return;

            if (_upgradeCard != null)
            {
                SetupButton(houseUI, "Cancel", () => CancelResearch(houseUI));
                UpdatePanelSize(houseUI);
                houseUI.SetDiscriptionText($"This house is researching {_upgradeCard.GetTitle()}.\nIt has researched {_researchPoints} out of {COST_OF_RESEARCH} points.\nIt will research {GetResearchPointsThisTurn()} points this turn.");
            }
            else if (House.GetDefenders().Count > 0)
            {
                SetupButton(houseUI, "Research", SelectResearch);
            }

            static void SetupButton(SimpleUI houseUI, string text, UnityAction onClick)
            {
                var buttonGameObject = houseUI.GetDemolishButton();
                buttonGameObject.SetActive(true);
                var buttonText = buttonGameObject.GetComponentInChildren<Text>();
                buttonText.text = text;
                var button = buttonGameObject.GetComponent<Button>();
                button.onClick = new Button.ButtonClickedEvent();
                button.onClick.AddListener(onClick);
            }

            static void UpdatePanelSize(SimpleUI houseUI)
            {
                var text = houseUI.GetDescriptionTextComponent();
                var textRectTransform = text.GetComponent<RectTransform>();
                var panelRectTransform = text.transform.parent.GetComponent<RectTransform>();
                textRectTransform.anchoredPosition = new Vector2(textRectTransform.anchoredPosition.x, .55f);
                panelRectTransform.anchorMax = new Vector2(panelRectTransform.anchorMax.x, 1.25f);
            }
        }
    }
}
