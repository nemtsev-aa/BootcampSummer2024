using System.Collections.Generic;

public class CoinCounter {
    private List<Coin> _coins;
    private List<Coin> _coinsFromLevel;

    public CoinCounter() {
        _coins = new List<Coin>();
        _coinsFromLevel = new List<Coin>();
    }

    public void AddCoinInLevelList(Coin coin) {
        _coinsFromLevel.Add(coin);
        coin.gameObject.SetActive(false);
    }

    public void AddCoinInGeneralList() {
        
        if (_coinsFromLevel.Count > 0)
            _coins.AddRange(_coinsFromLevel);
    }

    public void RemoveCoinsFromLevel() {
        _coinsFromLevel.Clear();
    }

    public int GetPointsFromLevel() {
        int points = 0;

        if (_coinsFromLevel.Count > 0) {
            foreach (Coin iCoin in _coinsFromLevel) {
                points += iCoin.Value;
            }

            return points;
        }
        else
            return 0;
    }

    public int GetAllPoints() {
        int points = 0;

        if (_coins.Count > 0) {
            foreach (Coin iCoin in _coins) {
                points += iCoin.Value;
            }

            return points;
        }
        else
            return 0;
    }
}
