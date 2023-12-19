using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BreadFlip
{
    public class Item : MonoBehaviour
    {
        [SerializeField] TMP_Text Num;
        [SerializeField] TMP_Text Login;
        [SerializeField] TMP_Text Record;
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
            Num.text = id.ToString();
            Login.text = player.player;
            Record.text = player.record.ToString();
        }
    }
}
