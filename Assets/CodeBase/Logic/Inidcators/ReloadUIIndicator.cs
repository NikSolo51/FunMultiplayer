namespace CodeBase.Logic.Inidcators
{
    public class ReloadUIIndicator : UIIndicator
    {
        public override void AnimateIndicator(float percent)
        {
            _indicator.fillAmount = percent;
        }
    }
}