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
        [SerializeField] private TMP_Text _skinPrice;
        [SerializeField] private TMP_Text _skinName;

        [Header("Skin Changing")]
        [SerializeField] private ToastSkinChanger _skinChanger;
        private Skins _selectedSkin;

        private Dictionary<Skins, int> _skinsPricing = new Dictionary<Skins, int>
        {
            {Skins.DefaultSkin, 0},
            {Skins.NotDefaultSkin, 500},
            {Skins.CatSkin, 1000},
            {Skins.CorgiAssSkin, 5000}

        };

        private Dictionary<Skins, string> _skinsNaming = new Dictionary<Skins, string>
        {
            {Skins.DefaultSkin, "Хлеб"},
            {Skins.NotDefaultSkin, "Надкусанный хлеб"},
            {Skins.CatSkin, "Котохлеб"},
            {Skins.CorgiAssSkin, "Булочки корги"}
        };

        private List<Sprite> SkinsImages = new List<Sprite>();

        public static int EquippedSkin = -1;

        private void OnEnable() {
                       
            // отображаем имеющиеся монеты
            if (PlayerPrefs.HasKey("all_coins"))
            {
                _coinsText.text = PlayerPrefs.GetInt("all_coins").ToString();
            }

            _buyButton.onClick.AddListener(DoButtonActions);

            FillSkinsList();

            // скин DefaultSkin по умолчанию есть у игрока
            if (!PlayerPrefs.HasKey("SKIN_" + Skins.DefaultSkin.ToString()))
                PlayerPrefs.SetInt("SKIN_" + Skins.DefaultSkin.ToString(), 1);

            // задаём уже выбранный скин как надетый


            MarketCell.SkinSelected += ChangeSkinCell;
            // MarketCell.SkinSelected += SetSelectedSkin;
            // MarketCell.SkinSelected += (Skins skin) => _selectedSkin = skin;
        }

        private void OnDisable() {
            MarketCell.SkinSelected -= ChangeSkinCell;
            // MarketCell.SkinSelected -= SetSelectedSkin;
        }

        private void FillSkinsList()
        {
            var list = _buyingCells.GetComponent<BuyingCells>()._cellsPrefabs;
            for (int i = 0; i < list.Count; i++)
            {
                SkinsImages.Add(list[i].GetComponentsInChildren<Image>()[1].sprite);
            }
        }

#region Methods for selecting cell in Market
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
            /* else */ if (PlayerPrefs.HasKey("SKIN_EQUPPIED"))
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

#region Market button logic
        private void DoButtonActions()
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
                _skinChanger.ChangeSkin((int)_selectedSkin);
                
                // снимамем все скины
                // foreach (Skins skin in Enum.GetValues(typeof(Skins)))
                // {
                //     PlayerPrefs.SetInt("SKIN_EQUPPIED" + skin.ToString(), 0);
                // }
                // надеваем выбранный
                PlayerPrefs.SetInt("SKIN_EQUPPIED", (int)_selectedSkin);
                SwitchToEquipped();
            }
        }
#endregion

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

        // срабатывает при нажатии на кнопку магазина в Главном Меню, задаёт начальный выбранный скин
        public void SetDefaultView()
        {
            _buyingCells.GetComponent<BuyingCells>().SpawnedItems[0].GetComponent<MarketCell>().buttonToggle.isOn = true;
            // _buyingCells.GetComponent<BuyingCells>().SpawnedItems[0].GetComponent<Image>().sprite = 
            //                                         _buyingCells.GetComponent<BuyingCells>().SpawnedItems[0].GetComponent<MarketCell>().
            //                                         buttonToggle.spriteState.selectedSprite;
            // MarketCell.SkinSelected.Invoke(Skins.DefaultSkin);
        }
    }
}
