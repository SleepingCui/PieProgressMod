namespace PieProgressMod
{
    public class MainSettings : UnityModManagerNet.UnityModManager.ModSettings
    {
        public float posx { get; set; } = -80f;
        public float posy { get; set; } = -80f;
        public float scale { get; set; } = 1.0f;

        public bool usex { get; set; } = true;
        public float xoffx { get; set; } = 30f;
        public float xoffy { get; set; } = 0f;
        public string font { get; set; } = "Arial";
        public string xfmt { get; set; } = "{0}%";

        public int fillr { get; set; } = 255;
        public int fillg { get; set; } = 255;
        public int fillb { get; set; } = 255;
        public int filla { get; set; } = 150;

        public int passr { get; set; } = 0;
        public int passg { get; set; } = 255;
        public int passb { get; set; } = 0;
        public int passa { get; set; } = 150;

        public int bgr { get; set; } = 0;
        public int bgg { get; set; } = 0;
        public int bgb { get; set; } = 0;
        public int bga { get; set; } = 102;

        public int borderr { get; set; } = 255;
        public int borderg { get; set; } = 255;
        public int borderb { get; set; } = 255;
        public int bordera { get; set; } = 0;

        public override void Save(UnityModManagerNet.UnityModManager.ModEntry entry)
        {
            Save(this, entry);
        }
    }
}