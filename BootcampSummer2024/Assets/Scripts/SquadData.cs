public class SquadData {
    public SquadData() {
    }

    public SquadData(int squadIndex, int count) {
        Index = squadIndex;
        Count = count;
    }

    public int Index { get; private set; }

    public int Count { get; private set; }

    public void SetSquadIndex(int index) {
        Index = index;
    }

    public void SetCount(int count) {
        Count = count;
    }
}
