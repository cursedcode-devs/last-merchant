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
        int tradeTier = Random.Range(0, 3); // 0: Mały, 1: Średni, 2: Duży

        switch (tradeTier)
        {
            case 0: // Mała pomoc
                give = Random.Range(5, 12);
                recive = Mathf.RoundToInt(give * Random.Range(2.0f, 3.0f));
                break;
            case 1: // Standardowa wymiana
                give = Random.Range(15, 25);
                recive = Mathf.RoundToInt(give * Random.Range(1.5f, 2.0f));
                break;
            case 2: // Wielka ekspedycja
                give = Random.Range(30, 50);
                recive = Mathf.RoundToInt(give * Random.Range(1.3f, 1.6f));
                break;
        }

        // Zabezpieczenie: gracz zawsze dostaje więcej niż daje (minimum +2 jednostki)
        if (recive < give + 2) recive = give + 2;
        switch (planetTypesGive)
        {
            case PlanetTypes.Parts:
                {
                    tradeText = "Daj mi " + give + " cz\u0119\u015Bci, ";
                    break;
                }
            case PlanetTypes.Food:
                {
                    tradeText = "Daj mi " + give + " jedzenia, ";
                    break;
                }
            case PlanetTypes.Fuel:
                {
                    tradeText = "Daj mi " + give + " paliwa, ";
                    break;
                }
        }

        switch (planetTypesRecive)
        {
            case PlanetTypes.Parts:
                {
                    tradeText += "a ja dam ci " + recive + " cz\u0119\u015Bci";
                    break;
                }
            case PlanetTypes.Food:
                {
                    tradeText += "a ja dam ci " + recive + " jedzenia";
                    break;
                }
            case PlanetTypes.Fuel:
                {
                    tradeText += "a ja dam ci " + recive + " paliwa";
                    break;
                }
        }


        isTradeGenereted = true;
    }
}
