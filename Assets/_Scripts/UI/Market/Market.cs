using System.Collections.Generic;
using BreadFlip.Entities.Skins;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BreadFlip.UI
{
    public class Market : MonoBehaviour
    {
        [Header("Button Buy/Equip")]
        [SerializeField] private TMP_Text _coinsText;
        [SerializeField] private GameObject _buyButtonObj;
        [SerializeField] private GameObject _notEquipedButtonObj;
        [SerializeField] private GameObject _equipedButtonObj;
        [SerializeField] private Button _buyButton;
        
        [Header("Skin selector")]
        [SerializeField] private Image _bigImage;
        [SerializeField] private GameObject _buyingCells;
        [SerializeField] private GameObject _categories;
        [SerializeField] private TMP_Text _skinPrice;
        [SerializeField] private TMP_Text _skinName;

        [Header("Skin Changing")]
        [SerializeField] private ToastSkinChanger _skinChanger_bread;
        [SerializeField] private ChangeWallAndFloorMaterial _skinChanger_kitchen;
        private Skins _selectedSkin;
        private CategoryType _selectedCategory;

        private Dictionary<Skins, int> _skinsPricing = new Dictionary<Skins, int>
        {
            {Skins.Bread_DefaultSkin, 0},
            {Skins.Bread_NotDefaultSkin, 500},
            {Skins.Bread_CatSkin, 1000},
            {Skins.Bread_CorgiAssSkin, 5000}

        };

        private Dictionary<Skins, string> _skinsNaming = new Dictionary<Skins, string>
        {
            {Skins.Bread_DefaultSkin, "Хлеб"},
            {Skins.Bread_NotDefaultSkin, "Надкусанный хлеб"},
            {Skins.Bread_CatSkin, "Котохлеб"},
            {Skins.Bread_CorgiAssSkin, "Булочки корги"}
        };

        private List<Sprite> SkinsImages = new List<Sprite>();

        public static int EquippedSkin = -1;

        private void OnEnable() {
                       
            // отображаем имеющиеся монеты
            if (PlayerPrefs.HasKey("all_coins"))
            {
                _coinsText.text = PlayerPrefs.GetInt("all_coins").ToString();
            }

            
            _buyButton.onClick.AddListener(DoActionsOnBreadSkins); // в будущем перенести эту команду в метод переключения категории (переприсваиваем листенер при переключении категории)

            FillSkinsList();

            // скин DefaultSkin по умолчанию есть у игрока
            if (!PlayerPrefs.HasKey("SKIN_" + Skins.Bread_DefaultSkin.ToString()))
                PlayerPrefs.SetInt("SKIN_" + Skins.Bread_DefaultSkin.ToString(), 1);

            // подписываемся на события
            MarketCell.SkinSelected += ChangeSkinCell;
            CategoryCell.CategorySelected += ChangeShownCategory;
        }

        private void OnDisable() {
            MarketCell.SkinSelected -= ChangeSkinCell;
            CategoryCell.CategorySelected -= ChangeShownCategory;
        }

        /// <summary>
        /// Меняем лист префабов ячеек для оборажения в соответствии с категорией
        /// </summary>
        /// <param name="type"> Тип категории, передаваемый через событие </param>
        private void ChangeShownCategory(CategoryType type)
        {
            _selectedCategory = type;
            Debug.Log($"Category: {_selectedCategory}");
            if (_selectedCategory == CategoryType.Bread)
            {
                _buyingCells.GetComponent<BuyingCells>()._cellsPrefabs = _categories.GetComponent<Categories>()._breadCellsPrefabs;
                FillSkinsList();
                _buyingCells.GetComponent<BuyingCells>().ShowSkins();
                _buyingCells.GetComponent<BuyingCells>().SpawnedItems[0].GetComponent<MarketCell>().buttonToggle.isOn = true;
            }
            // else if (_selectedCategory == CategoryType.Toast)
            // {
            //     _buyingCells.GetComponent<BuyingCells>()._cellsPrefabs = _categories.GetComponent<Categories>()._toastsCellsPrefabs;
            //     _buyingCells.GetComponent<BuyingCells>().ShowSkins();
            // }
            else if (_selectedCategory == CategoryType.Kitchen)
            {
                _buyingCells.GetComponent<BuyingCells>()._cellsPrefabs = _categories.GetComponent<Categories>()._kitchenCellsPrefabs;
                FillSkinsList();
                _buyingCells.GetComponent<BuyingCells>().ShowSkins();
                _buyingCells.GetComponent<BuyingCells>().SpawnedItems[0].GetComponent<MarketCell>().buttonToggle.isOn = true;
            }
        }

        // заполняем список скинов для отображения
        private void FillSkinsList()
        {
            if (SkinsImages.Count > 0) SkinsImages.Clear();
            var list = _buyingCells.GetComponent<BuyingCells>()._cellsPrefabs;
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    SkinsImages.Add(list[i].GetComponentsInChildren<Image>()[1].sprite);
                }
            }
        }

