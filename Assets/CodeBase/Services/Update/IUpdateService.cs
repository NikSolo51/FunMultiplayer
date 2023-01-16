namespace CodeBase.Services.Update
{
    public interface IUpdateService 
    {
        void Update();
        void FixedUpdate();
        void LateUpdate();
        void Register(ICacheUpdate obj);
        void Unregister(ICacheUpdate obj);
    }
}