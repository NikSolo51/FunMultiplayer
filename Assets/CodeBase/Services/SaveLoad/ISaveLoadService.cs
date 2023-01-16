using CodeBase.Data;

namespace CodeBase.Services.SaveLoad
{
    public interface ISaveLoadService 
    {
        void SaveProgress();
        PlayerProgress LoadProgress();

        void Register(ISavedProgressReader progressReader);
        void InformProgressReaders();
        void CleanUp();
    }
}