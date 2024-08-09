public class Example : SheetFetcher 
{
    public string Name;
    public float HP;
    public float DEF;
    public float ATK;


    public override void OnComplete(string data)
    {
        var list = GetRowByFirstColumn(data, Name);
        HP = float.Parse(list[1]);
        DEF = float.Parse(list[2]);
        ATK = float.Parse(list[3]); 
    }
}
