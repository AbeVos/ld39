using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatController : MonoBehaviour
{
    private Player player;
    [SerializeField]
    private Image[] bars;
    private float barScale;

    private void Awake()
    {
    }

    private void Start()
    {
        player = GameManager.Player;
    }

    private void Update()
    {
        float battery = bars.Length * player.Battery / player.MaxCharge;

        for (int i = 0; i < bars.Length; i++)
        {
            bars[i].fillAmount = battery - (bars.Length - i-1);
        }
    }
}
