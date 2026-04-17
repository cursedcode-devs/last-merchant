using UnityEngine;

public class Trade
{
    public int give;
    public int recive;
    public int restoreHealth = 15;
    public string tradeText;
    public bool isTradeGenereted = true;
    public Trade(PlanetTypes planetTypesRecive, PlanetTypes planetTypesGive)
    {
        GenerateTrade(planetTypesRecive, planetTypesGive);
    }

    public int GetRecive()
        { return recive; }

    public void GenerateTrade(PlanetTypes planetTypesRecive, PlanetTypes planetTypesGive)
    {
        recive = 17 + Random.Range(0, 4);
        give = 12 + Random.Range(0, 3);
        switch (planetTypesGive)
        {
            case PlanetTypes.Parts:
                {
                    tradeText = "Daj mi " + give + " czêœci, ";
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
                    tradeText += "a ja ci dam " + recive + " czêœci";
                    break;
                }
            case PlanetTypes.Food:
                {
                    tradeText += "a ja ci dam " + recive + " jedzenia";
                    break;
                }
            case PlanetTypes.Fuel:
                {
                    tradeText += "a ja ci dam " + recive + " paliwa";
                    break;
                }
        }

        tradeText += " Wciœnij Y, by zaakceptowaæ wymianê, N by odmówiæ.";
        isTradeGenereted = true;
    }
}
