namespace CodeBase.Services.Randomaizer
{
    public interface IRandomService 
    {
        int Next(int minValue, int maxValue);
    }
}