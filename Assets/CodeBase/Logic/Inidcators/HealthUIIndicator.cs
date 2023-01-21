namespace CodeBase.Logic.Inidcators
{
    public class HealthUIIndicator : UIIndicator
    {
        public override void AnimateIndicator(float percent)
        {
            _indicator.fillAmount = percent;
        }
    }
}