#region Methods for selected cell in Market
        private void ChangeBigImage(Skins skin)
        {
            _bigImage.sprite = SkinsImages[(int)skin];
        }

        private void ChangeSkinName(Skins skin)
        {
            _skinName.text = _skinsNaming[skin];
        }

        private void ChangeSkinCell(Skins skin)
        {
            _selectedSkin = skin;
            ChangeBigImage(skin);
            ChangeSkinName(skin);

            // Debug.Log(_selectedSkin.ToString() + "|||" + skin.ToString());
            // покупал ли игрок этот скин
            if (PlayerPrefs.HasKey("SKIN_" + skin.ToString()))
            {
                if (PlayerPrefs.GetInt("SKIN_" + skin.ToString()) == 1)
                {
                    SwitchFromBuyToEquip();
                }
            }

            else
            {
                SwitchToBuyButton();
                _skinPrice.text = _skinsPricing[skin].ToString();
                
                if (PlayerPrefs.GetInt("all_coins") < _skinsPricing[skin])
                {
                    _buyButton.interactable = false;
                    foreach (var comp in _buyButtonObj.GetComponents<ChangeTextColor>())
                    {
                        comp.ChangeColorOnPressed();
                    }
                    _buyButtonObj.transform.parent.gameObject.GetComponent<EventTrigger>().enabled = false;
                }
                else
                {
                    _buyButton.interactable = true;
                    foreach (var comp in _buyButtonObj.GetComponents<ChangeTextColor>())
                    {
                        comp.ChangeColorToDefault();
                    }
                    _buyButtonObj.transform.parent.gameObject.GetComponent<EventTrigger>().enabled = true;
                }
            }

            // надет ли выбранный скин
            if (PlayerPrefs.HasKey("SKIN_EQUPPIED"))
            {
                if (PlayerPrefs.GetInt("SKIN_EQUPPIED") == (int)skin)
                {
                    SwitchToEquipped();
                }
            }
            else
            {
                PlayerPrefs.SetInt("SKIN_EQUPPIED", (int)skin);
                SwitchToEquipped();
            }
        }
#endregion

#region Market main button logic
        private void DoActionsOnBreadSkins()
        {
            // покупка скина
            if (_buyButtonObj.activeSelf)
            {
                // 1. вычитаем деньги 
                var moneyToRemove = _skinsPricing[_selectedSkin];
                PlayerPrefs.SetInt("all_coins", PlayerPrefs.GetInt("all_coins") - moneyToRemove);

                // 2. сохраняем покупку скина
                PlayerPrefs.SetInt("SKIN_" + _selectedSkin.ToString(), 1);

                // 3. меняем кнопку
                SwitchFromBuyToEquip();

                // 4. меняем показания монет
                _coinsText.text = (int.Parse(_coinsText.text) - moneyToRemove).ToString();
            }

            // надевание скина
            else if (_notEquipedButtonObj.activeSelf)
            {
                // меняем скин
                _skinChanger_bread.ChangeSkin((int)_selectedSkin);
                
                // надеваем выбранный
                PlayerPrefs.SetInt("SKIN_EQUPPIED", (int)_selectedSkin);
                SwitchToEquipped();
            }
        }
#endregion

#region Methods to switch sign on main market button
        private void SwitchFromBuyToEquip()
        {
            _buyButtonObj.SetActive(false);
            _equipedButtonObj.SetActive(false);
            _notEquipedButtonObj.SetActive(true);

            _buyButton.interactable = true;
        }

        private void SwitchToEquipped()
        {
            _buyButtonObj.SetActive(false);
            _equipedButtonObj.SetActive(true);
            _notEquipedButtonObj.SetActive(false);

            _buyButton.interactable = false;
            _equipedButtonObj.GetComponent<ChangeTextColor>().ChangeColorOnPressed();
        }

        private void SwitchToBuyButton()
        {
            _buyButtonObj.SetActive(true);
            _equipedButtonObj.SetActive(false);
            _notEquipedButtonObj.SetActive(false);

            _buyButton.interactable = true;
        }
#endregion

        // срабатывает при нажатии на кнопку магазина в Главном Меню, задаёт начальные выбранные категорию и скин
        public void SetDefaultView()
        {
            _categories.GetComponent<Categories>().breadCategoryPrefab.GetComponent<CategoryCell>().buttonToggle.isOn = true;
            _categories.GetComponent<Categories>().breadCategoryPrefab.GetComponent<Image>().sprite = 
                _categories.GetComponent<Categories>().breadCategoryPrefab.GetComponent<CategoryCell>().buttonToggle.spriteState.selectedSprite;
            ChangeShownCategory(CategoryType.Bread);
            _buyingCells.GetComponent<BuyingCells>().SpawnedItems[0].GetComponent<MarketCell>().buttonToggle.isOn = true;
        }
    }
}
