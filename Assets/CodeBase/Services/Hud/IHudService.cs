namespace CodeBase.Services.Hud
{
    public interface IHudService 
    {
        void ChangeState(HudState hudState);

        HudState GetState();
    }
}