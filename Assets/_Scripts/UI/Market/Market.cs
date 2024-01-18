using System;
using System.Collections.Generic;
using BreadFlip.Entities.Skins;
using TMPro;
using UnityEngine;
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
        [SerializeField] private TMP_Text _skinPrice;
        [SerializeField] private TMP_Text _skinName;

        [Header("Skin Changing")]
        [SerializeField] private ToastSkinChanger _skinChanger;
        private Skins _selectedSkin;

        private Dictionary<Skins, int> _skinsPricing = new Dictionary<Skins, int>
        {
            {Skins.DefaultSkin, 0},
            {Skins.NotDefaultSkin, 4},
            {Skins.CatSkin, 5},
            {Skins.CorgiAssSkin, 100000}

        };

        private Dictionary<Skins, string> _skinsNaming = new Dictionary<Skins, string>
        {
            {Skins.DefaultSkin, "Хлеб"},
            {Skins.NotDefaultSkin, "Надкусанный хлеб"},
            {Skins.CatSkin, "Котохлеб"},
            {Skins.CorgiAssSkin, "Булочки корги"}
        };

        private List<Sprite> SkinsImages = new List<Sprite>();

        private void OnEnable() {
                       
            // отображаем имеющиеся монеты
            if (PlayerPrefs.HasKey("all_coins"))
            {
                _coinsText.text = PlayerPrefs.GetInt("all_coins").ToString();
            }

            _buyButton.onClick.AddListener(someMethod);

            FillSkinsList();

            MarketCell.SkinSelected += ChangeBigImage;
            MarketCell.SkinSelected += ChangeSkinPrice;
            MarketCell.SkinSelected += ChangeSkinName;
            MarketCell.SkinSelected += (Skins skin) => _selectedSkin = skin;

            // TODO меняем контент кнопки и её логику
        }

        private void OnDisable() {
            MarketCell.SkinSelected -= ChangeBigImage;
            MarketCell.SkinSelected -= ChangeSkinPrice;
            MarketCell.SkinSelected -= ChangeSkinName;
            MarketCell.SkinSelected -= (Skins skin) => _selectedSkin = skin;
        }

        private void FillSkinsList()
        {
            var list = _buyingCells.GetComponent<BuyingCells>()._cellsPrefabs;
            for (int i = 0; i < list.Count; i++)
            {
                SkinsImages.Add(list[i].GetComponentsInChildren<Image>()[1].sprite);
                
            }
        }

        private void ChangeBigImage(Skins skin)
        {
            _bigImage.sprite = SkinsImages[(int)skin];
        }

        private void ChangeSkinPrice(Skins skin)
        {
            if (_skinsPricing[skin] > 0)
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
                }
                else
                {
                    _buyButton.interactable = true;
                    foreach (var comp in _buyButtonObj.GetComponents<ChangeTextColor>())
                    {
                        comp.ChangeColorToDefault();
                    }
                }

            }
            else
            {
                SwitchFromBuyToEquip();
            }
        }

        private void ChangeSkinName(Skins skin)
        {
            _skinName.text = _skinsNaming[skin];
        }

        private void someMethod()
        {
            if (_buyButtonObj.activeSelf)
            {
                // логика покупки
            }

            else if (_notEquipedButtonObj.activeSelf)
            {
                _skinChanger.ChangeSkin(_skinChanger.skins[(int)_selectedSkin]);
                SwitchToEquipped();
            }
        }

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

        public void SetDefaultView()
        {
            _buyingCells.GetComponent<BuyingCells>().SpawnedItems[0].GetComponent<MarketCell>().buttonToggle.isOn = true;
            _buyingCells.GetComponent<BuyingCells>().SpawnedItems[0].GetComponent<Image>().sprite = 
                                                    _buyingCells.GetComponent<BuyingCells>().SpawnedItems[0].GetComponent<MarketCell>().
                                                    buttonToggle.spriteState.selectedSprite;
        }
    }
}
