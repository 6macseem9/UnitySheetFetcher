public class Example : SheetFetcher 
{
    public string Name;
    public float HP;
    public float DEF;
    public float ATK;


    public override void HandleFetchedData(string data)
    {
        var list = GetRowByFirstColumn(Name);
        HP = float.Parse(list[1]);
        DEF = float.Parse(list[2]);
        ATK = float.Parse(list[3]); 
    }
}
