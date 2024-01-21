using System;
using System.Collections.Generic;
using BreadFlip.Entities.Skins;
using BreadFlip.Generation;
using TMPro;
using Unity.VisualScripting;
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
        [SerializeField] private SwitchToasterSkin _skinChanger_toaster;
        private Skins _selectedSkin;
        private int _selectedSkinIndex;
        private CategoryType _selectedCategory;

        [Header("SkinsCount")]
        public int BreadSkinsCount;
        public int TostersSkinsCount;
        public int KitchenSkinsCount;

        private Dictionary<Skins, int> _skinsPricing = new Dictionary<Skins, int>
        {
            {Skins.Bread_DefaultSkin, 0},
            {Skins.Bread_NotDefaultSkin, 500},
            {Skins.Bread_CatSkin, 1000},
            {Skins.Bread_CorgiAssSkin, 5000},
            {Skins.Kitchen_Default, 0},
            {Skins.Kitchen_Bricks, 500},
            {Skins.Kitchen_Cats, 1000},
            {Skins.Kitchen_Lightning, 5000},
            {Skins.Kitchen_Game, 10000},
            {Skins.Kitchen_Gold, 20000},
            {Skins.Toaster_default, 0},
            {Skins.Toaster_bricks, 500},
            {Skins.Toaster_cute, 1000},
            // {Skins.Toaster_undefined, -1},
            {Skins.Toaster_voxel, 10000},
            {Skins.Toaster_golden, 20000},
        };

        private Dictionary<Skins, string> _skinsNaming;

        private List<Sprite> SkinsImages = new List<Sprite>();

        public static int EquippedSkin = -1;

        private void OnEnable() 
        {

            _skinsNaming = new Dictionary<Skins, string>
            {
                {Skins.Bread_DefaultSkin, LocalizationManager.Localize("Skins.Bread")},
                {Skins.Bread_NotDefaultSkin, LocalizationManager.Localize("Skins.NotDefaultBread")},
                {Skins.Bread_CatSkin, LocalizationManager.Localize("Skins.CatBread")},
                {Skins.Bread_CorgiAssSkin, LocalizationManager.Localize("Skins.CorgiBread")},
                {Skins.Kitchen_Default, LocalizationManager.Localize("Skins.WhiteWall")},
                {Skins.Kitchen_Bricks, LocalizationManager.Localize("Skins.BricksWall")},
                {Skins.Kitchen_Cats, LocalizationManager.Localize("Skins.CatWall")},
                {Skins.Kitchen_Lightning, LocalizationManager.Localize("Skins.LightningWall")},
                {Skins.Kitchen_Game, LocalizationManager.Localize("Skins.GamerWall")},
                {Skins.Kitchen_Gold, LocalizationManager.Localize("Skins.GoldWall")},
                {Skins.Toaster_default,LocalizationManager.Localize("Skins.ToasterDefault")},
                {Skins.Toaster_bricks,LocalizationManager.Localize("Skins.BricksToaster")},
                {Skins.Toaster_cute,LocalizationManager.Localize("Skins.CuteToaster")},
                // {Skins.Toaster_undefined,"undefined"},
                {Skins.Toaster_voxel,LocalizationManager.Localize("Skins.VoxelToaster")},                
                {Skins.Toaster_golden,LocalizationManager.Localize("Skins.GoldenToaster")},
            };
                       
            // отображаем имеющиеся монеты
            if (PlayerPrefs.HasKey("all_coins"))
            {
                _coinsText.text = PlayerPrefs.GetInt("all_coins").ToString();
            }

            // скины по умолчанию есть у игрока
            if (!PlayerPrefs.HasKey("SKIN_" + Skins.Bread_DefaultSkin.ToString()))
                PlayerPrefs.SetInt("SKIN_" + Skins.Bread_DefaultSkin.ToString(), 1);
            if (!PlayerPrefs.HasKey("KITCHEN_" + Skins.Kitchen_Default.ToString()))
                PlayerPrefs.SetInt("KITCHEN_" + Skins.Kitchen_Default.ToString(), 1);
            if (!PlayerPrefs.HasKey("TOASTER_" + Skins.Toaster_default.ToString()))
                PlayerPrefs.SetInt("TOASTER_" + Skins.Toaster_default.ToString(), 1);

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
            // Debug.Log($"Category: {_selectedCategory}");
            if (_selectedCategory == CategoryType.Bread)
            {
                // переприсваиваем листенер на кнопку для нужной категории
                _buyButton.onClick.RemoveAllListeners();
                _buyButton.onClick.AddListener(DoActions_BreadSkins);

                // переключаем категорию
                _buyingCells.GetComponent<BuyingCells>()._cellsPrefabs = _categories.GetComponent<Categories>()._breadCellsPrefabs;
                FillSkinsList();
                _buyingCells.GetComponent<BuyingCells>().ShowSkins();
                _buyingCells.GetComponent<BuyingCells>().SpawnedItems[0].GetComponent<MarketCell>().buttonToggle.isOn = true;
            }
            else if (_selectedCategory == CategoryType.Toast)
            {
                // переприсваиваем листенер на кнопку для нужной категории
                _buyButton.onClick.RemoveAllListeners();
                _buyButton.onClick.AddListener(DoActions_ToasterSkins);

                // переключаем категорию
                _buyingCells.GetComponent<BuyingCells>()._cellsPrefabs = _categories.GetComponent<Categories>()._toastsCellsPrefabs;
                FillSkinsList();
                _buyingCells.GetComponent<BuyingCells>().ShowSkins();
                _buyingCells.GetComponent<BuyingCells>().SpawnedItems[0].GetComponent<MarketCell>().buttonToggle.isOn = true;
            }
            else if (_selectedCategory == CategoryType.Kitchen)
            {
                // переприсваиваем листенер на кнопку для нужной категории
                _buyButton.onClick.RemoveAllListeners();
                _buyButton.onClick.AddListener(DoActions_KitchenSkins);

                // переключаем категорию
                _buyingCells.GetComponent<BuyingCells>()._cellsPrefabs = _categories.GetComponent<Categories>()._kitchenCellsPrefabs;
                FillSkinsList();
                _buyingCells.GetComponent<BuyingCells>().ShowSkins();
                // Debug.Log($"SpawnedItems: {_buyingCells.GetComponent<BuyingCells>().SpawnedItems.Count}");
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
        private void ChangeSkinInfoPanel(Skins skin)
        {
            if (skin.ToString().StartsWith("Bread")) _selectedSkinIndex = (int)skin;
            else if (skin.ToString().StartsWith("Kitchen")) _selectedSkinIndex = (int)skin - BreadSkinsCount;
            else _selectedSkinIndex = (int)skin - BreadSkinsCount - KitchenSkinsCount;

            _bigImage.sprite = SkinsImages[_selectedSkinIndex];
            _skinName.text = _skinsNaming[skin];
        }

        private void ChangeSkinCell(Skins skin)
        {
            if (skin.ToString().StartsWith("Bread"))
                BreadSkinAction(skin);
            else if (skin.ToString().StartsWith("Kitchen"))
                KitchenSkinAction(skin);
            else if (skin.ToString().StartsWith("Toaster"))
                ToasterSkinAction(skin);
        }

        private void ToasterSkinAction(Skins skin)
        {
            _selectedSkin = skin;
            ChangeSkinInfoPanel(skin);

            // Debug.Log(_selectedSkin.ToString() + "|||" + skin.ToString());
            // покупал ли игрок этот скин
            if (PlayerPrefs.HasKey("TOASTER_" + skin.ToString()))
            {
                if (PlayerPrefs.GetInt("TOASTER_" + skin.ToString()) == 1)
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
            if (PlayerPrefs.HasKey("TOASTER_EQUPPIED"))
            {
                if (PlayerPrefs.GetInt("TOASTER_EQUPPIED") == _selectedSkinIndex)// == (int)skin)
                {
                    SwitchToEquipped();
                }
            }
            else
            {
                PlayerPrefs.SetInt("TOASTER_EQUPPIED", _selectedSkinIndex);//, (int)skin);
                SwitchToEquipped();
            }
        }

        private void KitchenSkinAction(Skins skin)
        {
            _selectedSkin = skin;
            ChangeSkinInfoPanel(skin);

            // Debug.Log(_selectedSkin.ToString() + "|||" + skin.ToString());
            // покупал ли игрок этот скин
            if (PlayerPrefs.HasKey("TOASTER_" + skin.ToString()))
            {
                if (PlayerPrefs.GetInt("TOASTER_" + skin.ToString()) == 1)
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
            if (PlayerPrefs.HasKey("TOASTER_EQUPPIED"))
            {
                if (PlayerPrefs.GetInt("TOASTER_EQUPPIED") == _selectedSkinIndex)// == (int)skin)
                {
                    SwitchToEquipped();
                }
            }
            else
            {
                PlayerPrefs.SetInt("TOASTER_EQUPPIED", _selectedSkinIndex);//, (int)skin);
                SwitchToEquipped();
            }
        }

        private void BreadSkinAction(Skins skin)
        {
            _selectedSkin = skin;
            ChangeSkinInfoPanel(skin);

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
        private void DoActions_BreadSkins()
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

        private void DoActions_KitchenSkins()
        {
            // покупка скина
            if (_buyButtonObj.activeSelf)
            {
                // 1. вычитаем деньги 
                var moneyToRemove = _skinsPricing[_selectedSkin];
                PlayerPrefs.SetInt("all_coins", PlayerPrefs.GetInt("all_coins") - moneyToRemove);

                // 2. сохраняем покупку скина
                PlayerPrefs.SetInt("KITCHEN_" + _selectedSkin.ToString(), 1);

                // 3. меняем кнопку
                SwitchFromBuyToEquip();

                // 4. меняем показания монет
                _coinsText.text = (int.Parse(_coinsText.text) - moneyToRemove).ToString();
            }

            // надевание скина
            else if (_notEquipedButtonObj.activeSelf)
            {
                // меняем скин
                _skinChanger_kitchen.SetWallAndFloor(_skinChanger_kitchen.WallsAndFloors[_selectedSkinIndex][0], 
                                                    _skinChanger_kitchen.WallsAndFloors[_selectedSkinIndex][1]);
                
                // надеваем выбранный
                PlayerPrefs.SetInt("KITCHEN_EQUPPIED", _selectedSkinIndex);//, (int)_selectedSkin);
                SwitchToEquipped();
            }
        }

        private void DoActions_ToasterSkins()
        {
            // покупка скина
            if (_buyButtonObj.activeSelf)
            {
                // 1. вычитаем деньги 
                var moneyToRemove = _skinsPricing[_selectedSkin];
                PlayerPrefs.SetInt("all_coins", PlayerPrefs.GetInt("all_coins") - moneyToRemove);

                // 2. сохраняем покупку скина
                PlayerPrefs.SetInt("TOASTER_" + _selectedSkin.ToString(), 1);

                // 3. меняем кнопку
                SwitchFromBuyToEquip();

                // 4. меняем показания монет
                _coinsText.text = (int.Parse(_coinsText.text) - moneyToRemove).ToString();
            }

            // надевание скина
            else if (_notEquipedButtonObj.activeSelf)
            {
                // меняем скин
                _skinChanger_toaster.SetToasterSkin(_selectedSkinIndex);
                
                // надеваем выбранный
                PlayerPrefs.SetInt("TOASTER_EQUPPIED", _selectedSkinIndex);//, (int)_selectedSkin);
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
            Debug.LogWarning(_categories.GetComponent<Categories>().breadCategoryPrefab.GetComponent<Image>().sprite.name);
            ChangeShownCategory(CategoryType.Bread);
            _buyingCells.GetComponent<BuyingCells>().SpawnedItems[0].GetComponent<MarketCell>().buttonToggle.isOn = true;
        }
    }
}
