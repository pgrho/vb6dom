namespace Shipwreck.VB6Models.Forms
{ 
    public sealed class CommandButton : Control
    {
        public string Caption
        {
            get => GetPropertyAsString();
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