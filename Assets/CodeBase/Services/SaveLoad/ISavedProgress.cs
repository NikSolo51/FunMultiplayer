using CodeBase.Data;

namespace CodeBase.Services.SaveLoad
{
    public interface ISavedProgress : ISavedProgressReader
    {
        void UpdateProgress(PlayerProgress progress);
    }
}