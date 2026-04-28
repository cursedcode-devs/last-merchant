using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Trade
{
    public int give;
    public int recive;
    public int restoreHealth = 25;
    public string tradeText;
    public bool isTradeGenereted = true;
    public Trade(PlanetTypes planetTypesRecive, PlanetTypes planetTypesGive)
    {
        GenerateTrade(planetTypesRecive, planetTypesGive);
    }

    public int GetRecive()
        { return recive; }

    public int GetGive()
    { return give; }

    public void GenerateTrade(PlanetTypes planetTypesRecive, PlanetTypes planetTypesGive)
    {
        int tradeTier = Random.Range(0, 3);

        switch (tradeTier)
        {
            case 0: // mala pomoc
                give = Random.Range(5, 15);
                recive = Mathf.RoundToInt(give * Random.Range(2.0f, 3.0f));
                restoreHealth = 25;
                break;
            case 1: // standardowa wymiana
                give = Random.Range(15, 26);
                recive = Mathf.RoundToInt(give * Random.Range(1.5f, 2.0f));
                restoreHealth = 35;
                break;
            case 2: // wielka ekspedycja
                give = Random.Range(26, 41);
                recive = Mathf.RoundToInt(give * Random.Range(1.3f, 1.6f));
                restoreHealth = 50;
                break;
        }

        if (recive < give + 2) recive = give + 2;
        switch (planetTypesGive)
        {
            case PlanetTypes.Parts:
                {
                    tradeText = "Daj mi " + give + "  <voffset=0.2em><sprite name=\"parts\"></voffset>";
                    break;
                }
            case PlanetTypes.Food:
                {
                    tradeText = "Daj mi " + give + "  <voffset=0.2em><sprite name=\"food\"></voffset>";
                    break;
                }
            case PlanetTypes.Fuel:
                {
                    tradeText = "Daj mi " + give + $"  <voffset=0.2em><sprite name=\"fuel\"></voffset>";
                    break;
                }
        }

        switch (planetTypesRecive)
        {
            case PlanetTypes.Parts:
                {
                    tradeText += "a ja dam ci " + recive + "  <voffset=0.2em><sprite name=\"parts\"></voffset>";
                    break;
                }
            case PlanetTypes.Food:
                {
                    tradeText += "a ja dam ci " + recive + "  <voffset=0.2em><sprite name=\"food\"></voffset>";
                    break;
                }
            case PlanetTypes.Fuel:
                {
                    tradeText += "a ja dam ci " + recive + $"  <voffset=0.2em><sprite name=\"fuel\"></voffset>";
                    break;
                }
        }


        isTradeGenereted = true;
    }
}
