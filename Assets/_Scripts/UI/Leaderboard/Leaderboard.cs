using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BreadFlip
{
    public class Leaderboard : MonoBehaviour
    {
        private const int count = 10;
        private List<GameObject> items = new List<GameObject>();

        [SerializeField] GameObject ItemPrefab;

        private int from;

        private void OnEnable()
        {
            from = -count;
            ShowNext();
        }

        private void AddItem(DBPlayer player, int id)
        {
            var item = GameObject.Instantiate(ItemPrefab, gameObject.transform);
            item.GetComponent<Item>().SetData(player, id);
            items.Add(item);
        }

        private void ClearSpace()
        {
           foreach(var el in items)
            {
                GameObject.Destroy(el);
            }
            items.Clear();
        }

        public void ShowNext()
        {
            ClearSpace();
            from += count;
            var players = DBInterface.GetRecords(from, count);

            int i = from + 1;
            foreach (var el in players.record_list)
            {
                AddItem(el, i);
                ++i;
            }
        }

        public void ShowPrevious()
        {
            ClearSpace();
            from -= count;
            var players = DBInterface.GetRecords(from, count);

            int i = from + 1;
            foreach (var el in players.record_list)
            {
                AddItem(el, i);
                ++i;
            }
        }

    }
}
