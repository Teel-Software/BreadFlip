using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace BreadFlip
{
    public class Item : MonoBehaviour
    {
        [SerializeField] TMP_Text Num;
        [SerializeField] TMP_Text Login;
        [SerializeField] TMP_Text Record;
        [SerializeField] Sprite Player;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void SetData(DBPlayer player, int id)
        {
            Num.text = id.ToString() + ".";
            Login.text = player.player;
            if(PlayerPrefs.GetInt("PlayerId", -1) == player.id)
            {
                gameObject.GetComponent<Image>().sprite = Player;
                var color = new Color32(99, 53, 31, 255);
                Login.color = color;
                Num.color = color;
                Record.color = color;
            }
            Record.text = player.record.ToString();
        }
    }
}
