namespace CodeBase.Services.Update
{
    public interface IFixedUpdatable : ICacheUpdate
    {
        public void FixedUpdateTick();
    }
}