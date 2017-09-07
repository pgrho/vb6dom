namespace Shipwreck.VB6Models.Forms
{
    [FormType("VB.CommandButton")]
    public sealed class CommandButton : Control
    {
        public string Caption
        {
            get => GetProperty();
            set => SetProperty(value);
        }

        // 1  'ｸﾞﾗﾌｨｯｸｽ
        public int? Style
        {
            get => GetPropertyAsInt32();
            set => SetProperty(value);
        }
    }
}