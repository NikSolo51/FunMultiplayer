namespace CodeBase.Services.Update
{
    public interface IUpdatable : ICacheUpdate
    {
        public void UpdateTick();
    }
}