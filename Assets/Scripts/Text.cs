using UnityEngine;
using System.Threading.Tasks;
using TMPro;

public class Text : MonoBehaviour
{
    public TextMeshProUGUI text;


    public async void SetText(float damage)
    {
        text.text = "" + damage;

        await Task.Delay(1000);

        Destroy(gameObject);
    }
}
