using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textScore;
    [SerializeField] private List<Image> listHeart;
    [SerializeField] private Sprite spHeartFull;
    [SerializeField] private Sprite spHeartEmpty;

    public void UpdateTexteScore(int _score)
    {
        textScore.text = _score.ToString();
    }

    public void UpdateHearts(int _life,int _maxLife)
    {
        for (int i = 0; i < _maxLife; i++)
        {
            listHeart[i].sprite = i < _life ? spHeartFull : spHeartEmpty;
        }
    }
}
