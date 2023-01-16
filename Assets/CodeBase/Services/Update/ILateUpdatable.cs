namespace CodeBase.Services.Update
{
    public interface ILateUpdatable : ICacheUpdate
    {
        public void LateUpdateTick();
    }
}