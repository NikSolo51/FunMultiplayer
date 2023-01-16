namespace CodeBase.Services.Hud
{
    public class HudStateService : IHudService
    {
        private HudState _hudState;

        public void ChangeState(HudState hudState)
        {
            if (_hudState != hudState)
                _hudState = hudState;
        }

        public HudState GetState()
        {
            return _hudState;
        }
    }
}