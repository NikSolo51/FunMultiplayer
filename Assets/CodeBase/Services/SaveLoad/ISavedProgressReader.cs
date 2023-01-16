using CodeBase.Data;

namespace CodeBase.Services.SaveLoad
{
    public interface ISavedProgressReader
    {
        void LoadProgress(PlayerProgress progress);
    }
}